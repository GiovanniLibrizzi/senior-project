using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    public Slider sliderMusic;
    public Slider sliderSFX;
    public Slider sliderSoundscape;
    void Start() {
        
    }

    void Update() {
        
    }

    public void UpdateSliderMusic() {
        AudioManager.instance.SetMusicVolume(sliderMusic.value);
    }
    public void UpdateSliderSfx() {
        AudioManager.instance.SetSfxVolume(sliderSFX.value);
    }
    public void UpdateSliderSoundscape() {
        AudioManager.instance.SetSoundscapeVolume(sliderSoundscape.value);
    }
}
