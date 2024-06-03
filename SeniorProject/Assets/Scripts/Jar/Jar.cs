using System.Collections;
using UnityEngine;

public class Jar : MonoBehaviour {
    public enum JType {
        Feather,
        Fire, 
        Slime,
        Key
    }

    public enum JState {
        Grounded,
        Held,
        Thrown
    }

    private Vector3 position;
    protected Vector3 initialVelocity;
    protected Rigidbody rb;
    protected CapsuleCollider cCollider;
    protected PlayerMovement player = null;
    protected Transform orientation;
    public JState state;

    protected JType type;

    protected int attack = 1;
    private float throwSpeed = 15f;
    protected bool shattered = false;

    void Start() {
        state = JState.Grounded;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        cCollider.isTrigger = true;
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


        initialVelocity = orientation.forward * throwSpeed;
        rb.velocity = initialVelocity;

        AudioManager.instance.PlayOneShot(FMODEvents.instance.jarThrowSfx, transform.position);

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

    protected virtual void OnShatterCollision(Collision collision) {

    }

    private IEnumerator ShatterJar() {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if (state == JState.Thrown && collision.gameObject.tag != "Player") {
            //if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            StartCoroutine(ShatterJar());
            OnShatterCollision(collision);

            if (!shattered) {
                switch (type) {
                    case JType.Feather:
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.featherShatterSfx, transform.position);
                        break;
                    case JType.Fire:
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.fireShatterSfx, transform.position);
                        break;
                    case JType.Slime:
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.slimeShatterSfx, transform.position);
                        break;
                    case JType.Key:
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.keyShatterSfx, transform.position);
                        break;
                    default:
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.shatterSfx, transform.position);
                        break;
                }
            }
            shattered = true;
        }
    }
}
