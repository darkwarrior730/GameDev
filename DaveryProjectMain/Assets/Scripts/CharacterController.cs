using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public int thisCharId = 0;
    public bool lastFlipSprite = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private GameObject UI;
    private LevelManager level;
    private int currentCharId;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        UI = GameObject.Find("UI");
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        level = UI.GetComponent<LevelManager>();
        currentCharId = level.currentCharId;

        if (currentCharId == thisCharId)
        {
            move.x = Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Jump") && grounded)
            {
                velocity.y = jumpTakeOffSpeed;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * 0.5f;
                }
            }
        }

        // animations
        if (velocity.x != 0.0f)
        {
            animator.Play("Run");
        }
        else
        {
            animator.Play("Idle");
        }
        //bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        bool flipSprite = (move.x < 0.0f);
        if (flipSprite)
        {
            if (lastFlipSprite == false)
            {
                lastFlipSprite = true;
                flipSprite = true;
            }
            else
            {
                lastFlipSprite = true;
                flipSprite = false;
            }
        } else
        {
            if (move.x != 0.0f)
            {
                if (lastFlipSprite == true)
                {
                    lastFlipSprite = false;
                    flipSprite = true;
                }
                else
                {
                    lastFlipSprite = false;
                    flipSprite = false;
                }
            }
        }
        if (flipSprite)
        {
            //spriteRenderer.flipX = !spriteRenderer.flipX;
            transform.Rotate(0.0f, 180.0f, 0.0f);
            if (move.x < 0.0f)
            {
                transform.Translate(0.1f, 0.0f, 0.0f);
            } else if (move.x > 0.0f)
            {
                transform.Translate(0.1f, 0.0f, 0.0f);
            }
        }
        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityx", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }

    private void LateUpdate()
    {
        if (currentCharId == thisCharId)
        {
            if (Input.GetButtonDown("Switch"))
            {
                if (level.currentCharId + 1 < level.totalChars)
                {
                    level.currentCharId += 1;
                }
                else
                {
                    level.currentCharId = 0;
                }
            }
        }
    }
}

