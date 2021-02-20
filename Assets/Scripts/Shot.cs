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
    private float BaseBulletSize;
    

    public override void Awake()
    {
        base.Awake();
        particleSystem = GetComponent<ParticleSystem>();
        gunParticles = particleSystem.main;
        rigidBody = GetComponent<Rigidbody>();
        BaseBulletSize = transform.localScale.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collider = other.gameObject;
        
        if (other.CompareTag("Player") && other.GetComponent<Player>() == Owner.GetComponent<Player>()) {
            Debug.Log("bullet hit owner");
        }
        else {
            Debug.Log("Bullet hit: " + other.name);
            shotController.ReturnShot(GetComponent<Shot>());
        }
        
    }

    /// <summary>
    /// Initialise parameters for the Shot entity
    /// </summary>
    /// <param name="shooter"></param>
    /// <param name="shotController"></param>
    /// <param name="game"></param>
    /// <param name="name"></param>
    /// <param name="color"></param>
    public void Init(Player shooter, ShotController gun,  GameController gameController, string str,Color col) {
        Owner = shooter;
        name = str;
        game = gameController;
        entityColor = col;
        shotController = gun;
    }
    public void DoPowerUp() {
        isPoweredUp = true;
        transform.localScale=Vector3.one*BaseBulletSize*game.config.PowerupSizeMultiplier;
    }

    public void PowerDown() {
        isPoweredUp = false;
        transform.localScale=Vector3.one*BaseBulletSize;
    }

}
