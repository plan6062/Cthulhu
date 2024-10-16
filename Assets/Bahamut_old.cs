using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bahamut_old : MonoBehaviour
{

    public Transform initPosition;
    public Transform EatSharkPosition;
    public AudioSource EatSharkaudioSource;
    public AudioClip EatSharkaudioClip;

    public AudioSource ChaseaudioSource;
    public AudioClip ChaseaudioClip;
    public AudioSource WateraudioSource;
    public AudioClip WateraudioClip;
    public Transform ChasePosition;
    public Transform ChasePosition2;
    public int startChecker = 0;
    public int health = 2;
    public Animator anim;
    public Transform raft; // 플레이어의 Transform
    public float speed = 5f; // 이동 속도
    public float rotationSpeed = 90f; // 방향 전환 속도 (도/초)
    public float fixedYPosition = 0f; // 고정된 Y 좌표
    
    void Start() {
        EatSharkaudioSource.clip = EatSharkaudioClip;
        ChaseaudioSource.clip = ChaseaudioClip;
        WateraudioSource.clip = WateraudioClip;
    }

    public void InitPosition(Transform trans){
        anim.SetBool("eatshark",true);
        transform.position = trans.position;
    }
    public void EatShark(){
        
        EatSharkaudioSource.Play();
        anim.SetBool("eatshark",true);
        transform.position = EatSharkPosition.position;
        StartCoroutine(StartAfterDelay(10));
        StartCoroutine(SoundAfterDelay(3.8f));
    }
    IEnumerator StartAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        startChecker = 1;
        
    }
    IEnumerator SoundAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        WateraudioSource.Play();
        
    }
    void Update() {
        if (startChecker ==1){
            MoveTowardBoat();
        } else if (startChecker ==2){
            Underwater();
        }
    }
    
    bool first_MoveTowardBoat = true;
    void MoveTowardBoat_init()
    {
        anim.SetBool("chase",true);
        InitPosition(ChasePosition);
        ChaseaudioSource.Play();
    }
    void MoveTowardBoat() {
        
        if(first_MoveTowardBoat){
            MoveTowardBoat_init();
            first_MoveTowardBoat = false;
        }
        
        // 현재 위치와 플레이어 위치 계산 (XZ 평면에서만)
        Vector3 targetPosition = new Vector3(raft.position.x, fixedYPosition, raft.position.z);
        Vector3 currentPosition = new Vector3(transform.position.x, fixedYPosition, transform.position.z);

        // 플레이어를 향한 방향 계산
        Vector3 direction = (targetPosition - currentPosition).normalized;

        // 현재 방향
        Vector3 currentDirection = transform.forward;

        // 방향 전환 속도 적용
        Vector3 newDirection = Vector3.RotateTowards(currentDirection, direction, rotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);

        // 고개를 양옆으로만 움직이기 위해 Y축 회전만 적용
        Quaternion rotation = Quaternion.LookRotation(newDirection);
        rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

        // 괴물 회전 (XZ 평면에서만)
        transform.rotation = rotation;

        // 괴물 이동
        transform.position += newDirection * speed * Time.deltaTime;
    }

    bool first_Underwater = true;
    bool startagain = false;
    IEnumerator Underwater_init(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetBool("chaseagain1",true);
        InitPosition(ChasePosition2);
        ChaseaudioSource.Play();
        first_Underwater = false;
        startagain = true;
       
    }
    void Underwater() {
        if(first_Underwater){
            StartCoroutine(Underwater_init(10));
            first_Underwater = false;
        } 
        if (startagain) {
            startChecker = 1;
            Vector3 targetPosition = new Vector3(raft.position.x, fixedYPosition, raft.position.z);
            Vector3 currentPosition = new Vector3(transform.position.x, fixedYPosition, transform.position.z);

            // 플레이어를 향한 방향 계산
            Vector3 direction = (targetPosition - currentPosition).normalized;
            transform.LookAt(direction);
        }
        
    }
    IEnumerator WaitForSec(float time){
        yield return new WaitForSeconds(time);
        anim.SetBool("die1",true);
    }
    public void GetDamage(){
        ChaseaudioSource.Stop();
        health--;
        if(health < 1){
            startChecker = 3;
            
            transform.DOLookAt(raft.position,1f);
            // transform.DOLookAt(direction,1f);
            
            StartCoroutine(WaitForSec(0.5f));
        } else {
            startChecker = 2;
            anim.SetBool("hit1",true);
            StartCoroutine(SoundAfterDelay(1.7f));
        }
    }
}
