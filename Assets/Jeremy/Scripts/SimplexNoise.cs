using UnityEngine;

public class SimplexNoise : MonoBehaviour
{
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
}
