using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerMovement : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Movement")]
    public float moveSpeedBase = 5.57f;
    float moveSpeed;

    public float groundDragBase = 4f;
    float groundDrag;

    public float jumpForceBase = 8.62f;
    float jumpForce;

    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;


    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    PlayerGrab playerGrab;

    bool slimeJar;

    [Header("Other")]
    [SerializeField] GameObject fireTrail;


    // Health
    public int maxHp = 5;
    int hp;
    bool invincibility = false;
    float invincibilityTime = 1f;
    public static event Action<int> OnPlayerHit;



    private void Awake() {
        playerControls = new PlayerControls();

        moveSpeed = moveSpeedBase;
        groundDrag = groundDragBase;
        jumpForce = jumpForceBase;
        hp = maxHp;
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    private void Start()
    {
        playerGrab = GetComponent<PlayerGrab>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        FireJar.OnFireJarInteract += StartFireTrail;
        SlimeJar.OnSlimeJarInteract += StartSlimeStick;
        EnemyMovement.OnEnemyHitsPlayer += TakeDamage;
    }

    private void Update() {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        ThrowJar();

        // handle drag
        if (grounded) {
            //Debug.Log("grounded");
            rb.drag = groundDrag;
        } else {
            //Debug.Log("not grounded");

            rb.drag = 0;
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        Vector2 inputMove = playerControls.Main.Move.ReadValue<Vector2>();
        horizontalInput = inputMove.x;
        verticalInput = inputMove.y;

        // when to jump
        if (playerControls.Main.Jump.triggered && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            AudioManager.instance.PlayOneShot(FMODEvents.instance.jumpSfx, this.transform.position);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer() {
        // FeatherJar movement speed 

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    private void ThrowJar() {
        if (playerControls.Main.Throw.triggered) {
            //Debug.Log("PlayerMovement - Throw Jar");
            Jar jar = playerGrab.RemoveJar();
            // Check if there are any jars 
            if (jar != null) {
                jar.Throw();
            } 
        }
    }

    public void UpdateMoveSpeed(float spd) {
        moveSpeed += spd;
        //Debug.Log("update player move speed: " + spd + ", " + moveSpeed);
    }

    public void UpdateJumpForce(float jump) {
        jumpForce += jump;
    }

    public void StartFireTrail(int amt) {
        // add functionality for different amounts
        if (amt == 1) {
            fireTrail.SetActive(true);
        } else if (amt == 0) {
            fireTrail.SetActive(false);
        }
    }

    public void StartSlimeStick(int amt) {
        if (amt == 1) {
            // stick
            slimeJar = true;
        } else if (amt == 0) {
            // unstick
            slimeJar = false;
        }
    }
    

    public void TakeDamage(int amt) {
        if (!invincibility) {
            StartCoroutine(DamageCoroutine(amt));
        }
    }

    IEnumerator DamageCoroutine(int amt) {
        invincibility = true;
        hp -= amt;
        OnPlayerHit?.Invoke(hp);

        if (hp <= 0) {
            // Dies
            Debug.Log("YOU DIED");
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(invincibilityTime);
        invincibility = false;
    }

    private void OnCollisionStay(Collision collision) {
        if (slimeJar && !grounded) {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y/3, rb.velocity.z);
        }
        if (collision.gameObject.CompareTag("Ladder")) {
            Vector2 inputMove = playerControls.Main.Move.ReadValue<Vector2>();
            horizontalInput = inputMove.x;
            verticalInput = inputMove.y;
            if (verticalInput > 0) {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + 0.7f, rb.velocity.z);
            }
        }
    }

    


}