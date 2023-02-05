using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHUDController : MonoBehaviour
{
    [SerializeField] private Player player;
    [FormerlySerializedAs("waterReservoirUISlider")] [FormerlySerializedAs("waterReservoirUI")] [SerializeField] private PlayerWaterReservoirUISlider playerWaterReservoirUISlider;

    [SerializeField] private TMP_Text pressToRefillText;
    [SerializeField] private TMP_Text emptySourceText;
    [SerializeField] private TMP_Text treeLife;


    [SerializeField] private TMP_Text nextWaveInTxt;
    [SerializeField] private TMP_Text needRefillTxt;

    void Start()
    {
        pressToRefillText.gameObject.SetActive(false);
        emptySourceText.gameObject.SetActive(false);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnEnterInteractWithWaterZone, OnEnterWaterZone);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnExitInteractWithWaterZone, OnExitWaterZone);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnGainWater, OnGainWater);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnLoseWater, OnLoseWater);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnWaterSourceEmpty, OnWaterSourceEmpty);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnWaterSourceRefilled, OnWaterSourceRefilled);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnGemHit, UpdateTreeLife);

        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.ShowHideNextWaveTimer, ShowHideNextWaveTimer);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.UpdateNextWaveTimer, UpdateNextWaveTimer);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.ShowHideRefillText, ShowHideRefill);
    }

    private void OnDestroy()
    {
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnEnterInteractWithWaterZone, OnEnterWaterZone);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnExitInteractWithWaterZone, OnExitWaterZone);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnGainWater, OnGainWater);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnLoseWater, OnLoseWater);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnWaterSourceEmpty, OnWaterSourceEmpty);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnWaterSourceRefilled, OnWaterSourceRefilled);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnGemHit, UpdateTreeLife);

        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.ShowHideNextWaveTimer, ShowHideNextWaveTimer);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.UpdateNextWaveTimer, UpdateNextWaveTimer);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.ShowHideRefillText, ShowHideRefill);
    }

    private void OnWaterSourceEmpty()
    {
        if (player.CurrentWaterSource != null &&
           player.CurrentWaterSource.currentAmount <= 0)
        {
            pressToRefillText.gameObject.SetActive(false);
            emptySourceText.gameObject.SetActive(true);
        }
    }

    private void OnWaterSourceRefilled()
    {
        if (player.CurrentWaterSource != null)
        {
            pressToRefillText.gameObject.SetActive(true);
            emptySourceText.gameObject.SetActive(false);
        }
    }

    void OnEnterWaterZone()
    {
        if(!player.WaterReservoir.IsFull() && 
           player.CurrentWaterSource != null && 
           player.CurrentWaterSource.currentAmount > 0)
           pressToRefillText.gameObject.SetActive(true);
        if (!player.WaterReservoir.IsFull() &&
            player.CurrentWaterSource != null &&
            player.CurrentWaterSource.currentAmount <= 0)
            emptySourceText.gameObject.SetActive(true);
            
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
            emptySourceText.gameObject.SetActive(false);
    }

    void UpdateTreeLife(int _currentLife)
    {
        if (treeLife != null)
            treeLife.SetText(_currentLife.ToString());
    }

    private void ShowHideNextWaveTimer(bool _show)
    {
        if (nextWaveInTxt == null)
            return;

        nextWaveInTxt.gameObject.SetActive(_show);
    }

    private void UpdateNextWaveTimer(int _time)
    {
        if (nextWaveInTxt == null)
            return;

        nextWaveInTxt.SetText("Next wave in " + _time + "s");
    }

    private void ShowHideRefill(bool _show)
    {
        if (needRefillTxt == null)
            return;

        needRefillTxt.gameObject.SetActive(_show);

        if (_show)
        {
            if (showRefillCoroutine != null)
                StopCoroutine(showRefillCoroutine);

            showRefillCoroutine = StartCoroutine(ShowRefillText());
        }
    }

    Coroutine showRefillCoroutine = null;
    IEnumerator ShowRefillText()
    {
        yield return new WaitForSeconds(1.5f);
        ShowHideRefill(false);
        showRefillCoroutine = null;
    }
}
