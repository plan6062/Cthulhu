using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class Shark : Actor
{
    Quaternion headRotation;
    void Start(){
        
        
    }

    public void Retreat(){
        EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation);
        Vector3 headRotation_euler = headRotation.eulerAngles;
        headRotation_euler.y = 0;
        Quaternion newRotation = Quaternion.Euler(headRotation_euler);
        // 이후 newRotation 방향으로 후퇴, 이후 스스로를 파괴
        // 트래킹 없이, 지정된 방향으로 후퇴?
    }

    protected override void Acting(Stage newStage)
    {
        switch (newStage)
        {
            // case Stage.Stage1_EnterZone1:
            //     // 처음 등장하는 타이밍. 따라서 해당 부분 작업은 Start() 안에서 이루어진다.
            //     break;
            // case Stage.Stage1_EnterZone2:
            //     break;
            // case Stage.Stage1_EnterZone3:
            //     break;
            case Stage.Stage1_SharkDissapear:
                Retreat();
                break;
            default:            
                break;
        }
    }
}