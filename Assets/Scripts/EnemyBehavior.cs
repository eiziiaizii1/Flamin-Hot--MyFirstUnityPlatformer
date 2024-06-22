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

    private PlayerController playerController;

    [SerializeField] float throwRange = 10f;

    [SerializeField] float minTimeAmount = 1.0f;
    [SerializeField] float maxTimeAmount = 10.0f;
    [SerializeField] float spawnDelay = 1.0f;
    float currentTime = 0f;


    [SerializeField] float knockbackPower = 10f;
    [SerializeField] float knockbackDuration = 0.2f;

    public float health = 10f;
    public float damage = 2f;

    [SerializeField] float snowballThrowVolume = .5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = playerPos.GetComponent<PlayerController>();

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

        if (!playerController.isDead && playerDistance <= throwRange)
        {
            Vector2 throwDirection = (playerPos.position - throwPos.position).normalized;
            GameObject projectile = Instantiate(snowball, throwPos.position, Quaternion.identity);
            projectile.GetComponent<SnowballThrow>().setDirection(throwDirection);
            animator.SetTrigger("isAttacked");
            SoundManager.instance.PlayEffectSound(SoundManager.instance.EnemyEffect_Source, SoundManager.instance.SnowballThrow, snowballThrowVolume);
        }
    }

    protected void faceToPlayer()
    {
        if (playerPos == null) return;

        if (playerPos.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
        }else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerController != null) {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                Vector2 knockbackForce = knockbackDirection * knockbackPower;
                playerController.TakeDamage(damage);

                playerController.ApplyKnocback(knockbackForce,knockbackDuration);
                //Debug.Log($"Knockback direction: {knockbackDirection}, Knockback force: {knockbackDirection * knockbackPower}");
            }

        }
    }
}
