using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputListener : MonoBehaviour {
   public bool EnableAnyButtonCheck = false;
    void Start(){}

    void Update(){
        if (EnableAnyButtonCheck) CheckAnyButton();        
    }

    void CheckAnyButton(){
        if (Input.anyKey){
            System.Array values = System.Enum.GetValues(typeof(KeyCode));
            foreach(KeyCode code in values){
                if(Input.GetKeyDown(code)){ 
                    Debug.Log("KeyInputFound: " + System.Enum.GetName(typeof(KeyCode), code)); 
                }                    
            }        
        }        
    }

}