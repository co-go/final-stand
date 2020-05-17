using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    public GameObject maleZombie;
    public GameObject femaleZombie;
    public bool active;

    private Vector3 location;

    private EnemyController enemy;

    // Start is called before the first frame update
    void Start()
    {
        location = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnZombie(int roundNumber)
    {
        if (Random.value < 0.5f)
        {
            enemy = Instantiate(maleZombie, location, Quaternion.identity).GetComponent<EnemyController>();
        }
        else
        {
            enemy = Instantiate(femaleZombie, location, Quaternion.identity).GetComponent<EnemyController>();
        }

        /* enemy speed & health varies based on round progression */
        if (roundNumber < 3)
        {
            enemy.moveSpeed = 1;
        }
        else if (roundNumber < 5)
        {
            enemy.moveSpeed = Random.value * (enemy.maxSpeed/2);
        }
        else if (roundNumber < 7)
        {
            enemy.moveSpeed = Random.value * enemy.maxSpeed;
        }
        else
        {
            enemy.moveSpeed = enemy.maxSpeed - Random.value * 2;
        }

        if (roundNumber < 10)
        {
            enemy.health = enemy.health + (50 * roundNumber);
        }
        else
        {
            enemy.health = (enemy.health + (50 * 9)) * Mathf.Pow(1.1f, roundNumber - 9);
        }
    }
}
