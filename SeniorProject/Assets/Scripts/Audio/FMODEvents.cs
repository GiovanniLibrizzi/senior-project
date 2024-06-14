using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour {

    [field: Header("SFX")]
    [field: SerializeField] public EventReference jumpSfx { get; private set; }
    [field: SerializeField] public EventReference stepSfx { get; private set; }
    [field: SerializeField] public EventReference shatterSfx { get; private set; }
    [field: SerializeField] public EventReference featherShatterSfx { get; private set; }
    [field: SerializeField] public EventReference jarPickupSfx { get; private set; }
    [field: SerializeField] public EventReference jarThrowSfx { get; private set; }
    [field: SerializeField] public EventReference jarRespawnSfx { get; private set; }
    [field: SerializeField] public EventReference fireShatterSfx { get; private set; }
    [field: SerializeField] public EventReference slimeShatterSfx { get; private set; }
    [field: SerializeField] public EventReference keyShatterSfx { get; private set; }
    [field: SerializeField] public EventReference playerHitSfx { get; private set; }
    [field: SerializeField] public EventReference enemyHitSfx { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference musicTest { get; private set; }
    [field: SerializeField] public EventReference soundscape { get; private set; }


    public static FMODEvents instance {  get; private set; }

    void Awake() {
        if (instance != null) {
            //Debug.LogError("More than one FMODEvents created");
            Destroy(this);
        }
        instance = this;
    }


    void Start() {
        
    }

    void Update() {
        
    }
}
