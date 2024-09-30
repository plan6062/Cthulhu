using System.Collections;
using UnityEngine;
using Crest;
public class RaftController : MonoBehaviour
{
    // public Transform frontCollider;
    // public Transform backCollider;
    // public Transform leftCollider;
    // public Transform rightCollider;
    public Transform Collider;
    public float moveSpeed = 5f;

    private Rigidbody raftRigidbody;
    private PropellerRotation propellerRotation;
    private bool isAttackedbyBahamut = false;
    private bool isBoatDead = false;
    private Transform bahamutT;
    private float currentRotation = 0f;
    public float rotationSpeed = 90f; // 회전 속도
    Vector3 fixedposition;

    void Start()
    {
        raftRigidbody = GetComponent<Rigidbody>();
        propellerRotation = GetComponentInChildren<PropellerRotation>();
    }

    void Update()
    {
        if(!isBoatDead){
            if (isAttackedbyBahamut){
                isBoatDead = true;
                GetComponent<BoatProbes>().enabled = false;
                fixedposition = transform.position;
            }
            else if (propellerRotation.isRotating)
            {
                MoveRaftIfColliding(Collider, propellerRotation.direction);
            }
        }
        if(isBoatDead)
        {
            transform.position = fixedposition;
            float rotationAmount = rotationSpeed * Time.deltaTime;
            transform.RotateAround(bahamutT.position, bahamutT.right, rotationAmount);
            currentRotation += rotationAmount;

            // 180도 회전 후 정지
            if (currentRotation >= 180f)
            {
                rotationSpeed = 0f;
                currentRotation = 0f; // 회전 각도 초기화
            }
        }
    }

    void MoveRaftIfColliding(Transform colliderTransform, Vector3 direction)
    {
        Collider[] colliders = Physics.OverlapBox(colliderTransform.position, colliderTransform.localScale / 2f, colliderTransform.rotation);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Paddle"))
            {
                // Debug.Log("Paddle detected. Moving raft.");
                raftRigidbody.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
                // 이부분 AddForce로 수정해야 할 듯.  (?)
                return;
            }
        }
    }

    public void AttackedbyBahamut(Transform bahamutTransform) {
        bahamutT = bahamutTransform;
        isAttackedbyBahamut = true;
    }
}
