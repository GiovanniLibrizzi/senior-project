using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;
using System.Runtime.InteropServices;

public class AudioManager : MonoBehaviour {

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    [SerializeField] private Transform playerPos;

    private EventInstance musicEventInstance;

    // Enemy state
    public enum DangerState {
        Chill,
        Wary,
        Danger
    }
    private DangerState dangerState = DangerState.Chill;
    private int[] enemyStates = new int[3];

    // Timeline
    public TimelineInfo timelineInfo = null;
    private GCHandle timelineHandle;

    public static AudioManager instance { get; private set; }

    private FMOD.Studio.EVENT_CALLBACK beatCallback;

    // beat tracking
    public static int lastBeat = 0;
    public static string lastMarkerString = null;

    public delegate void BeatEventDelegate();
    public static event BeatEventDelegate beatUpdated;

    public delegate void MarkerListenerDelegate();
    public static event MarkerListenerDelegate markerUpdated;


    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo {
        public int currentBeat = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }


    void Awake() {
        if (instance != null) {
            Debug.LogError("More than one AudioManager created");
        }
        instance = this;


        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        InitializeMusic(FMODEvents.instance.musicTest);

    }

    private void Start() {
        if (instance != null) {
            timelineInfo = new TimelineInfo();
            beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
            timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned); // ignores garbage collection
            eventInstances[0].setUserData(GCHandle.ToIntPtr(timelineHandle));
            eventInstances[0].setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        }
    }

    private void Update() {
        if (lastMarkerString != timelineInfo.lastMarker) {
            lastMarkerString = timelineInfo.lastMarker;

            if (markerUpdated != null) {
                markerUpdated();
            }
        }

        if (lastBeat != timelineInfo.currentBeat) {
            lastBeat = timelineInfo.currentBeat;
            if (beatUpdated != null) {
                beatUpdated();
            }
        }
    }




    private void OnDestroy() {
        CleanUp();
    }


    private void InitializeMusic(EventReference musicEventReference) {
        musicEventInstance = CreateInstance(musicEventReference);
        //musicEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(playerPos));
        musicEventInstance.start();
    } 

    public void SetMusicJars(int jarsAmt) {
        StartCoroutine(ParameterWaitForBeat("Jars", jarsAmt));
        //
    }

    IEnumerator ParameterWaitForBeat(string parameterName, float value) {
        while (true) {
            if (lastBeat != timelineInfo.currentBeat) {
                musicEventInstance.setParameterByName(parameterName, (int)value);
                Debug.Log("Beat changed, update jar parameter in FMOD");
                yield break;
            }

            yield return null;
        }
    }

    

    public void SetEnemyState(EnemyMovement.EnemyState state, EnemyMovement.EnemyState previousState = EnemyMovement.EnemyState.Hurt) {
        if (state != EnemyMovement.EnemyState.Hurt && previousState != EnemyMovement.EnemyState.Hurt) {
            enemyStates[(int)state]++;
            enemyStates[(int)previousState]--;
        }

        UpdateDangerState();
    }

    public void UpdateDangerState() {
        if (enemyStates[(int)EnemyMovement.EnemyState.Roam] == 0 && enemyStates[(int)EnemyMovement.EnemyState.Attacking] == 0) {
            dangerState = DangerState.Chill;
        } else if (enemyStates[(int)EnemyMovement.EnemyState.Roam] > 0 && enemyStates[(int)EnemyMovement.EnemyState.Attacking] == 0) {
            dangerState = DangerState.Wary;
        } else if (enemyStates[(int)EnemyMovement.EnemyState.Attacking] > 0) {
            Debug.Log(enemyStates[(int)EnemyMovement.EnemyState.Attacking]);
            dangerState = DangerState.Danger;
        } else {
            dangerState = DangerState.Chill;
        }

        Debug.Log("AUDIO: DangerState " + dangerState.ToString());

        musicEventInstance.setParameterByName("DangerState", (int)dangerState);

    }

    public void DeleteEnemyState(EnemyMovement.EnemyState state) {
        enemyStates[(int)state]--;
        UpdateDangerState();
        //Debug.Log("dangerstates: " + enemyStates[0] + enemyStates[1] + enemyStates[2]);
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

    // Timeline stuff
    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr) {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

        if (result != FMOD.RESULT.OK) {
            Debug.LogError("Timeline Callback error: " + result);
        } else if (timelineInfoPtr != IntPtr.Zero) {
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type) {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT: {
                    var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                    // parameter.beat, bar, tempo...
                    timelineInfo.currentBeat = parameter.beat;
                }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER: {
                    var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                    // also parameter.position
                    timelineInfo.lastMarker = parameter.name;
                }
                    break;
            }
        }

        return FMOD.RESULT.OK;
    }
#if UNITY_EDITOR // || DEVELOPMENT_BUILD
    private void OnGUI() {
        GUILayout.Box($"Current Beat = {timelineInfo.currentBeat}, Last Marker = {(string)timelineInfo.lastMarker}");
    }
#endif


    private void CleanUp() {
        eventInstances[0].setUserData(IntPtr.Zero);

        foreach (EventInstance e in eventInstances) {
            e.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            e.release();
        }

        timelineHandle.Free();
    }
}
