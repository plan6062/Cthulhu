using UnityEngine;
using DG.Tweening;

public class CircularPath : MonoBehaviour
{
    public Transform raft; // 원의 중심점
    public float radius = 5f; // 반지름
    public float speed = 1f; // 이동 속도

    private float angle = 90f; // 현재 각도
    private float movecount = 0f;

    public State currentState;
    public enum State { MoveInCircle, MoveTowardBoat_Start,MoveTowardBoat_ing, AttackBoat, Retreat }

    void Start ()
    {
        currentState = State.MoveInCircle;
        angle = transform.rotation.eulerAngles.y;
    }
    void Update()
    {
        if (currentState == State.MoveInCircle){
            MoveInCircle();
        } else if (currentState == State.MoveTowardBoat_Start){
            MoveTowardBoat_Start();
        } else if (currentState == State.AttackBoat){
            AttackBoat();
        }
    }


    void MoveInCircle()
    {
        
        if (raft == null)
        {
            Debug.Log("No boat");
        }
        angle += speed * Time.deltaTime;
        float x = raft.position.x + Mathf.Cos(angle) * radius;
        float z = raft.position.z + Mathf.Sin(angle) * radius;
        Vector3 newPosition = new Vector3(x, transform.position.y, z);

        Vector3 direction = (newPosition - transform.position).normalized;
        transform.position = newPosition;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
        movecount += Time.deltaTime;
        if (movecount > 3) {
            currentState = State.MoveTowardBoat_Start;
        }
    }
     void MoveTowardBoat_Start()
    {
            
        currentState = State.MoveTowardBoat_ing;
        Vector3 targetPosition = new Vector3(raft.position.x, transform.position.y, raft.position.z);
        
        transform.DOKill(); // 이전 경로 이동 중단
        Vector3 to = new Vector3(0, -90, 0);

        // 회전 애니메이션 실행 (1초 동안 회전)
        transform.DORotate(to, 1f, RotateMode.Fast).SetEase(Ease.InSine);
        transform.DOMove(targetPosition, 3f).OnComplete(() => currentState = State.AttackBoat);
        

        // sharkAnimator.SetBool("isSwimming", true);
    }

    void AttackBoat()
    {
        // Vector3 targetPosition = raft.position;
        // transform.DOKill(); // 이전 경로 이동 중단
        // transform.DOMove(targetPosition, 1f).SetEase(Ease.Linear).OnComplete(() =>
        // {
        //     sharkAnimator.SetTrigger("attack");
        //     currentState = State.Retreat;
        // });
        // LookAtDirection(targetPosition - transform.position);
        Debug.Log("dsjfklsdf");
    }

    // void Retreat()
    // {
    //     Vector3 targetPosition = new Vector3(raft.position.x + maxRadius, transform.position.y, raft.position.z + maxRadius);
    //     transform.DOKill(); // 이전 경로 이동 중단
    //     transform.DOMove(targetPosition, 1f).SetEase(Ease.Linear).OnComplete(() =>
    //     {
    //         sharkAnimator.SetBool("isRetreating", false);
    //         currentState = State.MoveInCircle;
    //         GenerateWaypoints(); // 웨이포인트 재생성
    //         StartMoveInCircle();
    //     });
    //     LookAtDirection(targetPosition - transform.position);

    //     sharkAnimator.SetBool("isRetreating", true);
    // }
}
