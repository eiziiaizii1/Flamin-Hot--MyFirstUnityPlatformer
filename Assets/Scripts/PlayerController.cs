using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 15f;
    [SerializeField] private float walkSpeed = 15f;
    [SerializeField] private float runSpeed = 20f;
    float horizontalInput;
    bool isRunning = false;
    bool isGrounded = false;

    int pepperAmount = 0;

    private Rigidbody2D playerRb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CapsuleCollider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        // Horizontal Movement
        horizontalInput = Input.GetAxis("Horizontal");
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(horizontalInput));


        Debug.Log(playerRb.velocity.x);

        //Flip sprite based on the move direction
        if (horizontalInput < -0.01f)
            spriteRenderer.flipX = true;
        else if (horizontalInput > 0.01f)
            spriteRenderer.flipX = false;

        //Running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
           isRunning = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetBool("isJumping", true);
            playerRb.AddForce(Vector2.up * 300f, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
            animator.SetBool("isJumping", false);
        }
        
        if(collision.gameObject.CompareTag("Collectible"))
        {
            Destroy(collision.gameObject);
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
