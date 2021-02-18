﻿using UnityEngine;
using UnityEngine.InputSystem;

public class Player : ColorFightersBase
{
    public const int FaceLeft = -1;
    public const int FaceRight = 1;

    [Header("Gameplay Variables")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private GameObject defenseShield;
    [SerializeField] private ParticleSystem powerup;

    private float shotCooldown;

    //member objects
    private Renderer playerRenderer;
    private Rigidbody rigidBody;

    private Light playerLight;
    [SerializeField] private Shooter particleGun;
    [SerializeField] private ParticleSystem.MainModule gunParticles;
    [SerializeField] private DebugText debugText;    

    private Animator animator;
    private PlayerInput input;

    private Player self;
    private bool isGrounded = false;

    private float movementX = 0;
    private bool isDefending = false;

    public bool IsDefending
    {
        get { return isDefending; }
    }

    private bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
    }

    private float nextShotWindow = 0f;
    

    public Color playerColor
    {
        get { return playerRenderer.material.color; }
        set
        {
            SetPlayerColor(value);
        }
    }

    private bool isPoweredUp = false;

    void Awake() {
        playerRenderer = GetComponentInChildren<Renderer>();
        playerLight = GetComponent<Light>();
        particleGun = GetComponentInChildren<Shooter>();
        gunParticles = particleGun.GetComponent<ParticleSystem>().main;
        animator = GetComponent<Animator>();
        self = GetComponent<Player>();
        input = GetComponent<PlayerInput>();

    }
    void Start() {
        InitConfigVars(game.config);
        rigidBody = GetComponent<Rigidbody>();
        
        Debug.Log(name +" initiated.");
    }

    public void FixedUpdate() {
        debugText.force = movementX;
        debugText.velocity = rigidBody.velocity.x;

        if (Time.time > nextShotWindow) {
            //Debug.Log(name + ": Shot ready");
        }
        SetFacing(movementX); //adjust the direction of the particle cannon based on last movement
        rigidBody.AddForce(new Vector3(movementX * moveSpeed, 0f, 0f));


        Vector3 new_vel = rigidBody.velocity;
        new_vel.x = Mathf.Clamp(new_vel.x, -maxSpeed, maxSpeed);        
        
        rigidBody.velocity = new_vel;
    }

    private void InitConfigVars(GameConfig config) {
        jumpForce = config.PlayerJumpForce;
        gravityMultiplier = config.GravityMultiplier;
        moveSpeed = config.PlayerMaxSpeed;
        shotCooldown = config.ShotCooldown;
        maxSpeed = config.PlayerMaxSpeed;
        particleGun.SetParticleSpeed(config.BulletSpeed);
        if (config.ShowDebugData) {
            debugText.gameObject.SetActive(true);
        }
        
        //particleGun.GetComponent<ParticleSystem>().main.startSpeed = config.BulletSpeed;
    }
    public void OnJump(InputValue jumpValue) {
        if (isGrounded) {
            rigidBody.AddForce(Vector3.up * jumpForce / gravityMultiplier, ForceMode.VelocityChange);
            isGrounded = false;

        }
    }

    public void OnMove(InputValue movementValue) {
        Vector2 movementVector = movementValue.Get<Vector2>();

        //Debug.Log(name + ": movement vector " + movementVector);
        if (isDefending) {
            movementVector = Vector2.zero;
        }
        if (movementVector.x == 0) {
            rigidBody.velocity = Vector3.zero;
        } else {
            movementX = movementVector.x;
            //rigidBody.velocity = new Vector3(movementVector.x * moveSpeed, rigidBody.velocity.y, 0);
        }

        // record the last non-zero horizontal movement for the particle cannon.

        if (movementVector.x != 0) {
            movementX = movementVector.x;

        }
    }

    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Void")) {
            Die();
        }
        if (other.gameObject.CompareTag("Platform")) {
            isGrounded = true;

            //maintain  horizontal velocity when hitting a platform to keep movement smooth
            rigidBody.velocity = new Vector3(-other.relativeVelocity.x, 0, 0);
        }

        if (other.gameObject.CompareTag("Powerup")) {

        }

    }

    public void SetPlayerColor(Color color) {
        playerRenderer.material.SetColor("_Color", color);
        playerRenderer.material.SetColor("_EmissionColor", color);
        playerLight.color = color;
        gunParticles.startColor = color;
    }

    private void OnParticleCollision(GameObject other) {
        Shooter shooter = other.GetComponent<Shooter>();

        if (shooter.Owner == self) {
            //Debug.Log(name + "shot self");
        } else if (!isDefending) {
            Debug.Log(shooter.Owner.name + "==>" + name);
            game.PlayerHit(self, shooter);
            Die();
        } else {
            Debug.Log(name + " blocked!");
        }

    }

    /// <summary>
    /// Sets the firing direction for the particle cannon.
    /// Used in game startup and during FixedUpdate
    /// </summary>
    /// <param name="facingDirection">use PlayerController.FaceLeft or PlayerController.FaceRight</param>
    public void SetFacing(float facingDirection) {
        Vector3 new_facing = new Vector3(facingDirection, 0f, 0f);
        particleGun.transform.rotation = Quaternion.LookRotation(new_facing, Vector3.up);

    }
    public void OnShoot() {
        if (!isDefending && Time.time > nextShotWindow) {
            //this COULD be moved to the particle cannon, I suppose?
            particleGun.Shoot();
            nextShotWindow = Time.time + shotCooldown;
        }

    }

    public void OnDefend(InputValue input) {
        if (!input.isPressed && isDefending) //isDefending is redundant, because of how keys work. But just in case. 
        { //stop defending
            Debug.Log(name + ": Stop defending");
            rigidBody.constraints = RigidbodyConstraints.None;
            isDefending = false;
        } else if (input.isPressed) {
            //enter defense mode
            isDefending = true;
            Debug.Log(name + ": Defending");
            rigidBody.constraints = RigidbodyConstraints.FreezePositionX;
        }
        animator.SetBool("Defending", isDefending);
    }
    private void PlayerDefend() {
        //TODO: implement me
        //transform.GetChild(3).gameObject.SetActive(true);
    }

    private void PlayerStopDefending() {
        //TODO: implement me.
        //transform.GetChild(3).gameObject.SetActive(false);
    }

    private void Die() {

        isDead = true;

        playerColor = new Color(0.2f, 0.2f, 0.2f, 0.6f);
        playerLight.enabled = false;

        input.enabled = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;

        animator.SetBool("Dead", isDead);

    }

}