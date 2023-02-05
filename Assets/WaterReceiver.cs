using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WaterReceiver : MonoBehaviour
{
    public List<Collider> colliders;
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

    protected virtual void OnParticleCollisionEnter(Component colComponent, ParticleSystem.Particle particle, Vector3 position, Vector3 direction)
    {
        if (colliders.Contains(colComponent))
        {
            OnWaterReceived?.Invoke();
        }
    }


    protected void OnDestroy()
    {
        if (WaterSystem.Instance)
        {
            foreach (var col in colliders)
            {
                WaterSystem.Instance.UnregisterCollider(col);
            }
        }
        if (GlobalEvents.Instance)
            GlobalEvents.Instance.OnParticleCollisionEnter -= OnParticleCollisionEnter;
    }
}
