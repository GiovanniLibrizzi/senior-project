using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using System;


public class PlayerGrab : MonoBehaviour {

    PlayerControls playerControls;
    public GameObject hitboxGrab;
    public float hitboxTime;
    private float timer;
    private bool timerActive;

    public Stack<Jar> jars = new Stack<Jar>();
    public Dictionary<Jar.JType, int> jarDict = new Dictionary<Jar.JType, int>();


    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
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

        jar.Add();
        
    }

    public Jar RemoveJar() {
        // list
        Jar jar = jars.Pop();
        
        // dict
        Jar.JType type = jar.GetJType();
        if (jarDict.ContainsKey(type)) {
            int amt = jarDict[type];
            jarDict[type] = amt - 1;
        }
        return jar;
    }

    public int GetJarCount(Jar.JType jarType) {
        int amt = 0;
        if (jarDict.ContainsKey(jarType)) {
            amt = jarDict[jarType];
        }

        return amt;
    }

}
