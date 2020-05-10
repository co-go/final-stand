using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour {
    public Image ReloadBar;

    public float range = 100f;
    public float damage;
    public float fireRate;
    public float impactForce = 100f;
    public bool fullAuto = false;

    public float reserveAmmo;
    public float magSize;
    public float currAmmo;
    public float reloadTime;
    private bool isReloading = false;
    private IEnumerator reloadCoroutine;

    public ParticleSystem muzzleFlash;
    public GameObject defaultImpact;
    public GameObject enemyImpact;
    public GameObject dirtImpact;

    public int equipSlot = -1;

    private InventoryController inventory;
    private Animator animator;
    private float nextTimeToFire = 0f;

    public AudioClip fireAudioClip;
    public AudioClip reloadAudioClip;
    private AudioSource fireAudioSource;
    private AudioSource reloadAudioSource;

    void Start() {
        ReloadBar = ReloadBar.GetComponent<Image>();
        animator = GetComponent<Animator>();
        inventory = transform.parent.transform.parent.transform.parent.GetComponent<InventoryController>();

        if (equipSlot != -1) SendWeaponState();

        ReloadBar.fillAmount = 0;

        fireAudioSource = AddAudio(false, false, 1.0f, 1.0f, fireAudioClip);
        reloadAudioSource = AddAudio(false, false, 1.0f, 0.5f, reloadAudioClip);
    }

    void OnEnable() {
        // fix occasional artifact from canceling reload
        ReloadBar.fillAmount = 0;
    }

    void SendWeaponState() {
        inventory.UpdateWeaponInfo(equipSlot == 0, transform.name, currAmmo, reserveAmmo);
    }

    void Update() {
        if (!isReloading) {
            if (Time.time >= nextTimeToFire && (Input.GetButton("Fire1") && fullAuto || Input.GetButtonDown("Fire1") && !fullAuto)) {
                // set the next firing time to a point in the future (relative to current)
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }

            if (Input.GetKeyDown("r") && reserveAmmo > 0 && currAmmo < magSize && !isReloading) {
                reloadCoroutine = Reload();
                StartCoroutine(reloadCoroutine);
            }
        } else {
            ReloadBar.fillAmount += 1.0f / reloadTime * Time.deltaTime;
        }
    }

    void Shoot() {
        if (currAmmo > 0) {
            muzzleFlash.Play();
            fireAudioSource.Play();
            animator.SetTrigger("fire");

            currAmmo -= 1;

            // moved slightly forward so to avoid colliding with the player
            Vector3 origin = Camera.main.transform.position + Camera.main.transform.forward.normalized / 2f;
            RaycastHit hit;
            if (Physics.Raycast(origin, Camera.main.transform.forward, out hit, range)) {
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

            SendWeaponState();
        }

        if (currAmmo <= 0 && reserveAmmo > 0) StartCoroutine(Reload());
    }

    IEnumerator Reload() {
        isReloading = true;
        animator.SetTrigger("reload");
        reloadAudioSource.Play();
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        ReloadBar.fillAmount = 0;

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

        SendWeaponState();
        reloadCoroutine = null;
    }

    public void CancelReload() {
        if (reloadCoroutine != null) {
            Debug.Log("we canceling reload for: " + transform.name);
            StopCoroutine(reloadCoroutine);
            isReloading = false;
            animator.enabled = false;
            animator.enabled = true;
        }
    }

    public void RefillAmmo() {
        reserveAmmo += magSize * 6;
        SendWeaponState();
    }

    /* Creates new AudioSource component */
    public AudioSource AddAudio(bool loop, bool playAwake, float vol, float pitch, AudioClip clip)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;
        newAudio.pitch = pitch;
        return newAudio;
    }
}
