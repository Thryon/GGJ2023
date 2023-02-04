using UnityEngine;

[System.Serializable]
public class MinMaxRange
{
    public Vector2 minMax;

    public float GetRandomValue()
    {
        return Random.Range(minMax.x, minMax.y);
    }
}
