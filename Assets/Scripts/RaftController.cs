using System.Collections;
using UnityEngine;
using Crest;
using UnityEngine.InputSystem;
public class RaftController : MonoBehaviour
{
    // public Transform frontCollider;
    // public Transform backCollider;
    // public Transform leftCollider;
    // public Transform rightCollider;
    public Transform Collider;
    public float moveSpeed = 5f;
    public GameObject player;

    private Rigidbody raftRigidbody;
    private PropellerRotation propellerRotation;
    private bool isAttackedbyBahamut = false;
    private bool isBoatDead = false;
    private Transform bahamutT;
    private float currentRotation = 0f;
    public float rotationSpeed = 30f; // 회전 속도
    Vector3 fixedposition;
    bool done =false;
    float counter =0;
    Vector3 downward;
    void Start()
    {
        downward = new Vector3(0,-24f,0); 
        raftRigidbody = GetComponent<Rigidbody>();
        propellerRotation = GetComponentInChildren<PropellerRotation>();
    }

    void Update()
    {
        if(!done){
            if (propellerRotation.isRotating && !isBoatDead) {
                MoveRaftIfColliding(Collider, propellerRotation.direction);
            }
            
            if(isBoatDead)
            { 
                // transform.position = fixedposition;
                // float rotationAmount = rotationSpeed * Time.deltaTime;
                // transform.RotateAround(bahamutT.position, bahamutT.right, rotationAmount);
                // currentRotation += rotationAmount;

                // // 180도 회전 후 정지
                // if (currentRotation >= 20f)
                // {
                //     done = true;
                // }
                counter += Time.deltaTime;
                if(counter < 2){
                    transform.Translate(downward * Time.deltaTime , Space.World);
                }
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
        // player.transform.parent = null;
        GetComponent<BoatProbes>().enabled = false;
        fixedposition = transform.position;
        raftRigidbody.isKinematic = true;
        bahamutT = bahamutTransform;
        isBoatDead = true;
    }
}
