using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public class TreeRoot : MonoBehaviour
{
    public enum NoiseMethod { perlin, simplex, curves}

    // End point of the root
    public Transform endPoint;
    // Number of segments for the root
    public int baseSegments = 10;

    public NoiseMethod noiseMethod;
    
    [Header("Curves")]
    public MinMaxCurve offsetX = new MinMaxCurve(1, new AnimationCurve(), new AnimationCurve());
    public MinMaxCurve offsetY = new MinMaxCurve(1, new AnimationCurve(), new AnimationCurve());
    public MinMaxCurve offsetZ = new MinMaxCurve(1, new AnimationCurve(), new AnimationCurve());
    
    
    [Header("Noises")]
    // Amplitude of the noise
    public float amplitude = 1.0f;
    // Frequency of the noise
    public float frequency = 0.5f;
    // Octaves of the noise
    public int octaves = 1;
    // Persistence of the noise
    public float persistence = 1.0f;
    // Seed for the noise
    public int seed = 0;

    [Space]
    public AnimationCurve heightOffset;
    public AnimationCurve rotationOffset;

    [Header("Rendering")]
    // The line renderer to render the root
    public LineRenderer lineRenderer;
    // The time it takes to grow the root
    public float growTime = 1.5f;
    // The time it takes to shrink the root
    public float shrinkTime = .35f;
    // Min and max size of the root according to distance with target
    public Vector2 minMaxWidth = new Vector2(1.5f, 4f);
    // Cheat buttons to force generate, grow or shrink
    public bool generate, grow, shrink;

    [Header("Generation")]
    // Activate this option to update the mesh in update
    public bool generateUpdate;
    // Delay betwean updates
    public float generateDelay = 1f;

    private float _generateDelay;
    private Vector3[] points;
    private Keyframe[] widthKeys;

    // Function to generate the Perlin noise
    float PerlinNoise2D(float x, float y)
    {
        float total = 0.0f;
        float frequency = this.frequency;
        float amplitude = this.amplitude;

        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(x * frequency + seed, y * frequency + seed) * amplitude;
            frequency *= 2;
            amplitude *= persistence;
        }

        return total;
    }

    // Function to generate the simplex noise
    float SimplexNoise2D(float x, float y)
    {
        float total = 0.0f;
        float frequency = this.frequency;
        float amplitude = this.amplitude;

        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(x * frequency + seed, y * frequency + seed) * amplitude;
            frequency *= 2;
            amplitude *= persistence;
        }

        return total;
    }

    private void Start()
    {
        seed = Random.Range(0, 9999);
        widthKeys = lineRenderer.widthCurve.keys;
    }

    private void Update()
    {
        if (generate)
        {
            generate = false;
            Generate();
        }
        if (grow)
        {
            grow = false;
            Grow();
        }
        if (shrink)
        {
            shrink = false;
            Shrink();
        }

        if (generateUpdate)
        {
            if (_generateDelay <= 0)
            {
                Generate();
                _generateDelay = generateDelay;
            }
            else
                _generateDelay -= Time.deltaTime;
        }
    }

    void Generate()
    {
        // Determines the amount of segments
        float distance = Vector3.Distance(transform.position, endPoint.position);
        int segments = (int)(baseSegments * (1f + distance / (2 * baseSegments)));
        Vector3 direction = (endPoint.position - transform.position).normalized;
        float heightDifference = Mathf.Max(endPoint.position.y - transform.position.y, 0f);

        // Initialize the points array
        points = new Vector3[segments];
        // Calculate the step between each segment
        Vector3 step = (endPoint.position - transform.position) / segments;

        switch (noiseMethod)
        {
            case NoiseMethod.perlin:

                // Generate the root
                for (int i = 0; i < segments; i++)
                {
                    Vector3 position = transform.position + step * i;
                    float angle = PerlinNoise2D(position.x, position.y) * 360.0f;
                    Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, angle);
                    transform.rotation = rotation;

                    position += Vector3.up * heightOffset.Evaluate((float)i / (segments - 1)) * heightDifference;
                    position += Quaternion.AngleAxis(rotationOffset.Evaluate((float)i / (segments - 1)), direction) * direction;

                    // Store the local position of the segment
                    points[i] = position + transform.up * step.magnitude;
                }
                break;

            case NoiseMethod.simplex:
                // Generate the root
                for (int i = 0; i < segments; i++)
                {
                    Vector3 position = transform.position + step * i;
                    float angle = SimplexNoise2D(position.x, position.y) * 360.0f;
                    Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, angle);
                    transform.rotation = rotation;

                    position += Vector3.up * heightOffset.Evaluate((float)i / (segments - 1)) * heightDifference;
                    position += Quaternion.AngleAxis(rotationOffset.Evaluate((float)i / (segments - 1)), direction) * direction;

                    // Store the local position of the segment
                    points[i] = position + transform.up * step.magnitude;
                }
                break;

            case NoiseMethod.curves:
                {
                    // Generate the root
                    for (int i = 0; i < segments; i++)
                    {
                        Vector3 position = transform.position + step * i;
                        float noise = SimplexNoise2D(position.x, position.y);
                        float curveRatio = (float)i / (segments - 1);

                        Vector3 point = new Vector3(
                            x: offsetX.Evaluate(curveRatio, noise),
                            y: offsetY.Evaluate(curveRatio, noise),
                            z: offsetZ.Evaluate(curveRatio, noise));

                        points[i] = Vector3.Lerp(transform.position, endPoint.position, curveRatio) + point * distance / 3;
                    }
                    break;
                }
        }

        // Update Line Renderer
        lineRenderer.positionCount = segments;
        lineRenderer.SetPositions(points);

        float width = minMaxWidth.x + (minMaxWidth.y - minMaxWidth.x) * (distance / 30);
        lineRenderer.widthMultiplier = width;

    }

    private Coroutine growRoutine;

    public void Grow()
    {
        if (growRoutine != null)
            StopCoroutine(growRoutine);
        growRoutine = StartCoroutine(GrowRoot());
    }
    public void Shrink()
    {
        if (growRoutine != null)
            StopCoroutine(growRoutine);
        growRoutine = StartCoroutine(ShrinkRoot());
    }
    private IEnumerator GrowRoot()
    {
        float time = 0.05f;

        AnimationCurve widthCurve = new AnimationCurve();
        Keyframe[] keys = lineRenderer.widthCurve.keys;
        keys[keys.Length - 1].value = 0;
        
        while (time < growTime)
        {
            for (int i = 0; i < widthKeys.Length; i++)
            {
                keys[i].time = widthKeys[i].time * (time / growTime);
            }
            widthCurve.keys = keys;
            lineRenderer.widthCurve = widthCurve;

            time += Time.deltaTime;
            yield return null;
        }
        widthCurve.keys = widthKeys;
        lineRenderer.widthCurve = widthCurve;
    }
    private IEnumerator ShrinkRoot()
    {
        float time = 0.05f;

        AnimationCurve widthCurve = new AnimationCurve();
        Keyframe[] keys = lineRenderer.widthCurve.keys;
        keys[keys.Length - 1].value = 0;
        
        while (time < shrinkTime)
        {
            for (int i = 0; i < widthKeys.Length; i++)
            {
                keys[i].time = widthKeys[i].time * (1 - time / shrinkTime);
            }
            widthCurve.keys = keys;
            lineRenderer.widthCurve = widthCurve;

            time += Time.deltaTime;
            yield return null;
        }
    }
}
