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
    public GameObject transparentColliderPrefab;
    public float speed;
    float counter;
    float swimcounter;
    int swimcount = 0;
    float checkdistance;
    float blackoutDistance;
    public float chaseSpeed = 5f;
    bool isPlayerfindbahamut = false;
    bool is10secFinished = false;
    Quaternion headRotation;
    float spawnDistance = 30f; 
    [SerializeField]
    float colliderRadius;
    public RaftController raft;
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
                direction.y = 0;
                transform.position += direction * speed * Time.deltaTime;
                counter += Time.deltaTime;
                if (counter > 3){
                    counter = 0;
                    isChasing = false;
                    if(Checkdistance()){
                        // 바하무트 애니메이션 재생
                        // 1안 - 사람이 보는 방향 벡터에서 N만큼 떨어진 곳에서 바하무트가 솟아 오른다.
                        // 그와 동시에 해당 방향 반대 방향으로 배가 기울어진다. 배 기울어짐은 물리에 의존하지 않고 순전히 실제 수치를 조절한다.
                        // 바하무트의 애니메이션 재생과 배 기울어짐의 시간 차이는 .. 테스트 해보면서 조절
                        // 
                        
                        StartCoroutine(Death_OceanDrown());
                        MainTimeManager.Instance.SetStage(Stage.Stage1_BahamutSwimAttack_Death);
                    } else {
                        StartCoroutine(Popout());
                    }
                }
            }
        }
        if(currentStage == Stage.Stage1_BahamutSwimAttack_Death){
            if(isDrown){
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

    IEnumerator Death_OceanDrown()
    {   
        EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation);
        Vector3 BahamutDirection = headRotation * Vector3.forward;
        BahamutDirection.y = 0;
        BahamutDirection = BahamutDirection.normalized * 10.0f;
        transform.position = player.position + BahamutDirection;
        transform.LookAt(player.transform.position);// 이때 필요하다면 각도 조절
        
        // 애니메이션 재생 //
        
        yield return new WaitForSeconds(3); // 이 수치는 실제 플레이에서 테스트 후 조절

        raft.AttackedbyBahamut(transform);

        // 플레이어가 바다에 빠지고 잠시 대기
        yield return new WaitForSeconds(5);

        Vector3 totalDirection = Vector3.zero;
        for (int i = 0; i < 5; i++)
        {
            EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation);
            Vector3 forwardDirection = headRotation * Vector3.forward;
            totalDirection += forwardDirection;
            yield return new WaitForSeconds(0.2f);
        }

        Vector3 averageDirection = totalDirection / 5.0f;
        Vector3 oppositeDirection = -averageDirection;
        oppositeDirection.y = 0; // y 성분을 0으로 제거
        Vector3 vectorA = oppositeDirection.normalized * 10.0f; // 길이를 10으로 만든다

        // 3. 플레이어에서부터 A 벡터만큼 떨어진 트랜스폼에서 소리를 재생한다. 이 트랜스폼에 투명한 콜라이더 B를 위치시킨다.
        GameObject colliderObject = Instantiate(transparentColliderPrefab, player.position + vectorA, Quaternion.identity);
        colliderObject.transform.localScale = new Vector3(colliderRadius, colliderRadius, colliderRadius);

        // 오디오 소스 설정 및 재생
        AudioSource audioSource = colliderObject.AddComponent<AudioSource>();
        audioSource.Play();

        // 4. 이후 플레이어의 headRotation을 계속해서 파악하다가, 콜라이더와 충돌하면 바하무트 소환
        while (true)
        {
            if(EyeTracker.Instance.CheckSightCollison(player.gameObject,"sightcollision"))
            {
                transform.position = player.position + vectorA;
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(-directionToPlayer);
                isDrown = true;
                break;
            } else {
                yield return null;
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
                isChasing = true;
                break;
            case Stage.Stage1_BahamutSwimAttack_Death:
                break;
            case Stage.Stage1_GetClosetoReef:
                break;
            case Stage.Stage1_StopBoat:
                break;
            case Stage.Stage1_LookThroughHole:
                break;
            case Stage.Stage1_SwimStop:
                break;
            case Stage.Stage1_FindBahamut:
                break;
            default:            
                break;
        }
    }
}