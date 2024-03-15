using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour {

    [field: Header("SFX")]
    [field: SerializeField] public EventReference jumpSfx { get; private set; }
    [field: SerializeField] public EventReference stepSfx { get; private set; }



    [field: Header("Music")]
    [field: SerializeField] public EventReference musicTest { get; private set; }


    public static FMODEvents instance {  get; private set; }

    void Awake() {
        if (instance != null) {
            Debug.LogError("More than one FMODEvents created");
        }
        instance = this;
    }


    void Start() {
        
    }

    void Update() {
        
    }
}
