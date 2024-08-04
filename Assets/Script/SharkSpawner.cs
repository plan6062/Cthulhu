using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class SharkSpawner : Actor
{
    Shark shark1;
    Shark shark2;
    Shark shark3;

    Quaternion headRotation;
    public Text rotationText;


    void Start(){
        SummonShark(shark1);
    }

    void SummonShark(Shark shark){
        EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation);
        Vector3 headRotation_euler = headRotation.eulerAngles;
        headRotation_euler.y = 0;
        Quaternion newRotation = Quaternion.Euler(headRotation_euler);
        Vector3 direction = newRotation * Vector3.back*10;
        shark.SummonPosition.position = transform.position + direction;
        shark.SummonPosition.rotation = newRotation * Quaternion.Euler(0, 90, 0);
        Instantiate(shark, shark.SummonPosition.position ,shark.SummonPosition.rotation);
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