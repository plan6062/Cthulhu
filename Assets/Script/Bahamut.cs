using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class Bahamut : Actor
{

    bool isChasing = false;
    void Start(){
        // 애니메이션 재생(솟구치는 수영)
        // 사운드 출력
    }

    void Update(){
        if(isChasing){
            
            // Vector3 direction = player.position - transform.position;
            // direction.Normalize();  
            // transform.position += direction * speed * Time.deltaTime;

            // 충돌 감지를 어떤 방식으로 할 것인가?
            // n초에 한번 수면 위로 올라오기 전에, 플레이어와의 xz 좌표상 거리를 측정한다. 그 거리가 n 이하면 강제로 이벤트 발생.
            // 어떻게 이벤트가 실행되지?
            // 대가리 각도는 그대로 유지된 채 몸 포물선 이동만?
            // 
        }
    }

    protected override void Acting(Stage newStage)
    {
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
            case Stage.Stage1_LookThroughHole:
                break;
            default:            
                break;
        }
    }
}