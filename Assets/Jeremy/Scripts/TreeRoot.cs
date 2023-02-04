using UnityEngine;

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
    // The line renderer to render the root
    public LineRenderer lineRenderer;

    public Vector3[] points;

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
    }

    private void Update()
    {
        Generate();
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
}
