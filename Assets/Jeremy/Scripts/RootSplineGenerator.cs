using System.Collections;
using UnityEngine;

public class RootSplineGenerator : MonoBehaviour
{
    public MinMaxRange numberOfControlPointsRange;
    public MinMaxRange scaleRange;
    public MinMaxRange noiseRange;
    public MinMaxRange angleVariationRange;

    [Space]
    public MinMaxRange coneRadiusRange;
    public MinMaxRange coneDistanceRange;

    [Space]
    public Vector3 startPosition = Vector3.zero;
    public Vector3 endPosition = Vector3.forward;

    private Vector3[] controlPoints;

    [Space]
    public LineRenderer lineRenderer;
    public float growTime = 1.5f;
    public float shrinkTime = .35f;

    private Coroutine growRoutine;

    [Space]
    public bool generateUpdate;
    public float generateDelay = 1f;
    private float _generateDelay;

    private void Start()
    {
        Generate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Generate();

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
    private void Generate()
    {/*
        int numberOfControlPoints = (int)numberOfControlPointsRange.GetRandomValue();
        float scale = scaleRange.GetRandomValue();
        float noise = noiseRange.GetRandomValue();

        controlPoints = new Vector3[numberOfControlPoints];
        controlPoints[0] = startPosition;
        controlPoints[numberOfControlPoints - 1] = endPosition;
        for (int i = 1; i < numberOfControlPoints - 1; i++)
        {
            float t = (float)i / (float)(numberOfControlPoints - 1);
            float x = Mathf.Lerp(startPosition.x, endPosition.x, t);
            float y = Mathf.Lerp(startPosition.y, endPosition.y, t);
            float z = Mathf.Lerp(startPosition.z, endPosition.z, t);
            float offsetX = Mathf.PerlinNoise(x * noise, y * noise + Time.time) * scale;
            float offsetY = Mathf.PerlinNoise(y * noise + Time.time, z * noise) * scale;
            float offsetZ = Mathf.PerlinNoise(z * noise, x * noise + Time.time) * scale;
            controlPoints[i] = new Vector3(x + offsetX, y + offsetY, z + offsetZ);
        }

        lineRenderer.positionCount = numberOfControlPoints;
        lineRenderer.SetPositions(controlPoints);
        */
        
        // Random end position within an upside down cone
        float radius = coneRadiusRange.GetRandomValue();
        float angle = angleVariationRange.GetRandomValue();

        float coneX = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        float coneZ = radius * Mathf.Cos(angle * Mathf.Deg2Rad);

        endPosition = new Vector3(coneX, coneDistanceRange.GetRandomValue(), coneZ);


        // Generate control points
        int numberOfControlPoints = (int)numberOfControlPointsRange.GetRandomValue();
        float scale = scaleRange.GetRandomValue();
        float noise = noiseRange.GetRandomValue();
        float angleVariation = angleVariationRange.GetRandomValue();

        controlPoints = new Vector3[numberOfControlPoints];
        controlPoints[0] = startPosition;
        controlPoints[numberOfControlPoints - 1] = endPosition;
        for (int i = 1; i < numberOfControlPoints - 1; i++)
        {
            float t = (float)i / (float)(numberOfControlPoints - 1);
            float x = Mathf.Lerp(startPosition.x, endPosition.x, t);
            float y = Mathf.Lerp(startPosition.y, endPosition.y, t);
            float z = Mathf.Lerp(startPosition.z, endPosition.z, t);
            float offsetX = Mathf.PerlinNoise(x * noise, y * noise) * scale * Mathf.Sin(angleVariation * Mathf.Deg2Rad);
            float offsetY = Mathf.PerlinNoise(y * noise, z * noise) * scale * Mathf.Sin(angleVariation * Mathf.Deg2Rad);
            float offsetZ = Mathf.PerlinNoise(z * noise, x * noise) * scale * Mathf.Cos(angleVariation * Mathf.Deg2Rad);
            controlPoints[i] = new Vector3(x + offsetX, y + offsetY, z + offsetZ);
        }


        // Update Line Renderer
        lineRenderer.positionCount = numberOfControlPoints;
        lineRenderer.SetPositions(controlPoints);

        

        if (growRoutine != null)
            StopCoroutine(growRoutine);
        growRoutine = StartCoroutine(GrowRoot());
    }

    private IEnumerator GrowRoot()
    {
        float time = 0.05f;
        float endValue = lineRenderer.widthCurve.keys[1].value;
        AnimationCurve curve = new AnimationCurve();
        while (time < growTime)
        {
            curve.AddKey(.0f, 1.0f);
            curve.AddKey(1.0f, endValue);
            lineRenderer.widthCurve.MoveKey(1, new Keyframe(time / growTime, endValue));
            time += Time.deltaTime;
            yield return null;
        }
    }
}
