using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    public GameObject highScores;

    public void HighScores() {
        highScores.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Back() {
        highScores.SetActive(false);
        gameObject.SetActive(true);
    }

    public void PlayGame() {
        SceneManager.LoadScene("Main");
    }
}
