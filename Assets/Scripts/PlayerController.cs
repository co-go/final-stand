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

    public Text gameOverText;
    public Text scoreText;

    public Text healthText;
    public RectTransform healthBar;
    private float fullHealth;
    private float oneHealthPoint;
    private float healthBarIncrement;
    private float currentHealthBar;
    private bool alive;

    public float regenDelay = 5.0f;
    public float healthRegenSpeed;
    private float timeLastHit;
    private float regenCounter;

    private Vector3 moveDirection = Vector3.zero;

    private InventoryController inventoryController;

    void Start() {
        characterController = GetComponent<CharacterController>();
        inventoryController = GetComponent<InventoryController>();
        healthText.text = health + " / 100";
        fullHealth = healthBar.offsetMax.x;
        oneHealthPoint = (fullHealth - healthBar.offsetMin.x) / 100;
        healthBarIncrement = oneHealthPoint * 20;
        currentHealthBar = fullHealth;
        timeLastHit = 0f;
        regenCounter = 0;
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

        if (Time.time > timeLastHit + regenDelay && health < 100)
        {
            if (regenCounter >= 20 - healthRegenSpeed)
            {
                health += 1;
                healthText.text = health + " / 100";
                currentHealthBar += oneHealthPoint;
                healthBar.offsetMax = new Vector2(currentHealthBar, healthBar.offsetMax.y);
                regenCounter = 0;
            }
            else
            {
                regenCounter++;
            }
            
        }
    }

    private void Die()
    {
        scoreText.text = "Score: " + inventoryController.GetScore();
        healthBar.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        alive = false;
    }

    public void TakeDamage(int damage)
    {
        timeLastHit = Time.time;
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
    
    public bool isAlive()
    {
        return alive;
    }
}
