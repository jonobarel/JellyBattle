using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : ColorFightersBase
{
    private Stack<Shot> magazine;
    private float shotCooldown;
    public Shot shotPrefab;
    private float shotSpeed;
    public bool isPoweredUp;
    private Player shooter;
    private int shotCounter = 0;
    
    void Start()
    {
        shooter = GetComponentInParent<Player>();
        game = shooter.game;
        shotCooldown = game.config.ShotCooldown;
        shotSpeed = game.config.BulletSpeed;
        magazine = new Stack<Shot>(4);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire() {
        Shot new_shot;
        if (magazine.Count == 0) {
            new_shot = Instantiate(shotPrefab);
            new_shot.Owner = shooter;
            new_shot.name = string.Format("P{0}-Shot{1}", shooter.name.Replace("layer",""), shotCounter++);
            new_shot.game = game;
            new_shot.entityColor = shooter.entityColor;
        }
        else {
            new_shot = magazine.Pop();
        }

        new_shot.transform.position = gameObject.transform.position;

        Vector3 firing_dir = shooter.LastFacingDirVector();
        
        new_shot.gameObject.SetActive(true);
        new_shot.GetComponent<Rigidbody>().AddForce(firing_dir*shotSpeed, ForceMode.VelocityChange);
    }
}
