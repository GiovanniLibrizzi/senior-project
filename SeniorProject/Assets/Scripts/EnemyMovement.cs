using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    
    public enum EnemyState {
        Idle,
        Roam,
        Attacking,
        Hurt
    }

    // health
    [SerializeField] int maxHp = 2;
    int hp;

    // other variables
    EnemyState state = 0;
    EnemyState prevState = 0;
    Rigidbody rb;
    [SerializeField] float mspd = 3f;
    Vector3 roamVelocity = Vector3.zero;
    //bool hurtHitWall = false;

    float hurtTime = 1.2f;

    [SerializeField] GameObject player;
    [SerializeField] int attack;
    [SerializeField] int roamRange = 15;
    [SerializeField] int attackRange = 15;

    public static event Action<int> OnEnemyHitsPlayer;

    void Start() {
        hp = maxHp;
        rb = GetComponent<Rigidbody>();
        SetState(EnemyState.Idle);
    }

    void Update() {

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log(distanceToPlayer);

        switch (state) { 
            case EnemyState.Idle:

                // Transition
                if (distanceToPlayer < roamRange) {
                    TransitionRoam();
                }
                break;
            case EnemyState.Roam:
                rb.velocity = transform.forward * mspd;//roamVelocity;

                // Transition
                if (distanceToPlayer < attackRange) {
                    SetState(EnemyState.Attacking);
                }

                if (distanceToPlayer > roamRange) {
                    SetState(EnemyState.Idle);
                }
                
                break;

            case EnemyState.Attacking:
                transform.LookAt(player.transform.position);
                rb.velocity = transform.forward * mspd * 1.25f;

                // Transition
                if (distanceToPlayer > roamRange) {
                    TransitionRoam();
                }
                break;

            case EnemyState.Hurt:
                StartCoroutine(HurtCooldown());

                //if (hurtHitWall) {
                //    hurtHitWall = false;
                //    state = EnemyState.Attacking;
                //}
                break;
        }
    
    }

    private void SetState(EnemyState state) {
        prevState = this.state;
        this.state = state;
        AudioManager.instance.SetEnemyState(state, prevState);
    }

    private void TransitionRoam() {
        Debug.Log("Roam starting");
        StartCoroutine(RoamMovement());
        SetState(EnemyState.Roam);
    }

    public void TransitionHit() {
        Debug.Log("Enemy hit!!");
        SetState(EnemyState.Hurt);
    }   


    IEnumerator RoamMovement() {
        roamVelocity = mspd * new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));

        transform.LookAt(roamVelocity + transform.position);

        yield return new WaitForSeconds(3);

        StartCoroutine(RoamMovement());
    }

    IEnumerator HurtCooldown() {
        yield return new WaitForSeconds(hurtTime);

        if (state == EnemyState.Hurt) {
            SetState(EnemyState.Attacking);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Jar")) {

        }
    }
    private void OnCollisionStay(Collision collision) {
        if (state == EnemyState.Hurt) {
            //if (!collision.gameObject.CompareTag("Player")) {
                //hurtHitWall = true;
            //}
        } else {
            if (collision.gameObject.CompareTag("Player")) {
                OnEnemyHitsPlayer?.Invoke(attack);
            }
        }
    }

    public void TakeDamage(int amt) {
        hp -= amt;
        TransitionHit();
        Debug.Log("Enemy damage! Health: " + hp);
        if (hp <= 0) {
            DestroyAudioState();
            Destroy(gameObject);
        }
    }


    private void DestroyAudioState() {
   
        AudioManager.instance.DeleteEnemyState(EnemyState.Attacking);
    }

}
