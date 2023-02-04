using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaterReceiver : WaterReceiver
{
    protected override void OnParticleCollisionEnter(Component colComponent, ParticleSystem.Particle particle, Vector3 position, Vector3 direction)
    {
        if (colliders.Contains((Collider)colComponent))
        {
            var ragdoll = colComponent.GetComponentInParent<Ragdoll>();
            ragdoll.Fling(position, direction, 5f);
            OnWaterReceived?.Invoke();
        }
    }
}
