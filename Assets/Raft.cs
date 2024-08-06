using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Raft : Actor
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z > 20){
            MainTimeManager.Instance.SetStage(Stage.Opening_Corpse1);
        }
    }
}
