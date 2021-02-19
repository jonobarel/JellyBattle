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
        base.Awake();
        particleSystem = GetComponent<ParticleSystem>();
        gunParticles = particleSystem.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collider = other.gameObject;
        Debug.Log(name + " collided with " + collider.name);
        if (collider.CompareTag("Player") && collider.GetComponent<Player>() != Owner)
        {
            isPoweredUp = false;
        }
    }
}
