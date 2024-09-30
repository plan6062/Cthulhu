using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class SharkSpawner : Actor
{
    public Shark shark1;
    public Shark shark2;
    public Shark shark3;

    Quaternion headRotation;
    
    void Start(){
        SummonShark(shark1);
    }

    void SummonShark(Shark shark){
        EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation);
        Vector3 Backdirection = headRotation * Vector3.forward;
        Vector3 targetPosition = Camera.main.transform.position + Backdirection * 5f;
        targetPosition.y = 2f;
        Instantiate(shark,  targetPosition, headRotation);
    }

    protected override void Acting(Stage newStage)
    {
        switch (newStage)
        {
            case Stage.Stage1_EnterZone1:
                // 처음 등장하는 타이밍. 따라서 해당 부분 작업은 Start() 안에서 이루어진다.
                break;
            case Stage.Stage1_EnterZone2:
                SummonShark(shark2);
                break;
            case Stage.Stage1_EnterZone3:
                SummonShark(shark3);
                break;
            case Stage.Stage1_SharkDissapear:
                Destroy(gameObject);
                break;
            default:            
                break;
        }
    }
}