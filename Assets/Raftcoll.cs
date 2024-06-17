using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raftcoll : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody raftRigidbody;
    public PropellerRotation propellerRotation;
     
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Paddle") && propellerRotation.isRotating)
        {
            raftRigidbody.MovePosition(transform.position + propellerRotation.direction * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
