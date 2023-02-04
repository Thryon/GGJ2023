using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
    public float waterAmount = 10f;
    public float maxAmount = 100f;
    public float maxLvl = 0f;
    public float minLvl = -3f;
    private Transform WaterTransform;

    public void UseWater(float amount)
    {
        waterAmount -= amount;
        WaterTransform.localPosition = new Vector3(WaterTransform.localPosition.x, Mathf.Lerp(maxLvl, minLvl, waterAmount / maxAmount), WaterTransform.localPosition.z);
        if (waterAmount < 0)
        {
            waterAmount = 0;
        }

    }
    public void RefillWater(float amount)
    {
        waterAmount += amount;
        WaterTransform.localPosition = new Vector3(WaterTransform.localPosition.x, Mathf.Lerp(maxLvl, minLvl, waterAmount / maxAmount), WaterTransform.localPosition.z);
        if (waterAmount > maxAmount)
        {
            waterAmount = maxAmount;
        }
    }
}
