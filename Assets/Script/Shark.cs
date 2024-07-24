using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class Shark : Actor
{
    Quaternion headRotation;
    public Text rotationText;

    void Update(){
         EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation);
        Debug.Log(headRotation);
        rotationText.text = $"Head Rotation: {headRotation.eulerAngles}";
    }

    protected override void Acting(Stage newStage)
    {
        switch (newStage)
        {
            case Stage.Stage1_EnterZone1:
                // 처음 등장하는 타이밍. 따라서 해당 부분 작업은 Start() 안에서 이루어진다.
                break;
            case Stage.Stage1_EnterZone2:
                break;
            case Stage.Stage1_EnterZone3:
                break;
            case Stage.Stage1_SharkDissapear:
                break;
            default:            
                break;
        }
    }
}