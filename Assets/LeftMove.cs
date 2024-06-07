using UnityEngine;

public class LeftMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float maxSpeed = 5f;
    public float friction = 0.9f;

    private Rigidbody raftRigidbody;
    private PropellerRotation propellerRotation;

    void Start()
    {
        raftRigidbody = GetComponentInParent<Rigidbody>();
        propellerRotation = FindObjectOfType<PropellerRotation>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Paddle") && propellerRotation.isRotating)
        {
            MoveRaft();
        }
    }

    private void FixedUpdate()
    {
        ApplyFriction();
    }

    private void MoveRaft()
    {
        Vector3 moveDirection = -transform.right;
        float currentSpeed = Vector3.Dot(raftRigidbody.velocity, moveDirection);

        if (currentSpeed < maxSpeed)
        {
            float additionalSpeed = Mathf.Clamp(maxSpeed - currentSpeed, 0f, moveSpeed);
            raftRigidbody.AddForce(moveDirection * additionalSpeed, ForceMode.VelocityChange);
        }
    }

    private void ApplyFriction()
    {
        raftRigidbody.velocity *= friction;
    }
}