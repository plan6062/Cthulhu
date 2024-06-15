using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public SharkMovement sharkMovement;
    
    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 태그가 "Bullet"인지 확인
        if (other.gameObject.CompareTag("Bullet") && sharkMovement.isHitTiming)
        {
            // bool 변수 변경
            sharkMovement.isHit = true;
            Debug.Log("Bullet hit detected!");
        }
    }
    
}