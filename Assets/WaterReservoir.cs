using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterReservoir : MonoBehaviour
{
    [SerializeField] private int maxAmount = 100;
    [SerializeField] private bool playerReservoir = false;
    public UnityEvent<float> OnFillRateChanged;
    public int MaxAmount => maxAmount;
    [SerializeField]
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
            OnFillRateChanged?.Invoke(FillValue);
        }
    }

    public float FillValue => (float)amount / maxAmount;

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
        if(playerReservoir)
            GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnLoseWater, Amount);
        return Mathf.Max(rest, 0);
    }

    public int RefillWater(int amount)
    {
        int overflow = (amount + this.amount) - maxAmount;
        if (overflow > 0)
            amount = amount - overflow;
        Amount += amount;
        if(playerReservoir)
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

    public void SetMaxAmount(int amount)
    {
        maxAmount = amount;
    }
}
