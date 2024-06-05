using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Key : Jar {

    [SerializeField] GameObject particleBreak;
    void Awake() {
        type = JType.Key;
        rb = GetComponent<Rigidbody>();
        cCollider = GetComponent<CapsuleCollider>();
    }

    public override void Add() {
        base.Add();

        Debug.Log("Key Add");
    }

    public override void Throw() {
        base.Throw();
    }

    protected override void OnShatterCollision(Collision collision) {
        Instantiate(particleBreak, collision.contacts[0].point, Quaternion.identity);
        switch (collision.gameObject.tag) {
            case "Door":
                collision.gameObject.GetComponent<Door>().Open();
                break;
        }
    }


}
