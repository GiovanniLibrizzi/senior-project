using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerMovement : MonoBehaviour {

    PlayerControls playerControls;
    [SerializeField] private EventReference jumpSound;

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
        if (playerControls.Main.Jump.triggered) {
            AudioManager.instance.PlayOneShot(jumpSound, this.transform.position);
        }
    }
}
