using System.Collections;
using UnityEngine;

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
                StartCoroutine(TurnBoat());
            }
            else if (propellerRotation.isRotating)
            {
                MoveRaftIfColliding(Collider, propellerRotation.direction);
            }
        }
    }

    IEnumerator TurnBoat(){
        yield return null;
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
