using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shot : Entity
{
    public bool isPoweredUp;
    public ShotController shotController;
    new private ParticleSystem particleSystem;
    private ParticleSystem.MainModule gunParticles;
    public Rigidbody rigidBody;
    
    

    public override void Awake()
    {
        base.Awake();
        particleSystem = GetComponent<ParticleSystem>();
        gunParticles = particleSystem.main;
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collider = other.gameObject;
        Debug.Log(name + " collided with " + collider.name);
        if (collider.CompareTag("Player") && collider.GetComponent<Player>() != Owner)
        {
            isPoweredUp = false;
        } else if (collider.CompareTag("Void") || collider.CompareTag("Platform")) {
            shotController.ReturnShot(gameObject.GetComponent<Shot>());

        }
    }
}
