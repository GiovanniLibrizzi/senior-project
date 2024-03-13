using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioTrigger : MonoBehaviour {

    public enum Parameter {
        FullLoop,
        IntroLoop
    }
    [SerializeField] Parameter parameter;


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("audiotrigger player");
            AudioManager.instance.UpdateParameter(parameter, 1);
        }
    }


}
