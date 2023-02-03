using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
    public float waterAmount = 10f;

    public void UseWater(float amount)
    {
        waterAmount -= amount;
    }
}
