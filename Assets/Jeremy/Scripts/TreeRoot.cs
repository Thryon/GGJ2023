using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeRoot : MonoBehaviour
{
    public enum NoiseMethod { perlin, simplex}

    // End point of the root
    public Transform endPoint;
    // Number of segments for the root
    public int baseSegments = 10;

    public NoiseMethod noiseMethod;
    // Amplitude of the noise
    public float amplitude = 1.0f;
    // Frequency of the noise
    public float frequency = 1.0f;
    // Octaves of the noise
    public int octaves = 1;
    // Persistence of the noise
    public float persistence = 1.0f;
    // Seed for the noise
    public int seed = 0;

    [Space]
    // The line renderer to render the root
    public LineRenderer lineRenderer;
    public float growTime = 1.5f;
    public float shrinkTime = .35f;
    public bool generate, grow, shrink;

    private Vector3[] points;
    private Keyframe[] widthKeys;

    [Space]
    public bool generateUpdate;
    public float generateDelay = 1f;
    private float _generateDelay;

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
        int segments = (int)(baseSegments * (1f + distance / baseSegments));
        // Initialize the points array
        points = new Vector3[segments];

        switch (noiseMethod)
        {
            case NoiseMethod.perlin:
                // Initialize the points array
                points = new Vector3[segments];

                // Calculate the step between each segment
                Vector3 stepPerlin = (endPoint.position - transform.position) / segments;

                // Generate the root
                for (int i = 0; i < segments; i++)
                {
                    Vector3 position = transform.position + stepPerlin * i;
                    float angle = PerlinNoise2D(position.x, position.y) * 360.0f;
                    Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, angle);
                    transform.rotation = rotation;

                    // Store the local position of the segment
                    points[i] = transform.localPosition;

                    // Draw the segment using lines or curves
                    Debug.DrawLine(position, position + transform.up * stepPerlin.magnitude, Color.white);

                    // Store the local position of the segment
                    points[i] = position + transform.up * stepPerlin.magnitude - transform.position;
                }
                break;

            case NoiseMethod.simplex:
                // Calculate the step between each segment
                Vector3 stepSimplex = (endPoint.position - transform.position) / segments;

                // Generate the root
                for (int i = 0; i < segments; i++)
                {
                    Vector3 position = transform.position + stepSimplex * i;
                    float angle = SimplexNoise2D(position.x, position.y) * 360.0f;
                    Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, angle);
                    transform.rotation = rotation;


                    // Draw the segment using lines or curves
                    Debug.DrawLine(position, position + transform.up * stepSimplex.magnitude, Color.white);

                    // Store the local position of the segment
                    points[i] = position + transform.up * stepSimplex.magnitude - transform.position;
                }
                break;
        }
        // Update Line Renderer
        lineRenderer.positionCount = segments;
        lineRenderer.SetPositions(points);

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
