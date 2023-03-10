using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private WaterReservoir waterReservoir;
    [SerializeField] private float fireRate = 100f;
    [SerializeField] private LayerMask layerMask;

    private bool shooting = false;
    
    private void Start()
    {
        particles.Stop();
    }

    public void SetFireRate(float fireRate)
    {
        this.fireRate = fireRate;
    }
    
    private float timer = 0f;

    private float Interval => 1f / fireRate;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartShootingWater();
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            StopShootingWater();
        }

        if (shooting)
        {
            timer += Time.deltaTime;
            while (timer >= Interval)
            {
                if (waterReservoir.UseWater(1) > 0)
                {
                    StopShootingWater();
                    break;
                }
                else
                {
                    GlobalEvents.Instance.SendEvent(GlobalEventEnum.SmallCameraShake, 0.06f);
                    particles.Emit(1);
                }
                timer -= Interval;
            }
        }
    }

    private ParticleSystem.Particle[] particlesArray = new ParticleSystem.Particle[300];
    private void FixedUpdate()
    {
        int particlesAmount = particles.GetParticles(particlesArray);
        for (int i = 0; i < particlesAmount; i++)
        {
            var particle = particlesArray[i];
            var colliders = Physics.OverlapSphere(particlesArray[i].position, particlesArray[i].GetCurrentSize(particles), layerMask);

            for (int j = 0; j < colliders.Length; j++)
            {
                if(!WaterSystem.Instance.IsColliderRegistered(colliders[j]))
                    continue;
                
                if (particle.remainingLifetime == 0f)
                    break;
                particlesArray[i].remainingLifetime = 0f;
                Collider col = colliders[j];
                Vector3 colliderPoint = col.ClosestPoint(particle.position);
                Vector3 dir = (colliderPoint - particle.position).normalized;
                GlobalEvents.Instance.DispatchOnParticleCollisionEvent(col, particle, particle.position, dir);
            }
        }
        particles.SetParticles(particlesArray, particlesAmount);
    }

    void StartShootingWater()
    {
        shooting = true;
        timer = 0f;
        particles.Play();
    }

    void StopShootingWater()
    {
        shooting = false;
        particles.Stop();
    }
}
