using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float speed = 20f;
    public Rigidbody2D rb2d;
    public new Collider2D collider;

	// Use collision.name to find what has been collided with

	void Start () {
        rb2d.velocity = transform.right * speed;
        gameObject.layer = 9;
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Physics2D.IgnoreCollision(collision.collider, collider);
    }
    
}
