using UnityEngine;

public class FollowObject : MonoBehaviour
{
    private Transform target;

    void Update()
    {
        if (target == null)
        {
            GameObject raftObject = GameObject.FindGameObjectWithTag("Raft");
            if (raftObject != null)
            {
                target = raftObject.transform;
            }
        }
        else
        {
            Vector3 newPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.position = newPosition;
        }
    }
}