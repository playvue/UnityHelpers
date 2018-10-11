using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    SceneMuteController
    -------------------
    Uses AudioListener to set the volume of the Scene
    and store it in PlayerPrefs for future reference

    Instructions
    ------------
    1. Add to scene.
    2. Assign muteIcon and unmuteIcon to button gameobjects that execute ToggleSound() on touch

 */
public class SceneMuteController : MonoBehaviour {
    public GameObject muteIcon;
    public GameObject unmuteIcon;

    void Start(){
        SetState();
    }

    public void ToggleSound() {
        if (PlayerPrefs.GetInt ("SceneMute", 0) == 0) {
            PlayerPrefs.SetInt ("SceneMute", 1);
        } else {
            PlayerPrefs.SetInt ("SceneMute", 0);
        }
        SetState ();
    }

    private void SetState(){
        bool isMuteOff = PlayerPrefs.GetInt ("SceneMute", 0) == 0;
        AudioListener.volume = isMuteOff ? 1 : 0;
        if (muteIcon) muteIcon.SetActive (isMuteOff);
        if (unmuteIcon) unmuteIcon.SetActive (!isMuteOff);
    }

}