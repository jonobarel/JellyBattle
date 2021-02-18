using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ColorFighters/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Player")]
    public Color Player1DefaultColor;
    public Color Player2DefaultColor;
    public float PlayerMaxSpeed = 5;
    public float PlayerJumpForce = 5;
    public float PlayerAcceleration = 5;
    public float GravityMultiplier = 1;

    [Header("Bullets")]
    public float BulletSpeed = 20;
    public float ShotCooldown = 2;
    public Color deathColor;
    
    [Header("Powerup")]
    public float PowerupShotSpeedMultiplier = 3;
    public float PowerupSizeMultiplier = 2;

    [Header("World")]
    public float PowerupSpawnInterval = 10;

    [Header("Developer options")]
    public bool ShowDebugData;
}
