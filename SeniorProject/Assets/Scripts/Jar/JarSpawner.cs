using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;
using static UnityEngine.Rendering.DebugUI;

public class JarSpawner : MonoBehaviour {

    [SerializeField] Jar.JType type;
    [SerializeField] GameObject jarPrefab;
    [SerializeField] GameObject jarGroup = null;
    [SerializeField] float spawnTime = 8;
    [SerializeField] int jarHeldAmt;
    [SerializeField] float distanceRadius = 10;
    //[SerializeField] GameObject colliderCheck = null;
    Vector3 inGroundPos = Vector3.zero;
    Jar newJar = null;

    bool canSpawnJar = true;
    float groundSink = 0.45f;

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
        yield return new WaitForSeconds(0.1f);


        Vector3 inGroundPos = new Vector3(transform.position.x, transform.position.y-groundSink, transform.position.z);
        newJar = Instantiate(jarPrefab, inGroundPos, Quaternion.identity).GetComponent<Jar>();
        //newJar.transform.position = transform.position;
        StartCoroutine(JarEmerge(inGroundPos.y+groundSink));
        yield return new WaitForSeconds(spawnTime);
        canSpawnJar = true;
    }

    // Slowly move jar up
    IEnumerator JarEmerge(float yPos) {
        while (true) {
            newJar.transform.Translate(new Vector3(0, 0.01f*(Time.deltaTime*60), 0));
            if (newJar.transform.position.y >= yPos) {
                yield break;
            }

            yield return null;
        }
    }
}
