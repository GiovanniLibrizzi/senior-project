using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Jar : MonoBehaviour {
    public enum JType {
        Feather,
        Fire
    }

    public enum JState {
        Grounded,
        Held,
        Thrown
    }

    private Vector3 position;
    protected Rigidbody rb;
    protected CapsuleCollider cCollider;
    protected PlayerMovement player = null;
    protected Transform orientation;
    public JState state;

    protected JType type;


    private float throwSpeed = 15f;

    void Start() {
        state = JState.Grounded;

    }

    void Update() {
        if (state == JState.Held) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if (position != null) {
                //transform.position = position;
            }
            // Update rotation to be parent rotation (fixes throw direction)
            transform.rotation = transform.parent.rotation;
        }
    }



    public virtual void Add() {
        Debug.Log("Add (Jar Class)");
        // State update
        state = JState.Held;

        // Physics set
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //transform.rotation = p
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        cCollider.isTrigger = true;
        orientation = GetComponentInParent<Transform>();
    }

    public virtual void Throw() {
        Debug.Log("Throw (Jar Class)");
        // State update
        state = JState.Thrown;

        // Physics set
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        cCollider.isTrigger = false;
        transform.parent = null;

        // Rotation shooting
        orientation = GetComponentInParent<Transform>();
        rb.velocity = orientation.forward * throwSpeed;


    }

    public virtual void Process() {

    }

    public void SetPosition(Vector3 pos) {
        position = pos;
    }

    public static explicit operator Jar(GameObject v) {
        throw new System.NotImplementedException();
    }

    public JType GetJType() { return type; }
    public JState GetState() {
        return state;
    }

    private IEnumerator ShatterJar() {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if (state == JState.Thrown) {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                StartCoroutine(ShatterJar());
            } 
        }
    }
}
