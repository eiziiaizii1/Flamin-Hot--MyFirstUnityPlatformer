using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallThrow : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    public int direction = 1;
    private Rigidbody2D fireballRb;

    // Start is called before the first frame update
    void Start()
    {
        fireballRb = GetComponent<Rigidbody2D>();
        fireballRb.velocity = new Vector2(speed * direction, 0f);
    }
}
