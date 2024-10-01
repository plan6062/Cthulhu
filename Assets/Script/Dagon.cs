using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dagon : Actor
{
    Stage currentStage;
    Transform player;
    Transform raft;
    Quaternion headRotation;
    Animator anim;
    LightningController lightningController;
    public AudioSource audioSource_growl;

    private SkinnedMeshRenderer thismesh;

    void Start() {
        currentStage = MainTimeManager.Instance.GetCurrentStage();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        raft = GameObject.FindGameObjectWithTag("Raft").transform;
        lightningController = GameObject.FindGameObjectWithTag("lightning").GetComponent<LightningController>();
        StartCoroutine(DagonCloser(10));
        // StartCoroutine(Next1(5));
    }
  
    IEnumerator DagonCloser(int distance){
        while (true)
        {
            if(EyeTracker.Instance.CheckSightCollison(player.gameObject,"Sightcollision2"))
            {
                audioSource_growl.Play();
                yield return new WaitForSeconds(1);
                // thismesh.enabled = true;
                EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation); // 플레이어 시선 각도 빼오기
                Vector3 DagonDirection = headRotation * Vector3.forward*-1;
                DagonDirection.y = 0;
                DagonDirection = DagonDirection.normalized * distance;
                transform.position = raft.position + DagonDirection;
                transform.LookAt(player.transform.position);
                StartCoroutine(lightningController.LightningImmediate(1));
                StartCoroutine(makeSelfTrans(2));
                yield return new WaitForSeconds(5);
                if(currentStage==Stage.Stage2_DagonFind){
                    MainTimeManager.Instance.SetStage(Stage.Stage2_DagonOceanClose1,this.GetType().Name);
                } else if(currentStage ==Stage.Stage2_DagonOceanClose1) {
                    MainTimeManager.Instance.SetStage(Stage.Stage2_DagonOceanClose2,this.GetType().Name);
                } else if(currentStage ==Stage.Stage2_DagonOceanClose2) {
                    MainTimeManager.Instance.SetStage(Stage.Stage2_DagonOnBoat,this.GetType().Name);
                }
                
                break;
            } else {
                yield return null;
            }   
        }
    }

    IEnumerator makeSelfTrans(int sec)
    {
        yield return new WaitForSeconds(sec);
        // thismesh.enabled = false;
    }
    IEnumerator Next1(int sec)
    {
        yield return new WaitForSeconds(sec);
        MainTimeManager.Instance.SetStage(Stage.Stage2_DagonOceanClose1,this.GetType().Name);
    }
    IEnumerator Next2(int sec)
    {
        yield return new WaitForSeconds(sec);
        MainTimeManager.Instance.SetStage(Stage.Stage2_DagonOceanClose2,this.GetType().Name);
    }
    IEnumerator Next3(int sec)
    {
        yield return new WaitForSeconds(sec);
        MainTimeManager.Instance.SetStage(Stage.Stage2_DagonOnBoat,this.GetType().Name);
    }
    protected override void Acting(Stage newStage)
    {
        currentStage = newStage;
        switch (newStage)
        {
            case Stage.Stage2_DagonFind:
                
                break;
            case Stage.Stage2_DagonOceanClose1:
                StartCoroutine(DagonCloser(7));
                break;
            case Stage.Stage2_DagonOceanClose2:
                StartCoroutine(DagonCloser(4));
                break;
            case Stage.Stage2_DagonOnBoat:
                break;
            case Stage.Stage2_DagonGetClose:
                break;
            case Stage.Stage2_DagonDeathScene:
                break;
            case Stage.Stage2_DagonEnd:
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
