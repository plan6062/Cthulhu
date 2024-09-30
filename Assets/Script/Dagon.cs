using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dagon : Actor
{
    Stage currentStage;
    
    public Transform player;
    Quaternion headRotation;
    public Animator anim;
    public GameObject transCollider;
    public LightningController lightningController;

    private SkinnedMeshRenderer thismesh;

    void Start() {
        
    }
    IEnumerator InitPosition()
    {   

        transCollider.SetActive(true);
        while (true)
        {
            if(EyeTracker.Instance.CheckSightCollison(player.gameObject,"sightcollision2")) // 토러스에 닿은 순간
            {
                thismesh.enabled = true;
                EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation); // 플레이어 시선 각도 빼오기
                Vector3 DagonDirection = headRotation * Vector3.forward*-1;
                DagonDirection.y = 0;
                DagonDirection = DagonDirection.normalized * 10.0f;
                transform.position = player.position + DagonDirection;
                transform.LookAt(player.transform.position);
                anim.SetBool("idle", true);
                transCollider.SetActive(false);
                StartCoroutine(lightningController.LightningImmediate(1));
                StartCoroutine(makeSelfTrans(2));
                MainTimeManager.Instance.SetStage(Stage.Stage2_DagonOceanClose1,  this.GetType().Name);
                break;
            } else {
                yield return null;
            }   
        }
    }

    IEnumerator DagonCloser(){
        
        transCollider.SetActive(true);
        while (true)
        {
            if(EyeTracker.Instance.CheckSightCollison(player.gameObject,"sightcollision2")) // 토러스에 닿은 순간
            {
                EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation); // 플레이어 시선 각도 빼오기
                Vector3 DagonDirection = headRotation * Vector3.forward*-1;
                DagonDirection.y = 0;
                DagonDirection = DagonDirection.normalized * 7.0f;
                transform.position = player.position + DagonDirection;
                transform.LookAt(player.transform.position);
                anim.SetBool("idle", true);
                transCollider.SetActive(false);
                StartCoroutine(lightningController.LightningImmediate(1));
                StartCoroutine(makeSelfTrans(2));
                MainTimeManager.Instance.SetStage(Stage.Stage2_DagonOceanClose1,  this.GetType().Name);
                break;
            } else {
                yield return null;
            }   
        }
    }

    IEnumerator makeSelfTrans(int sec)
    {
        yield return new WaitForSeconds(sec);
        thismesh.enabled = false;
    }
    protected override void Acting(Stage newStage)
    {
        currentStage = newStage;
        switch (newStage)
        {
            case Stage.Stage2_DagonFind:
                StartCoroutine(InitPosition());
                break;
            case Stage.Stage2_DagonOceanClose1:
                StartCoroutine(DagonCloser());
                break;
            case Stage.Stage2_DagonOceanClose2:
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
