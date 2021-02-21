﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : ColorFightersBase
{
    private Stack<Shot> magazine;
    public Shot shotPrefab;

    public bool isPoweredUp;
    private Player shooter;
    private int shotCounter = 0;

    void Start()
    {
        shooter = GetComponentInParent<Player>();
        game = shooter.game;
        magazine = new Stack<Shot>(4);

    }

    public void Fire()
    {
        Shot new_shot;
        if (magazine.Count == 0)
        { //out of bullets, need to create a new one.
            new_shot = Instantiate(shotPrefab);

            new_shot.Init(shooter,
                GetComponent<ShotController>(),
                game, string.Format("{0}-Shot{1}",
                shooter.name.Replace("layer", ""), shotCounter++),
                shooter.entityColor);
        }
        else
        {
            new_shot = magazine.Pop();
        }

        new_shot.transform.position = gameObject.transform.position;

        Vector3 firing_dir = shooter.LastFacingDirVector();

        firing_dir.y += game.config.ShotElevation;
        Debug.Log("Firing direction: " + firing_dir);
        new_shot.gameObject.SetActive(true);
        new_shot.Fire(firing_dir, isPoweredUp);
        isPoweredUp = false;
    }
    public void ReturnShot(Shot shot)
    {
        shot.gameObject.SetActive(false);
        shot.rigidBody.velocity = Vector3.zero;
        magazine.Push(shot);


    }
}
