using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherJar : Jar {

    
    [SerializeField] float speedIncrease;
    [SerializeField] float jumpIncrease;
    
    void Start() {
        type = JType.Feather;
        rb = GetComponent<Rigidbody>();
        cCollider = GetComponent<CapsuleCollider>();
    }

   

    public override void Add() {
        base.Add();
        //rb.isKinematic = true;
       
        Debug.Log("Feather Add");
        if (player == null) {
            player = GetComponentInParent<PlayerMovement>();
        }
        player.UpdateMoveSpeed(speedIncrease);
        player.UpdateJumpForce(jumpIncrease);

    }

    public override void Throw() {
        base.Throw();
        player.UpdateMoveSpeed(-speedIncrease);
        player.UpdateJumpForce(-jumpIncrease);


    }

    //public override void Process() {

    //}

    protected override void OnShatterCollision(Collision collision) {
        switch (collision.gameObject.tag) {
            case "Movable":
                ApplyKnockback(collision);
                break;
            case "Enemy":
                ApplyKnockback(collision);
                EnemyMovement enemy = collision.gameObject.GetComponent<EnemyMovement>();
                enemy.TakeDamage(attack);
                break;
        }



        //if (collision.gameObject.CompareTag("Movable")) {
        //    Rigidbody rbOther = collision.gameObject.GetComponent<Rigidbody>();
        //    rbOther.velocity = initialVelocity.normalized * 16f; //1.1f; 
        //    //rbOther.AddForce(initialVelocity.normalized * rb.velocity.magnitude, ForceMode.Impulse);
        //}
    }

    private void ApplyKnockback(Collision collision) {
        Rigidbody rbOther = collision.gameObject.GetComponent<Rigidbody>();
        rbOther.velocity = initialVelocity.normalized * 16f; //1.1f; 
    }
}
