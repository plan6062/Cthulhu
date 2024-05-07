using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform target; 

    void Update()
    {
        
        Vector3 newPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.position = newPosition;
    }
}