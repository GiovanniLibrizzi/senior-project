using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireJar : Jar {


    public static event Action<int> OnFireJarInteract;
    private PlayerGrab grab = null;
    [SerializeField] GameObject particleExplode;

    void Awake() {
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
        Instantiate(particleExplode, collision.contacts[0].point, Quaternion.identity);
        // Check if colliding w exploding
        switch (collision.gameObject.tag) {
            case "Explodable":
                Debug.Log("Explode wall");
                Destroy(collision.gameObject);
                break;
            case "Enemy":
                EnemyMovement enemy = collision.gameObject.GetComponent<EnemyMovement>();
                enemy.TakeDamage(attack);
                break;
        }

    }
}
