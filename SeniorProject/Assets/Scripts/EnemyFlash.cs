using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlash : MonoBehaviour {

    Material[] materials;
    
    void Start() {
        //materials = GetComponents<Material>();
        materials = GetComponent<Renderer>().materials;
    }
    private float flashSpeed = 0.15f;
    public bool currentlyHit = false;
    private float minBrightness = 0.74f;
    private float maxBrightness = 2.4f;
    private float veryBright = 5f;

    void Update() {
       

    }

    public void EnemyHit(bool hit) {
        currentlyHit = hit;
        if (hit) {
            StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash() {
        while (currentlyHit) {
            foreach (Material mat in materials) {
                mat.SetFloat("_MinBrightness", veryBright);
                mat.SetFloat("_MaxBrightness", veryBright);
            }
            yield return new WaitForSeconds(flashSpeed);
            foreach (Material mat in materials) {
                mat.SetFloat("_MinBrightness", minBrightness);
                mat.SetFloat("_MaxBrightness", maxBrightness);
            }
            yield return new WaitForSeconds(flashSpeed);
        }
    }

}
