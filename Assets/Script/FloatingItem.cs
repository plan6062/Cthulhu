using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    [SerializeField] int maxDistanceFromBoat;
    [SerializeField] int flowSpeed;
    // 역할: 바다 위에 떠다니는 물체가 보트에서 너무 멀어지지 않게끔 하는 스크립트
    // 
    // 보트와 이 오브젝트 사이의 거리가 maxDistanceFromBoat 이상이면,
    // addForce? 등 메서드를 이용해서, 보트와의 거리가 maxDistanceFromBoat에 가까워 지도록 물체에 힘을 가한다.
    // 거리가 maxDistanceFromBoat 보다 가까울 때는 아무 것도 하지 않는다.
    
}
