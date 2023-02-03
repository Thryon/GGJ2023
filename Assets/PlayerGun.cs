using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float emitInterval = 0.02f;

    private bool shooting = false;
    
    private void Start()
    {
        particles.Stop();
    }

    private float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shooting = true;
            timer = 0f;
            particles.Play();
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            shooting = false;
            particles.Stop();
        }

        if (shooting)
        {
            timer += Time.deltaTime;
            while (timer >= emitInterval)
            {
                particles.Emit(1);
                timer -= emitInterval;
            }
        }
    }
}
