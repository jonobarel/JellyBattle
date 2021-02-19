using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shot : Entity
{
    public bool isPoweredUp;

    new private ParticleSystem particleSystem;
    private ParticleSystem.MainModule gunParticles;
    

    public override void Awake()
    {
        Debug.Log(name+" initialised");
        base.Awake();
        particleSystem = GetComponent<ParticleSystem>();
        gunParticles = particleSystem.main;
    }

    private void OnCollisionEnter(GameObject other)
    {
        Debug.Log(name + " collided with " + other.name);
        if (other.CompareTag("Player") && other.GetComponent<Player>() != Owner)
        {
            isPoweredUp = false;
        }
    }
}
