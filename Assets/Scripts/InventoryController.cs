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

    public Animator weaponHolder;
    public float swapTime;

    public Text pointsText;
    public int points = 500;
    private int score;

    public GameObject[] weapons;

    // if specify a -1 in this array, it will be treated as an empty spot
    public int[] currentWeapons = new int[] {0, -1};
    public int equippedWeapon = 0;

    void Start() {
        primaryName = primaryName.GetComponent<Text>();
        primaryAmmo = primaryAmmo.GetComponent<Text>();
        secondaryName = secondaryName.GetComponent<Text>();
        secondaryAmmo = secondaryAmmo.GetComponent<Text>();
        UpdatePointsText();
        score = points;
    }

    void Update() {
        // swap between weapons here
        if (Input.GetKeyDown("1") && equippedWeapon != 0) {
            StartCoroutine(SwapWeapons(1));
        } else if (Input.GetKeyDown("2") && equippedWeapon != 1 && currentWeapons[1] != -1) {
            StartCoroutine(SwapWeapons(2));
        }
    }

    IEnumerator SwapWeapons(int swapTo) {
        weaponHolder.SetTrigger("Swapping");
        yield return new WaitForSeconds(swapTime);
        HolsterWeapon();
        equippedWeapon = swapTo - 1;
        DrawWeapon();
        UpdateHighlight();
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

    public bool HasWeapon(int weaponIndex) {
        return currentWeapons[0] == weaponIndex || currentWeapons[1] == weaponIndex;
    }

    public void GetNewWeapon(int weaponIndex, int weaponCost) {
        // if we don't have a second weapon, put it there
        if (currentWeapons[1] == -1) {
            currentWeapons[1] = weaponIndex;
            StartCoroutine(SwapWeapons(2));
        }
        else
        {
            HolsterWeapon();
            currentWeapons[equippedWeapon] = weaponIndex;
            DrawWeapon();
        }
        points -= weaponCost;
        UpdatePointsText();
    }

    public void PurchaseAmmo(int weaponIndex, int ammoCost) {
        weapons[weaponIndex].GetComponent<WeaponController>().RefillAmmo();
        points -= ammoCost;
        UpdatePointsText();
    }

    public void AddPoints(int newPoints)
    {
        points += newPoints;
        score += newPoints;
        UpdatePointsText();
    }

    public void SpendPoints(int cost)
    {
        points -= cost;
        UpdatePointsText();
    }

    public int GetScore()
    {
        return score;
    }

    private void UpdatePointsText()
    {
        pointsText.text = "Points: " + points;
    }

    private void HolsterWeapon() {
        weapons[currentWeapons[equippedWeapon]].SetActive(false);
    }

    private void DrawWeapon() {
        weapons[currentWeapons[equippedWeapon]].SetActive(true);
    }
}
