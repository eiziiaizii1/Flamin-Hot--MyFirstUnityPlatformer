using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallThrow : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    [SerializeField] float damage = 5f;


    public int direction = 1;
    private Rigidbody2D fireballRb;

    // Start is called before the first frame update
    void Start()
    {
        fireballRb = GetComponent<Rigidbody2D>();
        fireballRb.velocity = new Vector2(speed * direction, 0f);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy"))
    //    {
    //        collision.gameObject.GetComponent<EnemyBehavior>().TakeDamage(damage);
    //    }
    //    Destroy( gameObject);
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyBehavior>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }


    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
