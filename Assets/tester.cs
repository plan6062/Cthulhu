using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : MonoBehaviour
{
    public Transform player;
    Quaternion headRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (EyeTracker.Instance.TryGetCenterEyeNodeStateRotation(out headRotation))
        {
            // 시야 방향 벡터를 계산
            Vector3 forwardDirection = headRotation * Vector3.forward*-1;

            // 물체가 위치할 포지션을 계산
            Vector3 targetPosition = Camera.main.transform.position + forwardDirection * 5f;

            // 물체의 높이를 조정
            targetPosition.y = 2f;

            // 물체의 위치를 업데이트
            transform.position = targetPosition;

            // 물체가 플레이어를 향하도록 회전
            transform.LookAt(Camera.main.transform.position);
        }
       
    }
}