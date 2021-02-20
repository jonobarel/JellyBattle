using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : ColorFightersBase
{
    private Stack<Shot> magazine;
    private float shotCooldown;
    public Shot shotPrefab;
    [SerializeField] private float shotSpeed;
    public bool isPoweredUp;
    private Player shooter;
    private int shotCounter = 0;
    [SerializeField] private float shotElevation = 0.5f;
    void Start()
    {
        shooter = GetComponentInParent<Player>();
        game = shooter.game;
        shotCooldown = game.config.ShotCooldown;
        shotSpeed = game.config.BulletSpeed;
        shotElevation = game.config.ShotElevation;
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
        
        firing_dir.y += shotElevation;
        new_shot.gameObject.SetActive(true);
        new_shot.Fire(firing_dir, isPoweredUp);
        isPoweredUp = false;
    }
    public void ReturnShot(Shot shot)
    {
        shot.rigidBody.velocity = Vector3.zero;
        shot.gameObject.SetActive(false);
        magazine.Push(shot);


    }
}
