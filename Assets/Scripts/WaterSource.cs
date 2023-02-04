using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
    public float waterAmount = 10f;
    public float maxAmount = 100f;

    public void UseWater(float amount)
    {
        waterAmount -= amount;
        if (waterAmount < 0)
        {
            waterAmount = 0;
        }
    }
    public void refillWater(float amount)
    {
        waterAmount += amount;
        if (waterAmount > maxAmount)
        {
            waterAmount = maxAmount;
        }
    }
}
