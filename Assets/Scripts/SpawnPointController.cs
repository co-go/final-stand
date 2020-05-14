using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    public GameObject maleZombie;
    public GameObject femaleZombie;
    public bool active;

    private Vector3 location;
    private float nextSpawnTime;

    private EnemyController enemy;

    // Start is called before the first frame update
    void Start()
    {
        location = transform.position;
        nextSpawnTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + 5.0f;
            SpawnZombie(1);
        }
    }

    public void SpawnZombie(int waveNumber)
    {
        if (Random.value < 0.5f)
        {
            enemy = Instantiate(maleZombie, location, Quaternion.identity).GetComponent<EnemyController>();
        }
        else
        {
            enemy = Instantiate(femaleZombie, location, Quaternion.identity).GetComponent<EnemyController>();
        }

        /* enemy speed & health varies based on wave progression */
        if (waveNumber < 3)
        {
            enemy.moveSpeed = 1;
        }
        else if (waveNumber < 6)
        {
            enemy.moveSpeed = Random.value * (enemy.maxSpeed/2);
        }
        else if (waveNumber < 12)
        {
            enemy.moveSpeed = Random.value * enemy.maxSpeed;
        }
        else
        {
            enemy.moveSpeed = (enemy.maxSpeed/2) + Random.value * (enemy.maxSpeed/2);
        }
        
        enemy.health = enemy.health * waveNumber;
    }
}
