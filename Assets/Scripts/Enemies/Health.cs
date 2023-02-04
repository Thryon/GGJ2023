using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int MaxHealth = 1;
    public float delayDestroyOnDeath = 0.25f;
    public UnityEvent OnDeathEvent;
    public UnityEvent OnTakeDamageEvent;
    int currentHealth;

    LootComponent lootComp;

    bool pendingKill = false;

    private void Start()
    {
        MaxHealth = (int)(WavesSystem.GetHealthMultiplier() * MaxHealth);
        currentHealth = MaxHealth;
        lootComp = GetComponent<LootComponent>();
    }

    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        OnTakeDamageEvent?.Invoke();
        if (currentHealth <= 0)
            Kill();
    }

    public void Kill()
    {
        if (pendingKill)
            return;

        if (lootComp != null)
            lootComp.SpawnLoot();

        pendingKill = true;

        StartCoroutine(DelayDestroy());
    }

    IEnumerator DelayDestroy()
    {
        OnDeath();
        yield return new WaitForSeconds(delayDestroyOnDeath);
        Destroy(transform.parent.gameObject);
    }

    public void GainHealth(int _gain)
    {
        currentHealth += _gain;
        if (currentHealth > MaxHealth)
            currentHealth = MaxHealth;
    }

    void OnDeath()
    {
        OnDeathEvent?.Invoke();
        // TODO: play death anim
    }
}
