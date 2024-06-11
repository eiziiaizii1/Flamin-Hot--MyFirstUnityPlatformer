using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    //private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        playerSpeed = isRunning ? runSpeed : walkSpeed;
        
        playerRb.velocity = new Vector2(horizontalInput * playerSpeed, playerRb.velocity.y);
        animator.SetFloat("xVelocity", Math.Abs(playerRb.velocity.x));
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        // Horizontal Movement
        horizontalInput = Input.GetAxis("Horizontal");
        //animator.SetFloat("xVelocity", Mathf.Abs(horizontalInput));


        //Debug.Log(playerRb.velocity.x);

        //Flip sprite based on the move direction
        if (horizontalInput < -0.01f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            lookDirection = -1;
        }  
        else if (horizontalInput > 0.01f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            lookDirection = 1;
        }
            

        //Running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
           isRunning = false;
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            animator.SetBool("isJumping", true);
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isGrounded", false);
            jumpCount++;
        }


        // Throwing Fireballs
        isFired = false;
        if (Input.GetKeyDown(KeyCode.E) && currentTime >= fireballCooldown)
        {
            if (pepperAmount > 0)
            {
                isFired = true;
                ThrowFireball();
                currentTime = 0f;
                pepperAmount--;
            }
        }
    }

    private void ThrowFireball()
    {
        // Instantiate the fireball at the fireBallSpawnPos position with no parent
        GameObject fireballInstance = Instantiate(fireball, fireBallSpawnPos.position, fireBallSpawnPos.rotation);
        fireballInstance.transform.parent = null; // Ensure it's not parented to the player or the spawn position
        FireBallThrow fireballScript = fireballInstance.GetComponent<FireBallThrow>();

        // Set the fireball direction based on the player's facing direction
        fireballScript.direction = lookDirection;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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
            animator.SetBool("isGrounded", false);
        }
    }
}
