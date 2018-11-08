using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 20f;
    public Rigidbody2D rb2d;
    public new Collider2D collider;

	// Use this for initialization
	void Start () {
        rb2d.velocity = transform.right * speed;
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.gameObject.layer != 10)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            Physics2D.IgnoreCollision(collision.collider, collider);
        }
    }

    // Update is called once per frame
    /*void Update () {

    }*/
}
