using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class OldPlayerMovement : MonoBehaviour {

    PlayerControls playerControls;
    CharacterController controller;
    Vector3 playerVelocity;
    bool touchingGround;
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float gravity;

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
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update() {
        touchingGround = controller.isGrounded;
        if (touchingGround && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }
        
        Vector2 inputMove = playerControls.Main.Move.ReadValue<Vector2>();

        Vector3 move = new Vector3(inputMove.x, 0, inputMove.y);
        controller.Move(move * Time.deltaTime * speed);

        if (move !=  Vector3.zero) {
            gameObject.transform.forward = move;
        }


        if (playerControls.Main.Jump.triggered && touchingGround) {
            playerVelocity.y += Mathf.Sqrt(jumpSpeed * -3f * gravity);
            AudioManager.instance.PlayOneShot(jumpSound, this.transform.position);
        }

        if (touchingGround) {
            Debug.Log("touching ground");
        } else {
            Debug.Log("not on ground");
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
