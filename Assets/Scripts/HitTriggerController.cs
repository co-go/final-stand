using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTriggerController : MonoBehaviour
{
    public int damage = 20;

    private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.root.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (other.tag == "Player")
        {
            EnemyController enemy = transform.root.GetComponent<EnemyController>();
            if (enemy.nextTimeToAttack > Time.time && !enemy.hasHit)
            {
                enemy.hasHit = true;
                other.GetComponent<PlayerController>().TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        enemy.GetComponent<EnemyController>().TakeDamage(damage);
    }
}
