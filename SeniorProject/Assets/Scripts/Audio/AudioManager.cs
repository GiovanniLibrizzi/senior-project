using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour {

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    [SerializeField] private Transform playerPos;

    private EventInstance musicEventInstance;


    public static AudioManager instance { get; private set; }

    void Awake() {
        if (instance != null) {
            Debug.LogError("More than one AudioManager created");
        }
        instance = this;


        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }

    private void Start() {
        InitializeMusic(FMODEvents.instance.musicTest);
    }

    private void InitializeMusic(EventReference musicEventReference) {
        musicEventInstance = CreateInstance(musicEventReference);
        //musicEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(playerPos));
        musicEventInstance.start();
    } 

    public void SetMusicJars(int jarsAmt) {
        musicEventInstance.setParameterByName("Jars", (int)jarsAmt);
    }


    public void PlayOneShot(EventReference sound, Vector3 worldPos) {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }


    public EventInstance CreateInstance(EventReference eventReference) {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject) {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;

    }

    private void CleanUp() {
        foreach (EventInstance e in eventInstances) {
            e.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            e.release();
        }
    }
}
