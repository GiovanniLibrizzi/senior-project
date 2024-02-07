using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireJar : Jar {


    public static event Action<int> OnFireJarInteract;
    private PlayerGrab grab = null;

    void Start() {
        type = JType.Fire;
        rb = GetComponent<Rigidbody>();
        cCollider = GetComponent<CapsuleCollider>();
    }

   

    public override void Add() {
        base.Add();
        //rb.isKinematic = true;
        if (grab == null) {
            grab = GetComponentInParent<PlayerGrab>();
        }
        OnFireJarInteract?.Invoke(grab.GetJarCount(JType.Fire));
        Debug.Log("FireJar Add");

    }

    public override void Throw() {
        base.Throw();
        OnFireJarInteract?.Invoke(grab.GetJarCount(JType.Fire));

    }

    public override void Process() {

    }

    protected override void OnShatterCollision(Collision collision) {
        // Check if colliding w exploding
        if (collision.gameObject.CompareTag("Explodable")) {
            Debug.Log("Explode wall");
            Destroy(collision.gameObject);
        }

    }
}
