using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    CharacterController characterController;

    public int health = 100;
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float sprintMultiplier = 2.0f;

    public Text healthText;

    private Vector3 moveDirection = Vector3.zero;

    void Start() {
        characterController = GetComponent<CharacterController>();
        healthText.text = health + " / 100";
    }

    void Update() {
        if (characterController.isGrounded) {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            if (Input.GetKey("left shift")) {
                moveDirection *= sprintMultiplier;
            }

            moveDirection = Camera.main.transform.TransformDirection(moveDirection);
            moveDirection.y = 0.0f;

            if (Input.GetButton("Jump")) {
                moveDirection.y = jumpSpeed;
            }
        } else {
            float prevY = moveDirection.y;
            float inAirMultiplier = 0.8f * speed;

            if (Input.GetKey("left shift")) {
                inAirMultiplier *= sprintMultiplier;
            }

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = Camera.main.transform.TransformDirection(moveDirection);
            moveDirection *= inAirMultiplier;
            moveDirection.y = prevY;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthText.text = health + " / 100";
    }
}
