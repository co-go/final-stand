using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    public GameObject pistol;

    public float damage = 10f;
    public float range = 100f;
    public ParticleSystem muzzleFlash;

    private Animator animator;

    void Start() {
        animator = pistol.GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    void Shoot() {
        muzzleFlash.Play();
        animator.SetTrigger("fire");

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range)) {
            EnemyController enemy = hit.transform.GetComponent<EnemyController>();
            if (enemy != null) {
                enemy.TakeDamage(damage);
            }
        }
    }
}
