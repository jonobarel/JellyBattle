using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// Controller class for the Arena. This can be replicated and attached for alternative Arena layouts.
    /// Handles spawning of Powerups.
    /// </summary>
public class ArenaController : ColorFightersBase
{


    /// <summary>
    /// Prefab object for the Powerup to be spawned.
    /// </summary>    
    public PowerUp powerUpPrefab;

    private float powerUpSpawnInterval;
    private float lastPickupTime = 0f;
    private Vector3[] spawnPoints;
    private PowerUp pup;
    private bool isPowerUpAvailable;
    private float SpawnPointVerticalOffset = 1f; //how far above the platforms the powerup should be spawned

    void Start()
    {
        powerUpSpawnInterval = game.config.PowerupSpawnInterval;
        spawnPoints = GetSpawnPoints();
        pup = Instantiate(powerUpPrefab, Vector3.zero, Quaternion.identity); //prewarm
        
        pup.gameObject.SetActive(false);
    }

    /// <summary>
    /// update is currently used only to decide whether to spawn a powerup.
    /// Could have been replaced with a timer, that triggers at a random time within a window around the interval.
    /// </summary>
    void FixedUpdate()
    {
        
        if(!isPowerUpAvailable) { 
            float t = Time.time - lastPickupTime;
            if (t > powerUpSpawnInterval) {
                SpawnPowerup();
            }
        }
    }

    /// <summary>
    /// When the powerup is taken, the graphic should be hidden and time starts counting.
    /// </summary>
    public void PowerUpTaken() {
        isPowerUpAvailable = false;
        pup.gameObject.SetActive(false);
        lastPickupTime = Time.time;
    }
    
    private void SpawnPowerup()
    {
        int rand = Random.Range(0, spawnPoints.Length);
        pup.transform.position = spawnPoints[rand];
        isPowerUpAvailable = true;
        pup.gameObject.SetActive(true);
    }

    /// <summary>
    /// Generates an array of positions that can be chosen by the arena to spawn a powerup. 
    /// This is done by iterating over all the platforms that are marked as canSpawnPowerUp = true in their inspector.
    /// 
    /// Allows the customization of arenas, and choosing spawn points for powerups without adding additional components.
    /// </summary>
    /// <returns>an array of Vector3 representing a position <i>slightly</i> above the platform.</returns>
    private Vector3[] GetSpawnPoints() {
        ArenaPlatform[] platforms = GetComponentsInChildren<ArenaPlatform>();
        List<Vector3> posList = new List<Vector3>(platforms.Length);
        Vector3 offset = new Vector3(0,SpawnPointVerticalOffset, 0);

        foreach (ArenaPlatform p in platforms)
        {
            if (p.canSpawnPowerup) {
                posList.Add(p.transform.position+offset);
            }
        }

        return posList.ToArray();
        
    }
}
