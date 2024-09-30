using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Reef : Actor
{
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Raft");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if(distance < 10){
                MainTimeManager.Instance.SetStage(Stage.Stage1_GetClosetoReef,this.GetType().Name);
            }
    }
}
