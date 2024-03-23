using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AudioTrigger;

public class LevelTrigger : MonoBehaviour {

    public string sceneName;
    private void Awake() {
        GetComponent<Renderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            SceneManager.LoadScene(sceneName);
        }
    }
}
