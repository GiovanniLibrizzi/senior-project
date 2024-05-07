using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Key : Jar {

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
        switch (collision.gameObject.tag) {
            case "Door":
                collision.gameObject.GetComponent<Door>().Open();
                break;
        }
    }


}
