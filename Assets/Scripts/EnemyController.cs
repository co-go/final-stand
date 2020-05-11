﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    CharacterController characterController;
    Animator animator;

    public float health = 100f;
    public float minDist = 2.0f;
    public float moveSpeed = 1.0f;
    public float maxSpeed = 5.0f;
    public float gravity = 20.0f;
    public float nextTimeToAttack = 0f;
    public int pointsOnDeath;
    public bool hasHit = false;
    public bool on;

    private Vector3 playerLocation;
    private Vector3 moveDirection;

    private GameObject player;
    private InventoryController inventoryController;

    private bool alive = true;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        inventoryController = player.GetComponent<InventoryController>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if (on) animator.SetBool("PlayerActive", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (on && alive)
        {
            playerLocation = new Vector3(player.transform.position.x, 0.0f, player.transform.position.z);
            transform.LookAt(playerLocation);

            if (Vector3.Distance(transform.position, player.transform.position) >= 10.0f)
            {
                moveSpeed = 5;
            }
            else
            {
                moveSpeed = 1;
            }

            animator.SetFloat("Blend", (moveSpeed - 1) / (maxSpeed - 1));

            if (Vector3.Distance(transform.position, player.transform.position) >= minDist)
            {
                moveDirection = transform.forward * moveSpeed;
                moveDirection.y -= gravity;
                characterController.Move(moveDirection * Time.deltaTime);
            }
            else
            {
                if (Time.time >= nextTimeToAttack)
                {
                    animator.SetTrigger("Attack");
                    nextTimeToAttack = Time.time + 2.0f;
                }
            }

            if (hasHit && Time.time >= nextTimeToAttack)
            {
                hasHit = false;
            }
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0f) {
            animator.SetTrigger("Killed");
            if (alive) inventoryController.AddPoints(pointsOnDeath);
            alive = false;
            Invoke("Die", 5.0f);
        }
    }

    void Die() {
        Destroy(gameObject);
    }
}
