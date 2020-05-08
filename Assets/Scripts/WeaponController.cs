using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour {
    public GameObject pistol;

    public GameObject Primary;
    public GameObject PrimaryAmmo;
    public GameObject PrimaryHighlight;
    public GameObject Secondary;
    public GameObject SecondaryAmmo;
    public GameObject SecondaryHighlight;

    private Text primaryText;
    private Text primaryAmmoText;
    private Text secondaryText;
    private Text secondaryAmmoText;

    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 10f;
    public float impactForce = 100f;

    public float reserveAmmo = 60f;
    public float magSize = 8f;
    public float currAmmo = 8f;

    public ParticleSystem muzzleFlash;
    public GameObject defaultImpact;
    public GameObject enemyImpact;
    public GameObject dirtImpact;

    private Animator animator;
    private float nextTimeToFire = 0f;

    void Start() {
        primaryText = Primary.GetComponent<Text>();
        primaryAmmoText = PrimaryAmmo.GetComponent<Text>();
        secondaryText = Secondary.GetComponent<Text>();
        secondaryAmmoText = SecondaryAmmo.GetComponent<Text>();

        animator = pistol.GetComponent<Animator>();
    }

    void Update() {
        // Full-auto
        // if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) 

        // Semi-auto firing
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire) {
            // set the next firing time to a point in the future (relative to current)
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown("r") && reserveAmmo > 0) {
            Reload();
        }
    }

    void Shoot() {
        if (currAmmo > 0) {
            muzzleFlash.Play();
            animator.SetTrigger("fire");

            currAmmo -= 1;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range)) {
                EnemyController enemy = hit.transform.GetComponent<EnemyController>();
                if (enemy != null) {
                    enemy.TakeDamage(damage);
                }

                if (hit.rigidbody != null) {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }

                if (hit.transform.tag == "Dirt") {
                    Instantiate(dirtImpact, hit.point, Quaternion.LookRotation(hit.normal));
                } else if (hit.transform.tag == "Enemy") {
                    Instantiate(enemyImpact, hit.point, Quaternion.LookRotation(hit.normal));
                } else {
                    Instantiate(defaultImpact, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }

            UpdateInfo();
        }

        if (currAmmo <= 0) Reload();
    }

    void Reload() {
        if (reserveAmmo > 0) {
            // if we have bullets in mag, return to pool
            reserveAmmo += currAmmo;
            currAmmo = 0;

            if (reserveAmmo < magSize) {
                currAmmo = reserveAmmo;
            } else {
                currAmmo = magSize;
            }

            reserveAmmo -= currAmmo;
        }

        Debug.Log("Reloading... ");
        UpdateInfo();
    }

    void UpdateInfo() {
        primaryAmmoText.text = currAmmo + " | " + reserveAmmo;
    }
}
