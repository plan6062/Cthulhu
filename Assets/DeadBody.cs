using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crest;
public class DeadBody : Actor
{
    private GameObject player;
    bool isSink = false;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Raft");
        if(player == null){
            Debug.Log("no player");
        }

        // StartCoroutine(test());
    }

    // Update is called once per frame
    void Update()
    {
        if(MainTimeManager.Instance.GetCurrentStage() == Stage.Opening_Corpse1)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if(distance < 7){
                MainTimeManager.Instance.SetStage(Stage.Opening_Corpse2,  this.GetType().Name);
                StartCoroutine(Sink());
            }
        }
        if(isSink)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.y -= 0.1f * Time.deltaTime;
            transform.position = currentPosition;
        }
        if(transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }

    // IEnumerator test(){
    //     while(true){
    //         yield return new WaitForSeconds(2);
    //         float distance = Vector3.Distance(player.transform.position, transform.position);
    //     }
    // }
    IEnumerator Sink()
    {   
        yield return new WaitForSeconds(5);
        isSink = true;
        GetComponent<SimpleFloatingObject>().enabled = false;
    }
}

