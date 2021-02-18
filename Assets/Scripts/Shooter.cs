using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : ColorFightersBase
{
    [SerializeField] private Color deathColor;
    
    [SerializeField] private Player owner;
    new private ParticleSystem particleSystem;
    private ParticleSystem.MainModule gunParticles;

    public Player Owner {
        get {return owner;}
    }

    void Awake() {
        particleSystem = GetComponent<ParticleSystem>();
        gunParticles = particleSystem.main;
    }
    public void Shoot() {
        particleSystem.Play();
    }

    public void SetParticleSpeed(float speed) {
       gunParticles.startSpeed = speed;
    }
}
