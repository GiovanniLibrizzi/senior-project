using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using System;
using Unity.VisualScripting;


public class PlayerGrab : MonoBehaviour {

    PlayerControls playerControls;
    public GameObject hitboxGrab;
    public float hitboxTime;
    private float timer;
    private bool timerActive;

    public Stack<Jar> jars = new Stack<Jar>();
    public Dictionary<Jar.JType, int> jarDict = new Dictionary<Jar.JType, int>();

    private Animator animator;

    private void OnEnable() {
        playerControls.Enable();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnDisable() {
        playerControls.Disable();
    }



    public static PlayerGrab instance { get; private set; }
    private void Awake() {
        playerControls = new PlayerControls();

        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this) {
            Destroy(this);
        } else {
            instance = this;
        }
    }

    void Start() {

    }




    void Update() {
        bool grabButton = playerControls.Main.Grab.triggered;

        if (grabButton) {
            hitboxGrab.SetActive(true);
            timer = hitboxTime;
            timerActive = true;
        }


        // Hitbox deactivate timer
        if (timerActive) {
            if (timer > 0) {
                timer -= Time.deltaTime;
            } else {
                timerActive = false;
                hitboxGrab.SetActive(false);
            }
        }

        for (int i = 0; i < GetJarCount(); i++) {

        }

    }

    public int GetJarCount() {
        return jars.Count;
    }
    public void AddJar(GameObject obj) {

        // list
        Jar jar = obj.GetComponent<Jar>();
        jars.Push(jar);

        // dict
        Jar.JType type = jar.GetJType();
        if (jarDict.ContainsKey(type)) {
            int amt = jarDict[type];
            jarDict[type] = amt + 1;
        } else {
            jarDict.Add(type, 1);
        }

        AudioManager.instance.SetMusicJars(GetJarCount());
        AudioManager.instance.SetMusicJars(type, GetJarCount(type));
        AudioManager.instance.PlayOneShot(FMODEvents.instance.jarPickupSfx, transform.position);

        jar.Add();

        animator.SetBool("isHolding", true);

    }

    public Jar RemoveJar() {
        if (GetJarCount() > 0) {
            // list
            Jar jar = jars.Pop();

            // dict
            Jar.JType type = jar.GetJType();
            if (jarDict.ContainsKey(type)) {
                int amt = jarDict[type];
                jarDict[type] = amt - 1;
            }

            AudioManager.instance.SetMusicJars(GetJarCount());
            AudioManager.instance.SetMusicJars(type, GetJarCount(type));

            if (GetJarCount() == 0) {
                animator.SetBool("isHolding", false);
            }

            return jar;
        }
        return null;
    }

    public int GetJarCount(Jar.JType jarType) {
        int amt = 0;
        if (jarDict.ContainsKey(jarType)) {
            amt = jarDict[jarType];
        }

        return amt;
    }

}
