using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeJar : Jar {


    public static event Action<int> OnSlimeJarInteract;
    private PlayerGrab grab = null;


    void Start() {
        type = JType.Slime;
        rb = GetComponent<Rigidbody>();
        cCollider = GetComponent<CapsuleCollider>();
    }

   

    public override void Add() {
        base.Add();
        //rb.isKinematic = true;
       
        Debug.Log("SlimeJar Add");
        if (grab == null) {
            grab = GetComponentInParent<PlayerGrab>();
        }
        OnSlimeJarInteract?.Invoke(grab.GetJarCount(JType.Slime));
        //if (player == null) {
        //    player = GetComponentInParent<PlayerMovement>();
        //}
        //player.UpdateMoveSpeed(speedIncrease);
        //player.UpdateJumpForce(jumpIncrease);

    }

    public override void Throw() {
        base.Throw();

        OnSlimeJarInteract?.Invoke(grab.GetJarCount(JType.Slime));

        //player.UpdateMoveSpeed(-speedIncrease);
        //player.UpdateJumpForce(-jumpIncrease);


    }

    public override void Process() {

    }

    protected override void OnShatterCollision(Collision collision) {
        //if (collision.gameObject.CompareTag("Movable")) {
           
        //}
    }
}
