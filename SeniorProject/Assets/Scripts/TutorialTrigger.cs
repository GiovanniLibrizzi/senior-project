using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTrigger : MonoBehaviour {

    public Image image;
    public Sprite sprite;

    void Start() {
        image.gameObject.SetActive(false);
        GetComponent<Renderer>().enabled = false;
    }

    void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            image.GetComponent<Image>().sprite = sprite;
            image.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            image.gameObject.SetActive(false);
        }
    }
}
