using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
    public int currentAmount = 10;
    public int maxAmount = 100;
    public float minLvl = -5f;
    public bool fillAtStart = true;

    public float lerpSpeed = 2f;

    public float refillInterval = 30f;
    public float refillTime = 0f;

    public bool use10;
    public bool refill10;

    private float maxLvl;
    private Vector3 targetPosition;

    private void Start()
    {
        maxLvl = transform.localPosition.y;
        minLvl = transform.localPosition.y + minLvl;

        if (fillAtStart)
        {
            currentAmount = maxAmount;
            transform.localPosition = targetPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(minLvl, maxLvl, currentAmount / maxAmount), transform.localPosition.z);
        }
    }

    private void Update()
    {
        refillTime += Time.deltaTime;
        if (refillTime >= refillInterval)
        {
            refillTime = 0f;
            RefillWater(Random.Range(100, 500));
            RefreshWaterLevel();
        }
        if (use10)
        {
            UseWater(10);
            use10 = false;
        }
        if (refill10)
        {
            RefillWater(10);
            refill10 = false;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * lerpSpeed);
    }

    public void RefreshWaterLevel()
    {
        targetPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(minLvl, maxLvl, (float)currentAmount / maxAmount), transform.localPosition.z);
    }

    public void UseWater(int amount)
    {
        currentAmount -= amount;
        if (currentAmount < 0)
        {
            currentAmount = 0;
        }
        RefreshWaterLevel();

    }
    public void RefillWater(int amount)
    {
        currentAmount += amount;
        if (currentAmount > maxAmount)
        {
            currentAmount = maxAmount;
        }
        RefreshWaterLevel();
    }
}
