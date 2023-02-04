using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public int numPoints = 10;
    public float scale = 10;
    public float magnitude = 1;

    private Vector3[] path;

    private void Start()
    {
        Generate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Generate();
    }
    public LineRenderer lineRenderer;
    void Generate()
    {
        path = new Vector3[numPoints];
        path[0] = startPoint;
        path[numPoints - 1] = endPoint;

        for (int i = 1; i < numPoints - 1; i++)
        {
            float t = (float)i / numPoints;
            float noise = Mathf.PerlinNoise(t * scale, 0) * 2 - 1;
            path[i] = Vector3.Lerp(startPoint, endPoint, t) + Vector3.up * noise * magnitude;
        }

        // Update Line Renderer
        lineRenderer.positionCount = numPoints;
        lineRenderer.SetPositions(path);
    }
}
