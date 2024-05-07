using UnityEngine;

public class RaftController : MonoBehaviour
{
    public Transform frontCollider;
    public Transform backCollider;
    public Transform leftCollider;
    public Transform rightCollider;
    public float moveSpeed = 5f;

    private Rigidbody raftRigidbody;

    void Start()
    {
        raftRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveRaftIfColliding(frontCollider, -transform.up);
        MoveRaftIfColliding(backCollider, transform.up);
        MoveRaftIfColliding(leftCollider, -transform.right);
        MoveRaftIfColliding(rightCollider, transform.right);
    }

    void MoveRaftIfColliding(Transform colliderTransform, Vector3 direction)
    {
        Collider[] colliders = Physics.OverlapBox(colliderTransform.position, colliderTransform.localScale / 2f, colliderTransform.rotation);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Paddle"))
            {
                raftRigidbody.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
                return;
            }
        }
    }
}
