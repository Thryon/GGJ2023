using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterReceptacle : MonoBehaviour
{
    public WaterReservoir reservoir;
    public Canvas canvas;
    public GameObject TriggerZone;
    
    public UnityEvent OnReceptacleFilled;

    private void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    public void AddWater(int amount)
    {
        if(reservoir.IsFull())
            return;
        
        reservoir.RefillWater(amount);
        if (reservoir.IsFull())
        {
            ReceptacleFilled();
        }
    }

    public void ReceptacleFilled()
    {
        TriggerZone.SetActive(false);
        canvas.gameObject.SetActive(false);
        
        OnReceptacleFilled?.Invoke();
    }

    public void OnPlayerTriggerEnter()
    {
        if (reservoir.IsFull())
            return;
        canvas.gameObject.SetActive(true);
    }
    
    public void OnPlayerTriggerExit()
    {
        canvas.gameObject.SetActive(false);
    }
}
