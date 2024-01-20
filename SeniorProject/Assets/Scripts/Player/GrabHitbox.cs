using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHitbox : MonoBehaviour {

    public PlayerGrab player;
    public GameObject playerObj;
    void Start() {

    }

    void Update() {
        
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log("touching jar");
        if (other.gameObject.CompareTag("jar")) {
            float jarCount = player.jars.Count;

            other.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f * jarCount, transform.position.z);
            other.gameObject.transform.parent = playerObj.transform;
            player.jars.Add(other.gameObject);
        }
    }


}
