using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
    public GameObject optionsPanel;
    public bool paused = false;

    void Start() {
        
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
            optionsPanel.SetActive(paused);
            Cursor.visible = paused;
            if (paused) {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
            } else {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

    }
}
