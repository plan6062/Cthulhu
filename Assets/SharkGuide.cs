using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkGuide : MonoBehaviour
{
    public Transform shark;

    // Update is called once per frame
    void Update()
    {
        if(checker){
            transform.LookAt(shark);
        }
        
    }


    bool checker = true;
    public void setTrue(){
        if(checker == false){
            checker = true;
        } else {
            // Debug.Log("setTrue Error");
        }
    }

    public void setFalse(){
        if(checker == true){
            checker = false;
        } else {
            Debug.Log("setFales Error");
        }
    }
}
