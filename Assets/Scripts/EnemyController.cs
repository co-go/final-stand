using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    CharacterController characterController;
    Animator animator;

    public float health = 100f;
    public float minDist = 2.0f;
    public float moveSpeed = 1.0f;
    public float maxSpeed = 6.0f;
    public float gravity = 20.0f;
    public float nextTimeToAttack = 0f;
    public int pointsOnDeath;
    public bool hasHit = false;
    public bool canHit = false;
    public bool on;

    private Vector3 playerLocation;
    private Vector3 moveDirection;

    private GameObject player;
    private InventoryController inventoryController;
    private GameController gameController;
    private NavMeshAgent agent;

    private bool alive = true;
    private float blend;

    public GameObject[] powerUps;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        inventoryController = player.GetComponent<InventoryController>();
        characterController = GetComponent<CharacterController>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        if (on) animator.SetBool("PlayerActive", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (on && alive && Time.time >= nextTimeToAttack)
        {
            if (agent.isStopped) agent.isStopped = false;
            if (hasHit) hasHit = false;
            if (canHit) canHit = false;

            playerLocation = new Vector3(player.transform.position.x, 0.0f, player.transform.position.z);
            transform.LookAt(transform.position);

            blend = (moveSpeed - 1) / (maxSpeed - 1);
            if (moveSpeed > 1.0f) blend = Mathf.Min(blend + 0.3f, 1.0f);

            animator.SetFloat("Blend", blend);

            if (Vector3.Distance(transform.position, player.transform.position) >= minDist)
            {
                agent.SetDestination(player.transform.position);
            }
            else
            {
                Attack();
                Invoke("EnableCanHit", 1);
            }
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0f) {
            animator.SetTrigger("Killed");
            if (alive)
            {
                inventoryController.AddPoints(pointsOnDeath);
                inventoryController.AddKill();
                gameController.LowerZombieCount();
            }
            alive = false;
            characterController.enabled = false;
            agent.isStopped = true;
            
            if (Random.Range(0, 100) < 25)
            {
                Vector3 newPosition = transform.position + new Vector3(0f, 1f, 0f);
                Instantiate(powerUps[Random.Range(0, powerUps.Length)], newPosition, Quaternion.identity);
            }

            Invoke("Die", 5.0f);
        }
    }

    void Die() {
        Destroy(gameObject);
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        nextTimeToAttack = Time.time + 2.0f;
        agent.isStopped = true;
    }

    private void EnableCanHit()
    {
        canHit = true;
    }
}
