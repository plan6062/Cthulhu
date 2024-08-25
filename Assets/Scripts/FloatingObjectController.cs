using UnityEngine;

public class FloatingObjectController : MonoBehaviour
{
    public float minDistance = 3f; // 보트와의 최소 거리
    public float forceMultiplier = 5f; // 힘의 크기
    
    private Rigidbody rb;
    private GameObject raft;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        raft = GameObject.FindGameObjectWithTag("Raft");
        
        if (raft == null)
        {
            Debug.LogError("Raft 태그 오브젝트 없음");
        }
    }

    void FixedUpdate()
    {
        if (raft != null)
        {
            float distance = Vector3.Distance(transform.position, raft.transform.position);
            
            if (distance > minDistance)
            {
                Vector3 direction = (raft.transform.position - transform.position).normalized;
                rb.AddForce(direction * forceMultiplier, ForceMode.Force);
            }
        }
    }
}