using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : ColorFightersBase
{
    [SerializeField] private Color deathColor;
    public bool isPoweredUp;

    [SerializeField] private Player owner;
    new private ParticleSystem particleSystem;
    private ParticleSystem.MainModule gunParticles;

    public Player Owner
    {
        get { return owner; }
    }

    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        gunParticles = particleSystem.main;
    }
    public void Shoot()
    {
        particleSystem.Play();
        if (isPoweredUp)
        {
            Debug.Log(owner.name + "fired a powered shot!");
        }
    }

    public void SetParticleSpeed(float speed)
    {
        gunParticles.startSpeed = speed;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Player>() != owner)
        {
            isPoweredUp = false;
        }
    }
}
