using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour {

    public GameObject optionsLayer;
    private bool optionsActive = false;

    public void StartGame() {
        SceneManager.LoadScene("Level1Revamp");
    }

    public void Options() {
        optionsActive = !optionsActive;
        optionsLayer.SetActive(optionsActive);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
