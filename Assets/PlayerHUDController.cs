using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private TMP_Text lootAppeared;

    public GameObject losePanel;
    public TMP_Text currentWave;

    IEnumerator Start()
    {
        yield return null;
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
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.ShowHideLootAppeared, ShowHideLootAppeared);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.UpdateLootText, UpdateLootText);

        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnGemHit, PlayGemHitAnim);
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnGemDeath, ShowLoseScreen);
        
        GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.UpdateLosePanel, UpdateLosePanel);

        player = ReferencesSingleton.Instance.player;
    }

    private void OnDestroy()
    {
        if (GlobalEvents.Instance == null)
            return;

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

        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.ShowHideLootAppeared, ShowHideLootAppeared);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.UpdateLootText, UpdateLootText);

        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnGemHit, PlayGemHitAnim);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.OnGemDeath, ShowLoseScreen);
        GlobalEvents.Instance.UnregisterEvent(GlobalEventEnum.UpdateLosePanel, UpdateLosePanel);

    }

    Animator treelifeAnimator;
    void PlayGemHitAnim(int amount)
    {
        if (treelifeAnimator == null)
            treelifeAnimator = treeLife.GetComponentInParent<Animator>();

        treelifeAnimator.SetTrigger("hit");
    }

    void ShowLoseScreen()
    {
        losePanel.SetActive(true);

        // stop player

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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

    private void ShowHideLootAppeared(bool _show)
    {
        if (lootAppeared == null)
            return;

        lootAppeared.gameObject.SetActive(_show);

        if (_show)
        {
            if (lootAppearedCoroutine != null)
                StopCoroutine(lootAppearedCoroutine);

            lootAppearedCoroutine = StartCoroutine(ShowLootText());
        }
    }

    Coroutine lootAppearedCoroutine = null;
    IEnumerator ShowLootText()
    {
        yield return new WaitForSeconds(3.0f);
        ShowHideLootAppeared(false);
        lootAppearedCoroutine = null;
    }

    void UpdateLootText(string _text)
    {
        if (lootAppeared == null)
            return;

        lootAppeared.SetText(_text);
    }

    void UpdateLosePanel(int _currentWave)
    {
        currentWave.SetText("Current wave: " + _currentWave);
    }

    public void ReloadMap()
    {
        SceneManager.LoadScene(0);
    }
}
