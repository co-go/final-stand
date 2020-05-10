using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    CharacterController characterController;
    Animator animator;

    public Transform player;
    public float health = 100f;
    public float minDist = 3.0f;
    public float moveSpeed = 1.0f;
    public float maxSpeed = 5.0f;
    public float gravity = 20.0f;
    public bool on;

    private Vector3 playerLocation;
    private Vector3 moveDirection;



    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (on) animator.SetBool("PlayerActive", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (on)
        {
            playerLocation = new Vector3(player.position.x, 0.0f, player.position.z);
            transform.LookAt(playerLocation);

            if (Vector3.Distance(transform.position, player.position) >= 10.0f)
            {
                moveSpeed = 5;
            }
            else
            {
                moveSpeed = 1;
            }

            animator.SetFloat("Blend", (moveSpeed - 1) / (maxSpeed - 1));

            if (Vector3.Distance(transform.position, player.position) >= minDist)
            {
                moveDirection = transform.forward * moveSpeed;
                moveDirection.y -= gravity;
                characterController.Move(moveDirection * Time.deltaTime);
            }
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0f) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }
}
