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
    bool isChasing = false;
    bool isDrown = false;
    public Transform player;
    public float speed;
    float counter;
    float swimcounter;
    int swimcount = 0;
    float checkdistance;
    float blackoutDistance;
    public float chaseSpeed = 5f;
    bool isPlayerfindbahamut = false;
    bool is10secFinished = false;

    public Transform[] swimpoints = new Transform[4];
    public Transform poppoint;
    public bool isBahamutHit = false;  
    void Start(){
        // 애니메이션 재생(솟구치는 수영)
        // 사운드 출력
    }

    void Update(){
        if(currentStage == Stage.Stage1_BahamutSwimAttack){
            if(isChasing){
                Vector3 direction = player.position - transform.position;
                direction.Normalize();  
                transform.position += direction * speed * Time.deltaTime;
                counter += Time.deltaTime;
                if (counter > 3){
                    counter = 0;
                    isChasing = false;
                    if(Checkdistance()){
                        // 바하무트 애니메이션 재생
                        // 그냥 배만 기울이면 플레이어는 알아서 떨어진다. 배 기울이기 스크립트 추가하기.
                        StartCoroutine(Death_OceanDrown());
                        MainTimeManager.Instance.SetStage(Stage.Stage1_BahamutSwimAttack_Death);
                    } else {
                        StartCoroutine(Popout());
                    }
                }
            } else if(isDrown){
                // 바하무트를 플레이어 방향으로 이동
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.position += directionToPlayer * chaseSpeed * Time.deltaTime;

                // 플레이어와의 거리 계산
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                // 플레이어와의 거리가 blackoutDistance 이하가 되면 암전 발생
                if (distanceToPlayer <= blackoutDistance)
                {
                    // 암전 로직 (예: 화면을 검게 처리하는 코드)
                    // 게임 오버
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
                MainTimeManager.Instance.SetStage(Stage.Stage1_SwimStop);
            } else if(swimcounter > 5) {
                swimcounter = 0;            
                transform.position = swimpoints[swimcount].position;
                swimcount += 1;
                // 애니메이션 재생
            }
        }
        if(currentStage == Stage.Stage1_SwimStop){
            
            counter += Time.deltaTime;
            if(counter > 6){
                counter = 5; // 5초가 지난 후부터, 1초에 한번 확인함.
                if(EyeTracker.Instance.CheckSightCollison(player.gameObject, "hole")){
                    gameObject.transform.position = poppoint.position;
                    gameObject.transform.rotation = poppoint.rotation;
                    MainTimeManager.Instance.SetStage(Stage.Stage1_FindBahamut);
                }
            }
        }
        if(currentStage == Stage.Stage1_FindBahamut){
            if (!isPlayerfindbahamut){
                if(EyeTracker.Instance.CheckSightCollison(player.gameObject, "bahamut")){
                    isPlayerfindbahamut = true;
                }
            } else {
                // 동공 애니메이션 재생
                StartCoroutine(WaitforOpenEye(10));
            }
            if (is10secFinished){
                if(isBahamutHit){
                    // 애니메이션 재생
                } else {
                    // 애니메이션 재생
                }
            }
        }
    }

    IEnumerator WaitforOpenEye(int n){
        yield return new WaitForSeconds(n);
        is10secFinished = true;
    }
    bool Checkdistance()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < checkdistance) {
            return true;
        } else{
            return false;
        } 
    }
    IEnumerator Popout()
    {   
        // 애니메이션 재생, 사운드 재생
        yield return new WaitForSeconds(3);
        speed *= 1.2f;
        isChasing = true;
    }

    Quaternion headRotation;
    float spawnDistance = 30f; 
    IEnumerator Death_OceanDrown()
    {   
         // 플레이어가 바다에 빠지고 잠시 대기
        yield return new WaitForSeconds(5);

        // 플레이어가 바라보는 방향의 벡터 얻기
        if (EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation))
        {
            // headRotation을 Euler 각도로 변환하여 방향 벡터 얻기
            Vector3 forwardDirection = headRotation * Vector3.forward;

            // 바하무트를 플레이어로부터 spawnDistance만큼 떨어진 위치에 배치
            Vector3 spawnPosition = player.position + forwardDirection * spawnDistance;
            transform.position = spawnPosition;

            // 바하무트의 로테이션을 플레이어를 향하도록 설정
            Vector3 directionToPlayer = (player.position - spawnPosition).normalized;
            transform.rotation = Quaternion.LookRotation(-directionToPlayer);

            // 바하무트 추적 시작
            isDrown = true;
        }
    }


    protected override void Acting(Stage newStage)
    {
        currentStage = newStage;
        switch (newStage)
        {
            case Stage.Stage1_BahamutAppear:
                // 처음 등장하는 타이밍. 따라서 해당 부분 작업은 Start() 안에서 이루어진다.
                break;
            case Stage.Stage1_BahamutSwimAttack:
                isChasing = true;
                break;
            case Stage.Stage1_BahamutSwimAttack_Death:
                break;
            case Stage.Stage1_GetClosetoReef:
                break;
            case Stage.Stage1_StopBoat:
                break;
            case Stage.Stage1_LookThroughHole:
                counter = 0;
                break;
            case Stage.Stage1_SwimStop:
                counter = 0;
                break;
            default:            
                break;
        }
    }
}