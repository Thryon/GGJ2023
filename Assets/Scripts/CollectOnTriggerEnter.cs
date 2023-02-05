using UnityEngine;

public class CollectOnTriggerEnter : MonoBehaviour
{
    public LootType lootType;
    public int amount = 1;
    bool isMagnetized = false;
    Transform playerTarget = null;

    private void OnTriggerEnter(Collider other)
    {
        if (isMagnetized)
            return;

        if (other.CompareTag("Player"))
        {
            isMagnetized = true;
            playerTarget = other.transform;
        }
    }

    float lerpParam = 0.0f;
    float lerpSpeed = 3.0f;

    private void Update()
    {
        if (!isMagnetized)
            return;

        lerpParam += Time.deltaTime * lerpSpeed;
        transform.position = Vector3.Lerp(transform.position, playerTarget.position, lerpParam);
        if (lerpParam >= 1.0f)
        {
            Collect();
        }
    }

    public void Collect()
    {
        switch (lootType)
        {
            case LootType.Seeds:
                ReferencesSingleton.Instance.player.Inventory.AddSeeds(amount);
                break;
            case LootType.Water:
                ReferencesSingleton.Instance.player.WaterReservoir.RefillWater(amount);
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }
}
