using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reef : Actor
{
    private GameObject player;
    bool done = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Raft");
    }

    // Update is called once per frame
    void Update()
    {
        if(!done){
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if(distance < 10)
            {    
                done = true;
                StartCoroutine(WaitfoSec(5));
            }
        }
        
    }

    IEnumerator WaitfoSec(int n){
        MainTimeManager.Instance.SetStage(Stage.Stage1_GetClosetoReef,this.GetType().Name);
        yield return new WaitForSeconds(n);
        MainTimeManager.Instance.SetStage(Stage.Stage1_LookThroughHole,this.GetType().Name);       
    }
}
