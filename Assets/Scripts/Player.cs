using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public const int FaceLeft = -1;
    public const int FaceRight = 1;
    public const float DefenseDownForce = 10f;

    [Header("Gameplay Variables")]
    
    
    
    
    [SerializeField] private GameObject defenseShield;
    

    private float shotCooldown;
    private bool toJump = false;

    //member objects
    private Rigidbody rigidBody;
    private static readonly int SieldInactiveLayer = 9;
    private static readonly int DefaultLayer = 0;

    //private Light playerLight;
    [SerializeField] private ShotController particleGun;
    [SerializeField] private ParticleSystem.MainModule gunParticles;
    [SerializeField] private DebugText debugText;

    private Animator animator;
    private PlayerInput input;

    private bool isGrounded = false;

    private float movementX = 0;
    private float lastDirectionFacing = 1;
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

    public Color playerColor
    {
        get { return entityColor; }
        set { entityColor = value; }

    }

    private float nextShotWindow = 0f;
    private bool isPoweredUp = false;

    public override void Awake()
    {
        base.Awake();
        particleGun = GetComponentInChildren<ShotController>();
        //gunParticles = particleGun.GetComponent<ParticleSystem>().main;
        animator = GetComponent<Animator>();
        entity = GetComponent<Player>();
        input = GetComponent<PlayerInput>();

    }
    void Start()
    {
        InitConfigVars(game.config);
        rigidBody = GetComponent<Rigidbody>();

        //Debug.Log(name + " initiated.");
    }

    public Vector3 LastFacingDirVector()
    {
        return new Vector3(lastDirectionFacing, 0, 0);
    }
    public void FixedUpdate()
    {
        debugText.force = movementX;
        debugText.velocity = rigidBody.velocity.x;

        rigidBody.AddForce(new Vector3(movementX * game.config.PlayerAcceleration * Time.fixedDeltaTime, 0f, 0f), ForceMode.Impulse);


        Vector3 new_vel = rigidBody.velocity;
        new_vel.x = Mathf.Clamp(new_vel.x, -game.config.PlayerMaxSpeed, game.config.PlayerMaxSpeed);

        rigidBody.velocity = new_vel;

        SetFacing(lastDirectionFacing); //adjust the direction of the particle cannon based on last movement
        if (toJump) {
            rigidBody.AddForce(Vector3.up * game.config.PlayerJumpForce / game.config.GravityMultiplier, ForceMode.VelocityChange);
            toJump = false;
        }
    }

    private void InitConfigVars(GameConfig config)
    {
        if (config.DebugMode)
        {
            debugText.gameObject.SetActive(true);
            if (name == "Player1")
            {
                debugText.textPosition = TextAnchor.MiddleLeft;
            }
            else
            {
                debugText.textPosition = TextAnchor.MiddleRight;
            }
        }

    }
    public void OnJump(InputValue jumpValue)
    {
        if (isGrounded)
        {
            toJump = true;
            isGrounded = false;

        }
    }

    public void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        //Debug.Log(name + ": movement vector " + movementVector);
        if (isDefending)
        {
            movementVector = Vector2.zero;
        }

        movementX = movementVector.x;

        // record the last non-zero horizontal movement for the particle cannon.
        if (movementVector.x != 0)
        {
            lastDirectionFacing = movementVector.x;

        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Void"))
        {
            Die();
        }
        else if (other.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;

            //maintain  horizontal velocity when hitting a platform to keep movement smooth
            rigidBody.velocity = new Vector3(-other.relativeVelocity.x, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            game.PowerUpTaken();
            particleGun.isPoweredUp = true;
        }
        else if (
                other.gameObject.CompareTag("Bullet")
                && other.GetComponent<Shot>().Owner != entity
                && (!isDefending || other.GetComponent<Shot>().isPoweredUp))
        {
            Die();
        }

        
    }

    /// <summary>
    /// Sets the firing direction for the particle cannon.
    /// Used in game startup and during FixedUpdate
    /// </summary>
    /// <param name="facingDirection">use PlayerController.FaceLeft or PlayerController.FaceRight</param>
    public void SetFacing(float facingDirection)
    {
        Vector3 new_facing = new Vector3(facingDirection, 0f, 0f).normalized;
        particleGun.transform.rotation = Quaternion.LookRotation(new_facing, Vector3.up);

    }
    public void OnShoot()
    {
        if (!isDefending && Time.time > nextShotWindow)
        {
            //this COULD be moved to the particle cannon, I suppose?
            particleGun.Fire();
            nextShotWindow = Time.time + game.config.ShotCooldown;
        }

    }

    public void OnDefend(InputValue input)
    {
        if (!input.isPressed) //isDefending is redundant, because of how keys work. But just in case. 
        { //stop defending
            defenseShield.layer = SieldInactiveLayer;
            rigidBody.constraints ^= RigidbodyConstraints.FreezePositionX; //unlock horizontal position.
            
            isDefending = false;
        }
        else if (input.isPressed)
        {
            //enter defense mode
            defenseShield.layer = DefaultLayer;
            isDefending = true;
            rigidBody.constraints |= RigidbodyConstraints.FreezePositionX; //lock horizontal position
            rigidBody.AddForce(Vector3.down*DefenseDownForce, ForceMode.Impulse);
        }
        animator.SetBool("Defending", isDefending);
    }
    private void PlayerDefend()
    {
        //TODO: implement me
        //transform.GetChild(3).gameObject.SetActive(true);
    }

    private void PlayerStopDefending()
    {
        //TODO: implement me.
        //transform.GetChild(3).gameObject.SetActive(false);
    }

    private void Die()
    {

        isDead = true;
        entityColor = new Color(0.2f, 0.2f, 0.2f, 0.6f);
        bodyLight.enabled = false;

        input.enabled = false;
        rigidBody.constraints|= RigidbodyConstraints.FreezePositionX;

        animator.SetBool("Dead", isDead);
        game.Victory(name);
    }

}