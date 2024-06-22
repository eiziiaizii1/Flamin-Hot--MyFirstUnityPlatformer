using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerDeath;

    [SerializeField] private GameObject fireball;
    [SerializeField] private Transform fireBallSpawnPos;
    [SerializeField] private float fireballCooldown = 1f;
    private float currentTime = 0f;
    public bool isFired = false;

    [SerializeField] private float playerSpeed = 15f;
    [SerializeField] private float walkSpeed = 15f;
    [SerializeField] private float runSpeed = 20f;
    [SerializeField] private float jumpForce = 300f;
    float horizontalInput;
    bool isRunning = false;
    bool isGrounded = false;
    int lookDirection = 1;
    short jumpCount = 0;

    public int pepperAmount = 0;
    public int maxPepperAmount = 5;
    public float extraPepperDamage = 2f;

    public float maxHealth= 0f;
    public float currentHealth = 0f;

    private Rigidbody2D playerRb;
    private Animator animator;
    private BoxCollider2D playerCollider;


    private bool isKnockbacked = false;
    private float knockbackEndTime = 0f;


    [SerializeField] float jumpVolumeLevel = 0.5f;
    [SerializeField] float fireballVolumeLevel = 0.5f;
    [SerializeField] float hurtVolumeLevel = 0.5f;
    [SerializeField] float aVolumeLevel = 0.5f;
    [SerializeField] float bVolumeLevel = 0.5f;
    [SerializeField] float cVolumeLevel = 0.5f;

    private void OnEnable()
    {
        OnPlayerDeath += DisablePlayerMovement;
    }

    private void OnDisable()
    {
        OnPlayerDeath -= DisablePlayerMovement;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        EnablePlayerMovement();
    }

    private void FixedUpdate()
    {
        if (!isKnockbacked)
        {
            playerSpeed = isRunning ? runSpeed : walkSpeed;
            playerRb.velocity = new Vector2(horizontalInput * playerSpeed, playerRb.velocity.y);
            animator.SetFloat("xVelocity", Math.Abs(playerRb.velocity.x));
        }
    }


    void Update()
    {
        currentTime += Time.deltaTime;
        // Horizontal Movement
        if (!isKnockbacked)
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }

        FlipPlayer();

        //Running
        if (Input.GetKey(KeyCode.LeftShift))
            isRunning = true;
        else
            isRunning = false;

        HandleJumping();

        HandleFireballLogic();

        HandleMovementSound();
    }

    private void FlipPlayer()
    {
        // Flip character based on the move direction
        if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            lookDirection = -1;
        }
        else if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
            lookDirection = 1;
        }
    }

    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            animator.SetBool("isJumping", true);
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            SoundManager.instance.PlayEffectSound(SoundManager.instance.PlayerEffect_Source, SoundManager.instance.Jump, jumpVolumeLevel);
            isGrounded = false;
            animator.SetBool("isGrounded", false);
            jumpCount++;
        }
    }

    private void HandleMovementSound()
    {
        if (isGrounded)
        {
            if (isRunning)
            {
                SoundManager.instance.PlayRunningSound();
            }
            else if (horizontalInput != 0f)
            {
                SoundManager.instance.PlayWalkingSound();
            }
            else
            {
                SoundManager.instance.StopMovementSound();
            }
        }
        else
        {
            SoundManager.instance.StopMovementSound();
        }
    }


    public void ApplyKnocback(Vector2 knockbackForce, float duration)
    {
        isKnockbacked = true;
        playerRb.AddForce(knockbackForce, ForceMode2D.Impulse);
        knockbackEndTime = Time.time + duration;
        StartCoroutine(KnockbackCoroutine(duration));
    }

    private IEnumerator KnockbackCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        isKnockbacked = false;
    }

    private void HandleFireballLogic()
    {
        isFired = false;
        if (Input.GetKeyDown(KeyCode.E) && currentTime >= fireballCooldown)
        {
            if (pepperAmount > 0)
            {
                isFired = true;
                SoundManager.instance.PlayEffectSound(SoundManager.instance.PlayerEffect_Source, SoundManager.instance.FireballThrow, fireballVolumeLevel);
                animator.SetTrigger("isFired");
                ThrowFireball();
                currentTime = 0f;
                pepperAmount--;
            }
        }
    }


    private void ThrowFireball()
    {
        // Instantiate the fireball at the fireBallSpawnPos position with no parent
        int yRotation = lookDirection == 1 ? 0 : 180;

        Quaternion fireballRotation = Quaternion.Euler(0, yRotation, 0);

        GameObject fireballInstance = Instantiate(fireball, fireBallSpawnPos.position, fireballRotation);
        fireballInstance.transform.parent = null; // Ensure it's not parented to the player or the spawn position
        FireBallThrow fireballScript = fireballInstance.GetComponent<FireBallThrow>();

        // Set the fireball direction based on the player's facing direction
        fireballScript.direction = lookDirection;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("isDamaged");
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0f)
        {
            OnPlayerDeath?.Invoke();
        }
        SoundManager.instance.PlayEffectSound(SoundManager.instance.PlayerEffect_Source, SoundManager.instance.PlayerHurt, hurtVolumeLevel);
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    private void DisablePlayerMovement()
    {
        animator.enabled = false;
        playerRb.bodyType = RigidbodyType2D.Static;
    }

    private void EnablePlayerMovement()
    {
        animator.enabled = true;
        playerRb.bodyType = RigidbodyType2D.Dynamic;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
            animator.SetBool("isJumping", false);
            jumpCount = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            pepperAmount++;
            Destroy(collision.gameObject);
            if (pepperAmount > maxPepperAmount)
            {
                TakeDamage(extraPepperDamage);
                pepperAmount = maxPepperAmount;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
            SoundManager.instance.StopMovementSound();
            animator.SetBool("isGrounded", false);
        }
    }
}
