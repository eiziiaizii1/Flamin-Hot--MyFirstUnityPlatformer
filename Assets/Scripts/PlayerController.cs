using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 0.1f;

    private Rigidbody2D playerRb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(new Vector2(horizontalInput * playerSpeed, 0.0f), ForceMode2D.Impulse);
        //playerRb.velocity = new Vector2(horizontalInput * playerSpeed, playerRb.velocity.y);
        animator.SetFloat("HorizontalSpeed", math.abs(playerRb.velocity.x));

        Debug.Log(horizontalInput);

        if (horizontalInput == 0 )
        {
            animator.SetBool("isSliding", true);
        }else
            animator.SetBool("isSliding", false);


        //Flip sprite based on the move direction
        if (horizontalInput < -0.01f)
            spriteRenderer.flipX = true;
        else if (horizontalInput > 0.01f)
            spriteRenderer.flipX = false;
    }
}
