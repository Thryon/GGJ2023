using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTriggerZone : MonoBehaviour
{
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;

    private bool playerInZone = false;

    public bool PlayerInZone => playerInZone;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTriggerEnterEvent?.Invoke();
            playerInZone = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTriggerExitEvent?.Invoke();
            playerInZone = false;
        }
    }
}
