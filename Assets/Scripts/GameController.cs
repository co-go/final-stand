using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public GameObject pauseMenu;
    public GameObject crosshair;
    public GameObject player;
    public GameObject playerUI;
    public GameObject weaponUI;
    public Camera mainCamera;
    public float scale;
    public bool paused = false;

    public Text RoundText;
    public int round;
    public int timeBetweenRounds;
    public int activeZombieCap;
    public int zombiesInGame;
    public float spawnDelay;
    public bool spawningEnabled;

    private int zombiesRemaining;
    private float nextSpawnTime;

    private GameObject[] spawnPoints;

    int screenWidth = Screen.width;
    int screenHeight = Screen.height;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        crosshair.SetActive(true);
        UpdateUIScale();
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawner");
        zombiesRemaining = 5 * (round + 1);
        if (spawningEnabled) StartNewRound();
    }

    void Update() {
        if (Input.GetKeyDown("escape")) {
            TogglePause();
        }

        if (Screen.height != screenHeight || Screen.width != screenWidth)
        {
            UpdateUIScale();
        }

        if (zombiesRemaining == 0)
        {
            zombiesRemaining = 5 * (round + 1);
            activeZombieCap += 2;
            Invoke("StartNewRound", timeBetweenRounds);
        }

        if (spawningEnabled && Time.time > nextSpawnTime && zombiesInGame < activeZombieCap)
        {
            SpawnZombie(FindClosestSpawn().GetComponent<SpawnPointController>());
            nextSpawnTime = Time.time + spawnDelay;
        }
    }

    void TogglePause() {
        paused = !paused;

        if (paused) {
            Time.timeScale = 0.0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        } else {
            Time.timeScale = 1.0f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        player.GetComponent<MouseLook>().Pause(paused);
        mainCamera.GetComponent<MouseLook>().Pause(paused);

        pauseMenu.SetActive(paused);
        crosshair.SetActive(!paused);
    }

    private void UpdateUIScale()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        playerUI.transform.localScale = new Vector3(screenWidth / 1850.0f * scale, screenHeight / 950.0f * scale, 1.0f);
        weaponUI.transform.localScale = new Vector3(screenWidth / 1850.0f * scale, screenHeight / 950.0f * scale, 1.0f);
    }

    private GameObject FindClosestSpawn()
    {
        float tempDist;
        float closestDistance = Vector3.Distance(player.transform.position, spawnPoints[0].transform.position);
        GameObject closestSpawn = spawnPoints[0];
        foreach (GameObject spawn in spawnPoints)
        {
            tempDist = Vector3.Distance(player.transform.position, spawn.transform.position);
            if (tempDist < closestDistance)
            {
                closestDistance = tempDist;
                closestSpawn = spawn;
            }
        } 
        return closestSpawn;
    }

    private void StartNewRound()
    {
        round++;
        RoundText.text = "Round " + round;
        spawningEnabled = true;
    }

    private void SpawnZombie(SpawnPointController spawnPoint)
    {
        zombiesInGame++;
        if (zombiesRemaining == zombiesInGame)
        {
            spawningEnabled = false;
        }
        spawnPoint.SpawnZombie(round);
    }

    public void LowerZombieCount()
    {
        zombiesRemaining--;
        zombiesInGame--;
    }
}