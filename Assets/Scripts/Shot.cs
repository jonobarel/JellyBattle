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
            return;
        }
        else {
            shotController.ReturnShot(GetComponent<Shot>());
        }
    }

    public void Fire(Vector3 firing_dir, bool powered) {
        float size_factor = BaseBulletSize*(powered ? game.config.PowerupSizeMultiplier : 1f);
        float powereup_speed_factor = powered ?  game.config.PowerupShotSpeedMultiplier : 1f;

        particleSys.Play();

        isPoweredUp = powered;
        firing_dir.x*=powereup_speed_factor;
        transform.localScale = Vector3.one*size_factor;
       
        GetComponent<Rigidbody>().AddForce(firing_dir*game.config.BulletSpeed, ForceMode.VelocityChange);

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
