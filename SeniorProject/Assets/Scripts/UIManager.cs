using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [SerializeField] TextMeshProUGUI textHealth;
    void Start() {
        textHealth.text = "Health: 5";
        PlayerMovement.OnPlayerHit += UpdateTextHealth;
    }

    public void UpdateTextHealth(int hp) {
        textHealth.text = "Health: " + hp;
    }




}
