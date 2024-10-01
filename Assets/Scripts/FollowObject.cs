using UnityEngine;

public class FollowObject : MonoBehaviour
{
    private Transform target;
    public bool isAttacked;
    float counter =0;
    private Vector3 bahamutT;    
    public float rotationSpeed = 90f; // 회전 속도
    Vector3 direction;
    Vector3 downward;
    public GameObject stuff1;
    public GameObject stuff2;

    private void Start() {
        downward = new Vector3(0,-8f,0); 
    }
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
        if (!isAttacked)
        {
            Vector3 newPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.position = newPosition;
        }
        if(isAttacked){ 
            counter += Time.deltaTime;
            if(counter < 1.5){
                transform.Translate(downward * Time.deltaTime, Space.World);
                // Vector3 moveXZ = direction * 6 * Time.deltaTime;
                // transform.position += moveXZ; 


                // if (currentYDecrease < 30f)
                // {
                //     float yStep = 2f * Time.deltaTime;
                //     if (currentYDecrease + yStep > 10f)
                //     {
                //         yStep = 10f - currentYDecrease;
                //     }
                //     transform.position += new Vector3(0, -yStep, 0);
                //     currentYDecrease += yStep;
                // }
            }
        }
    }

    public void AttackedbyBahamut(Vector3 bahamutTransform) {
        isAttacked = true;
        stuff1.SetActive(false);
        stuff2.SetActive(false);
        bahamutT = bahamutTransform;
        direction = transform.position - bahamutT;
        direction.y = 0;
        direction.Normalize();
    }
}