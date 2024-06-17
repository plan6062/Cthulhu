using UnityEngine;

public class RaftController : MonoBehaviour
{
    public Transform frontCollider;
    public Transform backCollider;
    public Transform leftCollider;
    public Transform rightCollider;
    public float moveSpeed = 5f;

    private Rigidbody raftRigidbody;
    private PropellerRotation propellerRotation;

    void Start()
    {
        raftRigidbody = GetComponent<Rigidbody>();
        propellerRotation = GetComponentInChildren<PropellerRotation>();
    }

    void Update()
    {
        if (propellerRotation.isRotating)
        {
            // Debug.Log("Propeller is rotating.");

            MoveRaftIfColliding(frontCollider, -transform.up);
            MoveRaftIfColliding(backCollider, transform.up);
            MoveRaftIfColliding(leftCollider, -transform.right);
            MoveRaftIfColliding(rightCollider, transform.right);
        }
        else
        {
            // Debug.Log("Propeller is not rotating.");
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
                return;
            }
        }
    }
}
