using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using System;


public class PlayerGrab : MonoBehaviour {

    PlayerControls playerControls;
    public GameObject hitboxGrab;
    public float hitboxTime;
    private float timer;
    private bool timerActive;

    public List<GameObject> jars = new List<GameObject>();


    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    void Start() {
            
    }




    void Update() {
        bool grabButton = playerControls.Main.Grab.triggered;

        if (grabButton) {
            hitboxGrab.SetActive(true);
            timer = hitboxTime;
            timerActive = true;
        }


        // Hitbox deactivate timer
        if (timerActive) {
            if (timer > 0) {
                timer -= Time.deltaTime;
            } else {
                timerActive = false;
                hitboxGrab.SetActive(false);
            }
        }

    }

}
