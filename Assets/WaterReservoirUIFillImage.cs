using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterReservoirUIFillImage : MonoBehaviour
{
    [SerializeField] private WaterReservoir waterReservoir;
    [SerializeField] private Image fillImage;
    [SerializeField] private PlayerTriggerZone playerTriggerZone;

    private void Start()
    {
        waterReservoir.OnFillRateChanged.RemoveListener(FillRateChanged);
        waterReservoir.OnFillRateChanged.AddListener(FillRateChanged);
    }

    private void OnDestroy()
    {
        waterReservoir.OnFillRateChanged.RemoveListener(FillRateChanged);
    }

    private Coroutine waitAndHideCoroutine;
    private void FillRateChanged(float fill)
    {
        if (!playerTriggerZone.PlayerInZone)
        {
            if (waitAndHideCoroutine != null)
                StopCoroutine(waitAndHideCoroutine);
            
            gameObject.SetActive(true);
            waitAndHideCoroutine = StartCoroutine(HideInSeconds(5f));
        }
        else
        {
            if (waitAndHideCoroutine != null)
            {
                StopCoroutine(waitAndHideCoroutine);
                waitAndHideCoroutine = null;
            }
        }
        
        RefreshFillValue(fill);
    }

    IEnumerator HideInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (!playerTriggerZone.PlayerInZone)
            gameObject.SetActive(false);
    }

    void OnEnable()
    {
        RefreshFillValue(waterReservoir.FillValue);
    }

    private void OnDisable()
    {
        if (waitAndHideCoroutine != null)
        {
            StopCoroutine(waitAndHideCoroutine);
            waitAndHideCoroutine = null;
        }
    }

    void RefreshFillValue(float value)
    {
        fillImage.fillAmount = value;
    }
}
