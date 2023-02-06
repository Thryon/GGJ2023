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

    private List<ParticleSystem.Particle> insideParticles = new List<ParticleSystem.Particle>(100);
    private List<ParticleSystem.Particle> enterParticles = new List<ParticleSystem.Particle>(100);
    private void OnParticleTrigger()
    {
        int insideParticlesCount = ParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, insideParticles,
            out ParticleSystem.ColliderData insideColliderData);

        int enterParticlesCount = ParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterParticles,
            out ParticleSystem.ColliderData enterColliderData);
        
        for (int i = 0; i < insideParticlesCount; i++)
        {
            int colliderCount = insideColliderData.GetColliderCount(i);
            for (int j = 0; j < colliderCount; j++)
            {
                Component comp = insideColliderData.GetCollider(i, j);
                var particle = insideParticles[i];
                particle.remainingLifetime = 0f;
                insideParticles[i] = particle;
                Collider col = (Collider)comp;
                Vector3 colliderPoint = col.ClosestPoint(particle.position);
                Vector3 dir = (colliderPoint - particle.position).normalized;
                ParticleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, insideParticles);
                GlobalEvents.Instance.DispatchOnParticleCollisionEvent(comp, insideParticles[i], particle.position, dir);
            }
        }
        
        for (int i = 0; i < enterParticlesCount; i++)
        {
            int colliderCount = enterColliderData.GetColliderCount(i);
            for (int j = 0; j < colliderCount; j++)
            {
                Component comp = enterColliderData.GetCollider(i, j);
                var particle = enterParticles[i];
                particle.remainingLifetime = 0f;
                enterParticles[i] = particle;
                Collider col = (Collider)comp;
                Vector3 colliderPoint = col.ClosestPoint(particle.position);
                Vector3 dir = (colliderPoint - particle.position).normalized;
                ParticleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterParticles);
                GlobalEvents.Instance.DispatchOnParticleCollisionEvent(comp, enterParticles[i], particle.position, dir);
            }
        }
    }

    private void OnDestroy()
    {
        if (WaterSystem.Instance != null)
            WaterSystem.Instance.RemoveEmitter(this);
    }
}
