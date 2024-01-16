using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance { get; private set; }

    void Awake() {
        if (instance != null) {
            Debug.LogError("More than one AudioManager created");
        }
        instance = this;
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos) {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
}
