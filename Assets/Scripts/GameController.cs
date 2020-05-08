using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject pauseMenu;
    public GameObject crosshair;
    public GameObject player;
    public Camera mainCamera;

    bool paused = false;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        crosshair.SetActive(true);
    }

    void Update() {
        if (Input.GetKeyDown("escape")) {
            TogglePause();
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
}