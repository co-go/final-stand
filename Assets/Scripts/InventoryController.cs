using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {
    public Text primaryName;
    public Text primaryAmmo;
    public GameObject primaryHighlight;
    public Text secondaryName;
    public Text secondaryAmmo;
    public GameObject secondaryHighlight;

    public GameObject[] weapons;

    // if specify a -1 in this array, it will be treated as an empty spot
    public int[] currentWeapons = new int[] {0, 1};
    public int equippedWeapon = 0;

    void Start() {
        primaryName = primaryName.GetComponent<Text>();
        primaryAmmo = primaryAmmo.GetComponent<Text>();
        secondaryName = secondaryName.GetComponent<Text>();
        secondaryAmmo = secondaryAmmo.GetComponent<Text>();
    }

    void Update() {
        // swap between weapons here
        if (Input.GetKeyDown("1") && equippedWeapon != 0) {
            weapons[currentWeapons[equippedWeapon]].GetComponent<WeaponController>().CancelReload();
            weapons[currentWeapons[equippedWeapon]].SetActive(false);
            weapons[currentWeapons[0]].SetActive(true);
            equippedWeapon = 0;
            UpdateHighlight();
        } else if (Input.GetKeyDown("2") && equippedWeapon != 1) {
            weapons[currentWeapons[equippedWeapon]].GetComponent<WeaponController>().CancelReload();
            weapons[currentWeapons[equippedWeapon]].SetActive(false);
            weapons[currentWeapons[1]].SetActive(true);
            equippedWeapon = 1;
            UpdateHighlight();
        }
    }

    public void UpdateHighlight() {
        if (equippedWeapon == 0) {
            primaryHighlight.SetActive(true);
            secondaryHighlight.SetActive(false);
        } else {
            primaryHighlight.SetActive(false);
            secondaryHighlight.SetActive(true);
        }
    }

    public void UpdateWeaponInfo(bool primary, string name, float currAmmo, float reserveAmmo) {
        if (primary) {
            primaryName.text = name;
            primaryAmmo.text = currAmmo + " | " + reserveAmmo;
        } else {
            secondaryName.text = name;
            secondaryAmmo.text = currAmmo + " | " + reserveAmmo;
        }
    }
}
