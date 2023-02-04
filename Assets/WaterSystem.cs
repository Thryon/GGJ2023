using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSystem : Singleton<WaterSystem>
{
    private List<WaterEmitter> waterEmitters = new List<WaterEmitter>();

    private List<Collider> registeredColliders = new List<Collider>();
    
    public void AddEmitter(WaterEmitter emitter)
    {
        waterEmitters.Add(emitter);
        foreach (var collider in registeredColliders)
        {
            emitter.ParticleSystem.trigger.AddCollider(collider);
        }
    }

    public void RemoveEmitter(WaterEmitter emitter)
    {
        waterEmitters.Remove(emitter);
        foreach (var collider in registeredColliders)
        {
            emitter.ParticleSystem.trigger.RemoveCollider(collider);
        }
    }

    public void RegisterCollider(Collider collider)
    {
        if(registeredColliders.Contains(collider))
            return;
        
        registeredColliders.Add(collider);
        foreach (var emitter in waterEmitters)
        {
            emitter.ParticleSystem.trigger.AddCollider(collider);
        }
    }

    public void UnregisterCollider(Collider collider)
    {
        registeredColliders.Remove(collider);
        foreach (var emitter in waterEmitters)
        {
            emitter.ParticleSystem.trigger.RemoveCollider(collider);
        }
    }
}
