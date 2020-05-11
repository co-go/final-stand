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
    public int weaponCost;
    public int ammoCost;

    private GameObject player;
    private InventoryController inventoryController;
    private bool canBuy = false;
    private bool hasWeapon = false;

    void Start() {
        pickupText = pickupText.GetComponent<Text>();
        player = GameObject.Find("Player");
        inventoryController = player.GetComponent<InventoryController>();
    }

    void Update() {
        if (Input.GetKeyDown("e") && canBuy) {
            if (hasWeapon) {
                inventoryController.PurchaseAmmo(weaponIndex, ammoCost);
            } else {
                inventoryController.GetNewWeapon(weaponIndex, weaponCost);
                
                hasWeapon = true;
                pickupText.text = baseAmmoText + weaponName + "\n" + ammoCost + " Points";
            }
            setCanBuy(ammoCost);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {

            hasWeapon = inventoryController.HasWeapon(weaponIndex);

            if (hasWeapon) {
                pickupText.text = baseAmmoText + weaponName + "\n" + ammoCost + " Points";
                setCanBuy(ammoCost);
            } else {
                pickupText.text = baseBuyText + weaponName + "\n" + weaponCost + " Points";
                setCanBuy(weaponCost);
            }

            pickupText.gameObject.SetActive(true);
        }
    }

    private void setCanBuy(int cost)
    {
        if (inventoryController.points >= cost) canBuy = true;
        else canBuy = false;
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            pickupText.gameObject.SetActive(false);
            canBuy = false;
            player = null;
        }
    }
}