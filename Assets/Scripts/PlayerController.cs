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
    public Text gameOverText;
    public RectTransform healthBar;
    private float fullHealth;
    private float healthBarIncrement;
    private float currentHealthBar;
    private bool alive;

    private Vector3 moveDirection = Vector3.zero;

    void Start() {
        characterController = GetComponent<CharacterController>();
        healthText.text = health + " / 100";
        fullHealth = healthBar.offsetMax.x;
        healthBarIncrement = (fullHealth - healthBar.offsetMin.x) / 5;
        currentHealthBar = fullHealth;
        alive = true;
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
        if (alive) characterController.Move(moveDirection * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        healthText.text = health + " / 100";
        currentHealthBar -= healthBarIncrement;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            healthBar.offsetMax = new Vector2(currentHealthBar, healthBar.offsetMax.y);
        }
    }

    private void Die()
    {
        healthBar.gameObject.SetActive(false);
        gameOverText.enabled = true;
        alive = false;
    }
}
