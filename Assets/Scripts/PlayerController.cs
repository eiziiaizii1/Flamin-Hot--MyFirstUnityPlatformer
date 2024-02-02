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
    bool isRunning=false;

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
        
        playerRb.AddForce(new Vector2(horizontalInput * playerSpeed, 0.0f), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal Movement
        horizontalInput = Input.GetAxis("Horizontal");
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(playerRb.velocity.x));

        bool isMoving = false;
        isMoving = (Mathf.Abs(horizontalInput) <= 0.2f) ? false : true; 
        animator.SetBool("isMoving",isMoving);

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

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            animator.SetBool("isGrounded",true);
        else
            animator.SetBool("isGrounded", false);
    }
}
