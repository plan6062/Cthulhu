using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class Bahamut : Actor
{

    Stage currentStage;
    bool isChasing = true;
    bool isDrown = false;
    Transform player;
    Transform raft;
    Transform reef;
    public GameObject transparentColliderPrefab;
    public float chasingSpeed;
    GameObject raftfloor;
    float counter;
    float swimcounter;
    int swimcount = 0;
    float checkdistance = 20f;
    float blackoutDistance = 6f;
    public float speed_underwater;
    bool isPlayerfindbahamut = false;
    bool is10secFinished = false;
    Quaternion headRotation;
    public AudioSource audioSource_growl;
    public AudioSource audioSource_coming;
    public AudioSource audioSource_underwater;    
    Transform[] swimpoints = new Transform[4];
    Transform poppoint;
    public bool isBahamutHit = false;  
    public Animator anim;
    bool check = false;
    GameObject fadeout;
    void Start(){
        poppoint = GameObject.FindGameObjectWithTag("Poppoint").transform; 
        fadeout = GameObject.FindGameObjectWithTag("MainCamera");
        raft = GameObject.FindGameObjectWithTag("Raft").transform;
        reef = GameObject.FindGameObjectWithTag("Rock").transform;
        raftfloor = GameObject.FindGameObjectWithTag("Followobject");
        for (int i = 0; i < 4; i++)
            {
                swimpoints[i] = reef.transform.GetChild(i);
            }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim.SetBool("meetfirst",true);
        StartCoroutine(WaitfoSec(5));        
    }
    IEnumerator WaitfoSec(int n){
        yield return new WaitForSeconds(n);
        audioSource_coming.Play();
        anim.SetBool("meetfirst",false);
        MainTimeManager.Instance.SetStage(Stage.Stage1_BahamutSwimAttack,this.GetType().Name);
        // 추격 브금 시작
    }
    void Update(){
        if(currentStage == Stage.Stage1_BahamutSwimAttack){
            if(isChasing){
                Vector3 direction = raft.position - transform.position;
                direction.Normalize();  
                direction.y = 0;
                transform.position += direction * chasingSpeed * Time.deltaTime;
                counter += Time.deltaTime;
                if (Checkdistance())
                {
                    StartCoroutine(Death_OceanDrown()); 
                    MainTimeManager.Instance.SetStage(Stage.Stage1_BahamutSwimAttack_Death,this.GetType().Name);
                }
                if (counter > 4 && currentStage == Stage.Stage1_BahamutSwimAttack)
                {
                    counter = 0;
                    isChasing = false;
                    StartCoroutine(Popout());
                }
            }
        }
        if(currentStage == Stage.Stage1_BahamutSwimAttack_Death){
            if(isDrown){
                // 바하무트를 플레이어 방향으로 이동
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.position += directionToPlayer * speed_underwater * Time.deltaTime;
                
                // 플레이어와의 거리 계산
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                // 플레이어와의 거리가 blackoutDistance 이하가 되면 암전 발생
                if (distanceToPlayer <= blackoutDistance)
                {
                    fadeout.GetComponent<Fadeout>().StartBlackout();
                    Destroy(gameObject);
                }
            }
        }        
        if(currentStage == Stage.Stage1_GetClosetoReef){
            // do nothing
        }
        if(currentStage == Stage.Stage1_LookThroughHole){
            counter += Time.deltaTime;
            swimcounter += Time.deltaTime;
            if(counter > 20){
                counter = 0;
                MainTimeManager.Instance.SetStage(Stage.Stage1_SwimStop,this.GetType().Name);
            } else if(swimcounter > 4) {
                swimcounter = 0;            
                transform.position = swimpoints[swimcount].position;
                transform.rotation = swimpoints[swimcount].rotation;
                swimcount += 1;
                anim.Play("diving2",0, 0f);
                
                if(swimcount==2){
                    audioSource_coming.volume = 0.7f;
                } else if (swimcount==3){
                    audioSource_coming.volume = 0.4f;
                }
                audioSource_coming.Play();
            }
        }
        if(currentStage == Stage.Stage1_SwimStop){
            
            counter += Time.deltaTime;
            if(counter > 1){
                counter = 5; // 5초가 지난 후부터, 1초에 한번 확인함.
                // if(EyeTracker.Instance.CheckSightCollison(player.gameObject, "hole"))
                if(true)
                {
                    transform.position = poppoint.position;
                    transform.rotation = poppoint.rotation;
                    MainTimeManager.Instance.SetStage(Stage.Stage1_FindBahamut,  this.GetType().Name);
                    anim.Play("meeteye",0, 0f);
                    counter = 0;
                }
            }
        }
        if(currentStage == Stage.Stage1_FindBahamut){
            
            // if(!check){
            //     StartCoroutine(WaitforDie(1));
            //     check = true;
            // }
            counter += Time.deltaTime;
            if(counter > 3){
                is10secFinished = true;
            }

            // if (!isPlayerfindbahamut){
            //     if(EyeTracker.Instance.CheckSightCollison(player.gameObject, "Bahamut")){
            //         isPlayerfindbahamut = true;
            //     }
            // } 
            if (is10secFinished && !done){
                done = true;
                if(isBahamutHit){
                    anim.Play("hiteye");
                } else {
                    anim.Play("dead2");
                    StartCoroutine(WaitforDie(1.35f));
                }
            }
        }
    }
    bool done = false;
    IEnumerator WaitforDie(float n){
        audioSource_growl.Play();
        yield return new WaitForSeconds(n);
        fadeout.GetComponent<Fadeout>().StartBlackout();
        MainTimeManager.Instance.SetStage(Stage.Stage1_EndBahamut,  this.GetType().Name);
    }
    
    IEnumerator WaitforOpenEye(int n){
        yield return new WaitForSeconds(n);
        is10secFinished = true;
    }
    bool Checkdistance()
    {
        float distance = Vector3.Distance(transform.position, raft.transform.position);
        if (distance < checkdistance) {
            return true;
        } else{
            return false;
        } 
    }
    IEnumerator Popout()
    {   
        anim.Play("diving2",0, 0f);
        audioSource_coming.Play();
        yield return new WaitForSeconds(2.5f);
        chasingSpeed *= 1.2f;
        isChasing = true;
    }

    IEnumerator Death_OceanDrown()
    {           
        EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation);
        Vector3 BahamutDirection = headRotation * Vector3.forward*-1;
        BahamutDirection.y = 0;
        BahamutDirection = BahamutDirection.normalized * 22.0f;
        transform.position = player.position + BahamutDirection;
        transform.LookAt(player.transform.position);// 이때 필요하다면 각도 조절
        
        anim.Play("diving2",0, 0f);
        
        yield return new WaitForSeconds(1.5f); // 이 수치는 실제 플레이에서 테스트 후 조절

        raft.GetComponent<RaftController>().AttackedbyBahamut(transform); // 뗏목 뒤집기 (렌더러)
        raftfloor.GetComponent<FollowObject>().AttackedbyBahamut(transform.position);
        transform.position = raft.position + new Vector3(0, -500,0);
        // 플레이어가 바다에 빠지고 잠시 대기
        yield return new WaitForSeconds(5);

        Vector3 totalDirection = Vector3.zero;
        for (int i = 0; i < 5; i++)
        {
            EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation);
            Vector3 forwardDirection = headRotation * Vector3.forward*-1;
            totalDirection += forwardDirection;
            yield return new WaitForSeconds(0.2f);
        }

        Vector3 averageDirection = totalDirection / 5.0f;
        Vector3 oppositeDirection = averageDirection;
        oppositeDirection.y = 0; // y 성분을 0으로 제거
        Vector3 vectorA = oppositeDirection.normalized * 10.0f; // 길이를 10으로 만든다

        // 3. 플레이어에서부터 A 벡터만큼 떨어진 트랜스폼에서 소리를 재생한다. 이 트랜스폼에 투명한 콜라이더 B를 위치시킨다.
        GameObject colliderObject = Instantiate(transparentColliderPrefab, player.position + vectorA, Quaternion.identity);
    
        // 4. 이후 플레이어의 headRotation을 계속해서 파악하다가, 콜라이더와 충돌하면 바하무트 소환
        while (true)
        {
            if(EyeTracker.Instance.CheckSightCollison(player.gameObject,"Sightcollision"))
            {
                transform.position = player.position + vectorA*17f;
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(directionToPlayer);
                
                anim.Play("dead1",0, 0f);
                audioSource_underwater.Play();
                isDrown = true;
                break;
            } else {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }


    protected override void Acting(Stage newStage)
    {
        currentStage = newStage;
        counter = 0;
        switch (newStage) 
        {
            case Stage.Stage1_BahamutAppear:
                // 처음 등장하는 타이밍. 따라서 해당 부분 작업은 Start() 안에서 이루어진다.
                break;
            case Stage.Stage1_BahamutSwimAttack:
                anim.speed = 2f;
                break;
            case Stage.Stage1_BahamutSwimAttack_Death:
                break;
            case Stage.Stage1_GetClosetoReef:
                
                break;
            case Stage.Stage1_LookThroughHole:
                break;
            case Stage.Stage1_SwimStop:
                anim.speed = 1f;
                break;
            case Stage.Stage1_FindBahamut:
                anim.speed = 2.5f;
                break;
            default:  
                
                break;
        }
    }
}