using System.Collections;
using UnityEngine;

public class Gem : MonoBehaviour
{
    Health health;

    private IEnumerator Start()
    {
        health = GetComponent<Health>();
        yield return null;
        yield return null;
        GlobalEvents.Instance.SendEvent(GlobalEventEnum.OnGemHit, health.MaxHealth);
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
