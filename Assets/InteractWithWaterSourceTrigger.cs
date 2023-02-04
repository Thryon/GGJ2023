using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KinematicCharacterController;
using Unity.VisualScripting;
using UnityEngine;

public class InteractWithWaterSourceTrigger : MonoBehaviour
{
    [SerializeField] private WaterSource WaterSource;
    
    private List<Player> playersInTrigger = new List<Player>();
    private List<Collider> collidersInTrigger = new List<Collider>();
    
    private void OnTriggerEnter(Collider other)
    {
        collidersInTrigger.Add(other);
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponentInParent<Player>();
            playersInTrigger.Add(player);
            player.EnterWaterSource(WaterSource);
            GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnEnterInteractWithWaterZone);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collidersInTrigger.Remove(other);
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponentInParent<Player>();
            player.LeaveWaterSource(WaterSource);
            playersInTrigger.Remove(player);
            GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnExitInteractWithWaterZone);
        }
    }
}
