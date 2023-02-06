using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWaterReservoirUISlider : MonoBehaviour
{
    [SerializeField] private Slider waterSlider;
    [SerializeField] private WaterReservoir waterReservoir;
    
    void Start()
    {
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnGainWater, OnGainWater);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnLoseWater, OnLoseWater);
    }

    void OnGainWater(int TotalAmount)
    {
        RefreshSliderValue();
    }
    
    void OnLoseWater(int TotalAmount)
    {
        RefreshSliderValue();
    }

    void RefreshSliderValue()
    {
        waterSlider.value = (float)waterReservoir.Amount / waterReservoir.MaxAmount;
    }

    private void OnDestroy()
    {
        if (GlobalEvents.Instance)
        {
            GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnGainWater, OnGainWater);
            GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnLoseWater, OnLoseWater);
        }
    }
}
