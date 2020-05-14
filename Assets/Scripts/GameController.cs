using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject pauseMenu;
    public GameObject crosshair;
    public GameObject player;
    public GameObject playerUI;
    public GameObject weaponUI;
    public Camera mainCamera;
    public float scale;
    public bool paused = false;
    
    int screenWidth = Screen.width;
    int screenHeight = Screen.height;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        crosshair.SetActive(true);
        UpdateUIScale();
    }

    void Update() {
        if (Input.GetKeyDown("escape")) {
            TogglePause();
        }

        if (Screen.height != screenHeight || Screen.width != screenWidth)
        {
            UpdateUIScale();
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
}