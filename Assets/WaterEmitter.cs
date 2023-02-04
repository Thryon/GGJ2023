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
                ParticleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles);
                GlobalEvents.Instance.DispatchOnParticleCollisionEvent(comp, particles[i]);
            }
        }
    }

    private void OnDestroy()
    {
        WaterSystem.Instance.RemoveEmitter(this);
    }
}
