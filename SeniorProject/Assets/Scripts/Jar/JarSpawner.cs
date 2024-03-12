using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarSpawner : MonoBehaviour {

    [SerializeField] Jar.JType type;
    [SerializeField] GameObject jarPrefab;
    [SerializeField] GameObject jarGroup = null;
    [SerializeField] float spawnTime = 8;
    [SerializeField] int jarHeldAmt;
    [SerializeField] float distanceRadius = 10;
    //[SerializeField] GameObject colliderCheck = null;

    Jar newJar = null;

    bool canSpawnJar = true;

    PlayerGrab playerJars;
    void Start() {
        GetComponent<Renderer>().enabled = false;
        playerJars = PlayerGrab.instance;
        if (jarGroup == null) {
            jarGroup = gameObject.transform.parent.gameObject;
        }
    }

    void Update() {
        if (canSpawnJar && playerJars.GetJarCount(type) <= jarHeldAmt && PlayerInRadius()) {
            if (newJar == null || newJar.state == Jar.JState.Thrown) { 
                StartCoroutine(SpawnJar());
            }
        }
    }

    private bool PlayerInRadius() {
        Vector3 pDistance = playerJars.gameObject.transform.position;
        return Vector3.Distance(transform.position, pDistance) < distanceRadius;
    }

    IEnumerator SpawnJar() {
        canSpawnJar = false;
        yield return new WaitForSeconds(1.5f);

        newJar = Instantiate(jarPrefab, jarGroup.transform).GetComponent<Jar>();
        newJar.transform.position = transform.position;

        yield return new WaitForSeconds(spawnTime);
        canSpawnJar = true;
    }
}
