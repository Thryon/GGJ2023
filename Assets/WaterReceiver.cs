using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WaterReceiver : MonoBehaviour
{
    public List<Collider> colliders;
    public bool KillParticles = true;
    public UnityEvent OnWaterReceived;
    
    protected virtual void Start()
    {
        if (colliders == null)
            colliders = GetComponents<Collider>().ToList();
        foreach (var col in colliders)
        {
            WaterSystem.Instance.RegisterCollider(col);
        }

        GlobalEvents.Instance.OnParticleCollisionEnter -= OnParticleCollisionEnter;
        GlobalEvents.Instance.OnParticleCollisionEnter += OnParticleCollisionEnter;
    }

    private void OnParticleCollisionEnter(Component col, ParticleSystem.Particle particle)
    {
        if (colliders.Contains(col))
        {
            if (KillParticles)
                particle.remainingLifetime = 0f;
            OnWaterReceived?.Invoke();
        }
    }


    private void OnDestroy()
    {
        foreach (var col in colliders)
        {
            WaterSystem.Instance.UnregisterCollider(col);
        }
        GlobalEvents.Instance.OnParticleCollisionEnter -= OnParticleCollisionEnter;
    }
}
