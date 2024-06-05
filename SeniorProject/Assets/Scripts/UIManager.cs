using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField] TextMeshProUGUI textHealth;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite heartFull;
    [SerializeField] Sprite heartEmpty;
    void Start() {
        textHealth.text = "Health: 5";
        PlayerMovement.OnPlayerHit += UpdateTextHealth;
    }

    private void OnDestroy() {
        PlayerMovement.OnPlayerHit -= UpdateTextHealth;
    }

    public void UpdateTextHealth(int hp) {
        for (int i = 4; i >= hp; i--) {
            hearts[i].sprite = heartEmpty;
        }
        textHealth.text = "Health: " + hp;
    }
}
