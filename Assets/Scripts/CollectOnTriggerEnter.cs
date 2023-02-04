using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectOnTriggerEnter : MonoBehaviour
{
    public LootType lootType;
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Collect();
    }

    public void Collect()
    {
        // Todo: give smthg
        switch (lootType)
        {
            case LootType.Seeds:
                ReferencesSingleton.Instance.player.Inventory.AddSeeds(amount);
                break;
            case LootType.Water:
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }
}
