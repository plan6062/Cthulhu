using UnityEngine;

public class Bullet : MonoBehaviour
{
     FixedJoint fixedJoint;
    
    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag =="Monster"){
            fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
        }
    }
    public void DestroyJoint(){
        Destroy(fixedJoint);
    }
}