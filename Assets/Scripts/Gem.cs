using UnityEngine;

public class Gem : MonoBehaviour
{
    Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnGemHit, health.MaxHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            health.TakeDamage(1);
    }

    public void OnDeath()
    {
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnGemDeath);

        // Lose screen -> stop waves and player, use event

    }

    public void OnHit()
    {
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnGemHit, health.CurrentHealth);
    }
}