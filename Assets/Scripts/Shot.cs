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
    private Vector3 trajectory;
    private bool toFire = false;
   

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
            return;
        }
        else {
            shotController.ReturnShot(GetComponent<Shot>());
        }
    }

    public void Fire(Vector3 firing_dir, bool powered){
        isPoweredUp = powered;
        float size_factor = BaseBulletSize*(isPoweredUp ? game.config.PowerupSizeMultiplier : 1f);
        float powerup_speed_factor = isPoweredUp ?  game.config.PowerupShotSpeedMultiplier : 1f;
        trajectory = firing_dir;
        trajectory.x*=powerup_speed_factor;
        transform.localScale = Vector3.one*size_factor;
        toFire = true;
    }
    
    private void FixedUpdate() {
        if (toFire) {
            FireFixedUpdate();
            toFire = false;
        }
    }
    
    public void FireFixedUpdate() {
        particleSys.Play();
        //Debug.Log(trajectory + ", " + game.config.BulletSpeed);
        rigidBody.AddForce(trajectory*game.config.BulletSpeed, ForceMode.VelocityChange);
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


}
