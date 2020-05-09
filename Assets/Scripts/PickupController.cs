using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupController : MonoBehaviour {
    static string baseBuyText = "Press 'E' to buy ";
    static string baseAmmoText = "Press 'E' to purchase ammo for ";

    public Text pickupText;
    public string weaponName;
    public int weaponIndex = -1;

    private GameObject player;
    private bool canBuy = false;
    private bool hasWeapon = false;

    void Start() {
        pickupText = pickupText.GetComponent<Text>();
    }

    void Update() {
        if (Input.GetKeyDown("e") && canBuy) {
            if (hasWeapon) {
                player.GetComponent<InventoryController>().PurchaseAmmo(weaponIndex);
            } else {
                player.GetComponent<InventoryController>().GetNewWeapon(weaponIndex);
                
                hasWeapon = true;
                pickupText.text = baseAmmoText + weaponName;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            canBuy = true;
            player = other.gameObject;

            hasWeapon = player.GetComponent<InventoryController>().HasWeapon(weaponIndex);

            if (hasWeapon) {
                pickupText.text = baseAmmoText + weaponName;
            } else {
                pickupText.text = baseBuyText + weaponName;
            }

            pickupText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            pickupText.gameObject.SetActive(false);
            canBuy = false;
            player = null;
        }
    }
}