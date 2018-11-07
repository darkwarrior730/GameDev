using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float minGroundNormalY = 0.65f;
    public float gravityMod = 1f;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Vector2 velocity;
    protected Rigidbody2D rb2d;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float shellRadius = 0.01f;
    protected const float minMoveDistance = 0.001f;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {
        // To be replaced with individual object velocity calculation
    }

    void FixedUpdate()
    {
        velocity += gravityMod * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;
        deltaPosition = PixelClamp(deltaPosition);

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            hitBufferList = CreateHitBufferList(move, distance);

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;

                CheckGrounded(currentNormal, yMovement);

                velocity = ReduceVelocityIfCollision(velocity, currentNormal);
                distance = ReduceDistanceMovedIfCollision(distance, hitBufferList[i].distance);
            }

        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }

    

    private Vector2 PixelClamp(Vector2 moveVector)
    {
        Vector2 clampedVector = new Vector2(
            moveVector.x < 0 ? Mathf.Floor(moveVector.x * 16) : Mathf.Ceil(moveVector.x * 16),
            moveVector.y < 0 ? Mathf.Floor(moveVector.y * 16) : Mathf.Ceil(moveVector.y* 16)
            );

        return clampedVector/16;
    }

    private List<RaycastHit2D> CreateHitBufferList(Vector2 move, float distance)
    {
        int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
        hitBufferList.Clear();
        for (int i = 0; i < count; i++)
        {
            hitBufferList.Add(hitBuffer[i]);
        }
        return hitBufferList;
    }

    void CheckGrounded(Vector2 currentNormal, bool yMovement)
    {
        if (currentNormal.y > minGroundNormalY)
        {
            grounded = true;
            if (yMovement)
            {
                groundNormal = currentNormal;
                currentNormal.x = 0;
            }
        }
    }

    private Vector2 ReduceVelocityIfCollision(Vector2 velocity, Vector2 currentNormal)
    {
        float projection = Vector2.Dot(velocity, currentNormal);
        if (projection < 0)
        {
            return (velocity - projection * currentNormal);
        }
        return velocity;
    }

    private float ReduceDistanceMovedIfCollision(float distanceToMove, float distanceFromCollision)
    {
        float modifiedDistance = distanceFromCollision - shellRadius;
        return (modifiedDistance < distanceToMove ? modifiedDistance : distanceToMove);
    }

}
