﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public int thisCharId = 0;

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
        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
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

