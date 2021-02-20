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
        Debug.Log(string.Format("{0} owned by {1} hit {2}", name,Owner, other.name ));
        if (other.CompareTag("Player") && other.GetComponent<Player>() == Owner) {
            return;
        }
        
        DoUnpower();
        shotController.ReturnShot(GetComponent<Shot>());
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
        transform.localScale*=game.config.PowerupSizeMultiplier;
    }

    public void DoUnpower() {
        isPoweredUp = false;
        transform.localScale/=game.config.PowerupSizeMultiplier;
    }
}
