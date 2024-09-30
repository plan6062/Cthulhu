using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public SharkMovement sharkMovement;
    public Bahamut_old bahamut;
    
    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 태그가 "Bullet"인지 확인
        if (other.gameObject.CompareTag("Bullet") && sharkMovement.isHitTiming)
        {
            // bool 변수 변경
            sharkMovement.isHit = true;
            Debug.Log("Bullet hit detected!");
        }
         if (other.gameObject.CompareTag("EatLocation") && sharkMovement.sharkhealth < 1)
        {
            bahamut.EatShark();
            Debug.Log("부딛힘");
            sharkMovement.Death();
        }
    }

    
}