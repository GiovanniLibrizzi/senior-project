using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioTrigger : MonoBehaviour {

    public enum Parameter {
        FullLoop,
        IntroLoop,
        Reverb,
        NoReverb
    }
    [SerializeField] Parameter parameter;

    private void Awake() {
        GetComponent<Renderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("audiotrigger player");
            AudioManager.instance.UpdateParameter(parameter, 1);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            // Check if player exits a reverb zone
            if (parameter == Parameter.Reverb) {
                AudioManager.instance.UpdateParameter(Parameter.NoReverb, 0);
            }
        }
    }


}
