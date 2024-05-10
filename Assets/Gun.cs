using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Bullet Info")]
    public GameObject bulletGameObj;
    public float bulletSpeed;
    Transform bulletTransform;
    Rigidbody bulletRb;
    Bullet BulletScript;

    public Transform barrel;

    public bool Shooted { get; set; }
    private void Start(){
        bulletTransform = bulletGameObj.transform;
        bulletRb = bulletGameObj.GetComponent<Rigidbody>();
        BulletScript = bulletGameObj.GetComponent<Bullet>();
    }

    public void Fire(){
        Shooted = true;
        bulletTransform.position = barrel.position;
        bulletRb.velocity = barrel.forward * bulletSpeed;
    }

    public void CancelFire(){
        Shooted = false;
        BulletScript.DestroyJoint();
    }

    public void Update(){
        if(!Shooted){
            bulletTransform.position = barrel.position;
            bulletTransform.forward = barrel.forward;
        }
    }
}