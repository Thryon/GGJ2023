using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterReservoir : MonoBehaviour
{
    [SerializeField] private int maxAmount = 100;
    
    public int MaxAmount => maxAmount;
    
    private int amount = 0;

    public int Amount
    {
        get
        {
            return amount;
        }

        set
        {
            amount = value;
            amount = Mathf.Clamp(amount, 0, maxAmount);
            Debug.Log("Water left: " + amount);
        }
    }

    /// <summary>
    /// Returns the rest that couldn't be used
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int UseWater(int amount)
    {
        int rest = amount - this.amount;
        if (rest > 0)
            amount = amount - rest;
        Amount -= amount;
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnLoseWater, Amount);
        return Mathf.Max(rest, 0);
    }

    public int RefillWater(int amount)
    {
        int overflow = (amount + this.amount) - maxAmount;
        if (overflow > 0)
            amount = amount - overflow;
        Amount += amount;
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnGainWater, Amount);
        return Mathf.Max(overflow, 0);
    }

    public bool IsFull()
    {
        return amount == maxAmount;
    }
    
    public bool IsEmpty()
    {
        return amount == 0;
    }
}
