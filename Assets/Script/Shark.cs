using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using System.Collections;

public class Shark : Actor
{
    public GameObject player;
    public Animator anim;
    public string[] animationNames;
    public SkinnedMeshRenderer objRenderer;
    public float distanceToBoat;
    bool isRetreat = false;
    int randomIndex;

    void Start()
    {
        anim.speed = 0.5f;
        player = GameObject.FindGameObjectWithTag("Raft");
        StartCoroutine(RepeatedAction());
    }

    IEnumerator RepeatedAction()
    {
        while (true)
        {
            float randomAngle = UnityEngine.Random.Range(0f, 360f);
            float angleRad = randomAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angleRad), -0.2f, Mathf.Sin(angleRad)) *distanceToBoat;
            transform.position = player.transform.position + offset;

            Vector3 AB = player.transform.position - transform.position;
            Vector3 orthogonal = Vector3.Cross(AB, Vector3.up);
            if (orthogonal == Vector3.zero)
            {            
                orthogonal = Vector3.forward;
            }
            orthogonal = orthogonal.normalized;
            transform.rotation = Quaternion.LookRotation(orthogonal, Vector3.up) * Quaternion.Euler(0, 90f, 0);
            randomIndex = UnityEngine.Random.Range(0, animationNames.Length);
            objRenderer.enabled =true;
            anim.SetBool(animationNames[randomIndex], true);

            // 이동 Coroutine 시작
            StartCoroutine(yChange());
            yield return new WaitForSeconds(7);
            if(isRetreat){
                break;
            }
        }
    }
    IEnumerator yChange()
    {
        float halfDuration = 1.6f;
        float elapsed = 0f;
        while (elapsed < halfDuration)
        {
            float delta = (3 / halfDuration) * Time.deltaTime*0.5f;
            transform.position += new Vector3(0, delta, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            float delta = (3 / halfDuration) * Time.deltaTime*0.5f;
            transform.position -= new Vector3(0, delta, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }        
        objRenderer.enabled =false;
        foreach (string param in animationNames)
        {
            anim.SetBool(param, false);
        }
    }
    public void Retreat(){
        isRetreat = true;
        StartCoroutine(WaitfoSec(5));
    }

    IEnumerator WaitfoSec(int n){
        yield return new WaitForSeconds(n);
        MainTimeManager.Instance.SetStage(Stage.Stage1_BahamutAppear,this.GetType().Name);
    }

    protected override void Acting(Stage newStage)
    {
        switch (newStage)
        {
            case Stage.Stage1_EnterZone1:
                // 처음 등장하는 타이밍. 따라서 해당 부분 작업은 Start() 안에서 이루어진다.
                break;
            case Stage.Stage1_EnterZone2:
                break;
            case Stage.Stage1_EnterZone3:
                break;
            case Stage.Stage1_SharkDissapear:
                Retreat();
                break;
            default:            
                Destroy(gameObject);
                break;
        }
    }
}