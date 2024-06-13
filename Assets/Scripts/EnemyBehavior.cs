using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    Animator animator;
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
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }


    protected void throwSnowBall()
    {
        animator.SetTrigger("isAttacked");
    }
}
