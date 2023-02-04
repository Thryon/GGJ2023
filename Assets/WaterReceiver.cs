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

    private void OnParticleCollisionEnter(Component colComponent, ParticleSystem.Particle particle)
    {
        if (colliders.Contains(colComponent))
        {
            Collider col = (Collider)colComponent;
            Vector3 colliderPoint = col.ClosestPoint(particle.position);
            Vector3 dir = (colliderPoint - particle.position).normalized;
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
