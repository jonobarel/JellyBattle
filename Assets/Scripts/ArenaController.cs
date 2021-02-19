using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : ColorFightersBase
{
    public GameObject powerUpSpawnPoints;
    public PowerUp powerUpPrefab;


    private float powerUpSpawnInterval;
    private float lastPickupTime = 0f;
    private Vector3[] spawnPoints;
    private PowerUp pup;
    private bool isPowerUpAvailable;

    private float SpawnPointVerticalOffset = 1f;


    void Start()
    {
        powerUpSpawnInterval = game.config.PowerupSpawnInterval;
        spawnPoints = GetSpawnPoints();
        pup = Instantiate(powerUpPrefab, Vector3.zero, Quaternion.identity); //prewarm
        
        pup.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("time since powerup spawn: " + Mathf.Round(Time.time - lastPickupTime));
        if(!isPowerUpAvailable) {
            float t = Time.time - lastPickupTime;
            if (t > powerUpSpawnInterval) {
                SpawnPowerup();
            }
        }

    }

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
