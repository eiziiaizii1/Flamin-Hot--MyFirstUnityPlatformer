using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballThrow : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float destroyTime;
    [SerializeField] float damage;
    private Vector2 direction;

    [SerializeField] float snowballImpactVolumeLevel = .5f;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void setDirection(Vector2 dir)
    {
        direction = dir;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            SoundManager.instance.PlayEffectSound(SoundManager.instance.EnemyEffect_Source, SoundManager.instance.SnowballImpact, snowballImpactVolumeLevel);
        }
    }
}
