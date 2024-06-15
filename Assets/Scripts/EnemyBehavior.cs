using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    Animator animator;


    public GameObject snowball;
    public Transform playerPos;
    public Transform throwPos;

    [SerializeField] float throwRange = 10f;

    [SerializeField] float minTimeAmount = 1.0f;
    [SerializeField] float maxTimeAmount = 10.0f;
    [SerializeField] float spawnDelay = 1.0f;
    float currentTime = 0f;

    public float health = 10f;
    public float damage = 2f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        float spawnInterval = Random.Range(minTimeAmount, maxTimeAmount);
        Debug.Log(spawnInterval);
        InvokeRepeating("throwSnowBall", spawnDelay, spawnInterval);

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        faceToPlayer();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }


    protected void throwSnowBall()
    {
        if (playerPos == null)
            return;

        float playerDistance = Vector2.Distance(playerPos.position, transform.position);

        if (playerDistance <= throwRange)
        {
            Vector2 throwDirection = (playerPos.position - throwPos.position).normalized;
            GameObject projectile = Instantiate(snowball, throwPos.position, Quaternion.identity);
            projectile.GetComponent<SnowballThrow>().setDirection(throwDirection);
            animator.SetTrigger("isAttacked");
        }
    }

    protected void faceToPlayer()
    {
        if (playerPos.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
        }else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
