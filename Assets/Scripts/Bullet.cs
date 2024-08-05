using UnityEngine;

public class Bullet : Actor
{
    FixedJoint fixedJoint;
    public AudioSource audioSource;
    public AudioClip audioClip;
    

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }
    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag =="Monster"){
            fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();
            audioSource.Play();
        }
    }
    public void DestroyJoint(){
        Destroy(fixedJoint);
    }
}