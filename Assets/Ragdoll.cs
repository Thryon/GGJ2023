using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private List<Component> componentsToDisable = new List<Component>();
    [SerializeField] private List<Collider> ragdollColliders = new List<Collider>();
    [SerializeField] private List<Rigidbody> ragdollRigidBodies = new List<Rigidbody>();

    private void Start()
    {
        Disable();
    }

    public void Enable()
    {

        ToggleComponentsToDisable(false);
        ToggleRagdollComponents(true);
    }

    void ToggleRagdollComponents(bool enable)
    {
        foreach (var ragdollComponent in ragdollColliders)
        {
            ragdollComponent.enabled = enable;
        }
        
        foreach (var ragdollComponent in ragdollRigidBodies)
        {
            ragdollComponent.isKinematic = !enable;
        }
    }

    void ToggleComponentsToDisable(bool enable)
    {
        foreach (var component in componentsToDisable)
        {
            if (component is MonoBehaviour)
            {
                ((MonoBehaviour)component).enabled = enable;
            }
            else if (component is Collider)
            {
                ((Collider)component).enabled = enable;
            }
            else if (component is Behaviour)
            {
                ((Behaviour)component).enabled = enable;
            }
            
        }
    }

    public void Disable()
    {
        ToggleComponentsToDisable(true);
        ToggleRagdollComponents(false);
    }

    public void Fling(Vector3 position, Vector3 direction, float force)
    {
        foreach (var ragdollComponent in ragdollRigidBodies)
        {
            if(ragdollComponent.isKinematic)
                continue;
            // ragdollComponent.isKinematic = false;
            ragdollComponent.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
    }
}
