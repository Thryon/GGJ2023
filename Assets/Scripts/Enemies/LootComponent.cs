using System.Collections.Generic;
using UnityEngine;

public class LootComponent : MonoBehaviour
{
    [System.Serializable]
    public struct LootPrefabQuantity
    {
        public GameObject prefab;
        public int quantity;
    }

    public List<LootPrefabQuantity> loots = new List<LootPrefabQuantity>();

    public float forceOnSpawn = 100.0f;

    public void SpawnLoot()
    {
        foreach (var loot in loots)
        {
            if (loot.prefab == null || loot.quantity == 0)
                continue;

            for (int i = 0; i < loot.quantity; i++)
            {
                GameObject spawned = Instantiate(loot.prefab, transform.position + Vector3.up, Quaternion.identity);
                Rigidbody rb = spawned.GetComponent<Rigidbody>();
                if (rb == null)
                    rb = spawned.AddComponent<Rigidbody>();

                Vector3 offsetRandom = Random.insideUnitSphere;
                rb.AddForce((2*Vector3.up + new Vector3(offsetRandom.x, 0, offsetRandom.z)).normalized * forceOnSpawn);
                rb.useGravity = true;
            }
        }
    }
}
