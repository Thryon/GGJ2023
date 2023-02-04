using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using TMPro;
using UnityEngine;

public class PlayerHUDController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private WaterReservoirUI waterReservoirUI;

    [SerializeField] private TMP_Text pressToRefillText;
    void Start()
    {
        pressToRefillText.gameObject.SetActive(false);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnEnterInteractWithWaterZone, OnEnterWaterZone);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnExitInteractWithWaterZone, OnExitWaterZone);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnGainWater, OnGainWater);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnLoseWater, OnLoseWater);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnWaterSourceEmpty, OnWaterSourceEmpty);
    }

    private void OnDestroy()
    {
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnEnterInteractWithWaterZone, OnEnterWaterZone);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnExitInteractWithWaterZone, OnExitWaterZone);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnGainWater, OnGainWater);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnLoseWater, OnLoseWater);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnWaterSourceEmpty, OnWaterSourceEmpty);
    }

    private void OnWaterSourceEmpty()
    {
        if(player.CurrentWaterSource != null && 
           player.CurrentWaterSource.currentAmount <= 0 && 
           pressToRefillText.gameObject.activeSelf)
            pressToRefillText.gameObject.SetActive(false);
    }

    void OnEnterWaterZone()
    {
        if(!player.WaterReservoir.IsFull() && 
           player.CurrentWaterSource != null && 
           player.CurrentWaterSource.currentAmount > 0)
            pressToRefillText.gameObject.SetActive(true);
    }
    
    void OnGainWater()
    {
        RefreshWaterZoneText();
    }
    
    void OnLoseWater()
    {
        RefreshWaterZoneText();
    }

    void RefreshWaterZoneText()
    {
        if (player.WaterReservoir.IsFull())
        {
            if(pressToRefillText.gameObject.activeSelf)
                pressToRefillText.gameObject.SetActive(false);
        }
        else
        {
            if(player.InWaterZone && !pressToRefillText.gameObject.activeSelf)
                pressToRefillText.gameObject.SetActive(true);
        }
    }
    
    void OnExitWaterZone()
    {
        if(!player.InWaterZone)
            pressToRefillText.gameObject.SetActive(false);
    }
}
