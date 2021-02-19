using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : ColorFightersBase
{
    public GameObject powerUpSpawnPoints;
    public PowerUp powerUpPrefab;


    private float powerUpSpawnInterval;
    private float lastPickupTime = 0f;
    private Transform[] spawnPoints;
    private PowerUp pup;
    private bool isPowerUpActive;

    void Start()
    {
        powerUpSpawnInterval = game.config.PowerupSpawnInterval;
        spawnPoints = GetComponentsInChildren<Transform>();
        pup = Instantiate(powerUpPrefab, Vector3.zero, Quaternion.identity); //prewarm
        pup.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("time since powerup spawn: " + Mathf.Round(Time.time - lastPickupTime));
        if(!isPowerUpActive) {
            float t = Time.time - lastPickupTime;
            if (t > powerUpSpawnInterval) {
                SpawnPowerup();
            }
        }

    }

    public void PowerUpTaken() {
        isPowerUpActive = false;
        pup.gameObject.SetActive(false);
        lastPickupTime = Time.time;
    }
    private void SpawnPowerup()
    {
        int rand = Random.Range(0, spawnPoints.Length);
        pup.transform.position = spawnPoints[rand].position;
        isPowerUpActive = true;
        pup.gameObject.SetActive(true);
    }

}
