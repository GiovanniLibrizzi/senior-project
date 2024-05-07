using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    float initialAngle;
    void Start() {
        initialAngle = transform.localRotation.eulerAngles.y;
    }

    public void Open() {
        StartCoroutine(OpenDoor());
        Debug.Log("Door opening...");
    }
    
    IEnumerator OpenDoor() {       
        while (true) {
            transform.Rotate(0, 3f*Time.fixedDeltaTime, 0);
            if (transform.localRotation.eulerAngles.y > initialAngle + 90f) {
                //Debug.Log("Reached 90, break;");
                yield break;
            }
            yield return null;
        }
    }
}
