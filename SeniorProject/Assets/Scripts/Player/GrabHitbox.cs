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
        //Debug.Log("touching jar");
        if (other.gameObject.CompareTag("jar")) {

            Jar jar = other.gameObject.GetComponent<Jar>();
            if (jar.GetState() == Jar.JState.Grounded) {
                // Set parent to the player object so it sticks to them            
                other.gameObject.transform.parent = playerObj.transform;
                // Reposition jar
                Vector3 jarPos = new Vector3(transform.position.x, transform.position.y + 0.3f * player.GetJarCount(), transform.position.z);
                other.gameObject.transform.position = jarPos;
                Debug.Log(jarPos);
                jar.SetPosition(jarPos);
                player.AddJar(other.gameObject);
            }
        }
    }


}
