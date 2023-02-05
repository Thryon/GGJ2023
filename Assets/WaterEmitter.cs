using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEmitter : MonoBehaviour
{
    public ParticleSystem ParticleSystem;

    private void Start()
    {
        WaterSystem.Instance.AddEmitter(this);
    }

    private List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>(100);
    private void OnParticleTrigger()
    {
        int particlesCount = ParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles,
            out ParticleSystem.ColliderData colliderData);

        for (int i = 0; i < particlesCount; i++)
        {

            int colliderCount = colliderData.GetColliderCount(i);
            for (int j = 0; j < colliderCount; j++)
            {
                Component comp = colliderData.GetCollider(i, j);
                var particle = particles[i];
                particle.remainingLifetime = 0f;
                particles[i] = particle;
                Collider col = (Collider)comp;
                Vector3 colliderPoint = col.ClosestPoint(particle.position);
                Vector3 dir = (colliderPoint - particle.position).normalized;
                ParticleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles);
                GlobalEvents.Instance.DispatchOnParticleCollisionEvent(comp, particles[i], particle.position, dir);
            }
        }
    }

    private void OnDestroy()
    {
        if (WaterSystem.Instance != null)
            WaterSystem.Instance.RemoveEmitter(this);
    }
}
