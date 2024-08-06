using UnityEngine;
using System.Collections;

public class PropellerRotation : MonoBehaviour
{
    public float rotationSpeed = 1000f;
    public bool isRotating { get; private set; } = false;

    public AudioSource audioSource;
    public AudioClip audioClip;

    public Vector3 direction;
    public Transform target;

    // 암초 관련 변수
    public GameObject reef;
    public float reefDetectionRange = 5f;
    private bool isNearReef = false;

    // 과열 시스템 변수
    private float heatGauge = 0f;
    private float maxHeat = 100f;
    private float cooldownTime = 5f;
    private bool isOverheated = false;
    public float heatIncreaseRate = 10f; // 초당 증가하는 열량
    public float heatDecreaseRate = 5f; // 초당 감소하는 열량

    public ParticleSystem smokeParticleSystem;


    private void Start()
    {
        audioSource.clip = audioClip;
        if(smokeParticleSystem != null){
            smokeParticleSystem.Stop();
        }
        if (reef == null)
        {
            reef = GameObject.FindGameObjectWithTag("Reef"); 
        }
    }

    public void StartRotation()
    {
        if (!isOverheated && !isNearReef)
        {
            isRotating = true;
            audioSource.Play();
        }
    }

    public void StopRotation()
    {
        isRotating = false;
        audioSource.Stop();
    }

    void Update()
{
    Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
    Vector3 currentPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

    direction = (targetPosition - currentPosition).normalized;

    if (reef != null)
    {
        float distanceToReef = Vector3.Distance(transform.position, reef.transform.position);

        if (distanceToReef < reefDetectionRange && !isNearReef)
        {
            isNearReef = true;
            StopRotation();
            if (smokeParticleSystem != null)
            {
                smokeParticleSystem.Play();
            }
        }
        else if (distanceToReef >= reefDetectionRange && isNearReef)
        {
            isNearReef = false;
            if (!isOverheated)
            {
                StartRotation();
            }
            if (smokeParticleSystem != null)
            {
                smokeParticleSystem.Stop();
            }
        }
    }
    else
    {
        Debug.LogWarning("reef 오브젝트 설정 필요");
    }

    if (isRotating)
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        IncreaseHeat();
    }
    else
    {
        DecreaseHeat();
    }

    // 과열 체크
    CheckOverheat();

    
   // Debug.Log($"Current state - IsRotating: {isRotating}, IsNearReef: {isNearReef}, IsOverheated: {isOverheated}, HeatGauge: {heatGauge}");
}

    private void IncreaseHeat()
    {
        heatGauge += heatIncreaseRate * Time.deltaTime;
        heatGauge = Mathf.Clamp(heatGauge, 0f, maxHeat);
    }

    private void DecreaseHeat()
    {
        heatGauge -= heatDecreaseRate * Time.deltaTime;
        heatGauge = Mathf.Clamp(heatGauge, 0f, maxHeat);
    }

    private void CheckOverheat()
    {
        if (heatGauge >= maxHeat && !isOverheated)
        {
            isOverheated = true;
            StopRotation();
            StartCoroutine(CooldownRoutine());
            if(smokeParticleSystem != null)
            {
                smokeParticleSystem.Play();
            }
        }
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldownTime);
        isOverheated = false;
        heatGauge = 0f;
        if(smokeParticleSystem != null)
        {
            smokeParticleSystem.Stop();
        }
        if (!isNearReef)
        {
            StartRotation();
        }
    }

    // 현재 열 게이지 값을 반환하는 메서드
    public float GetHeatGaugePercentage()
    {
        return (heatGauge / maxHeat) * 100f;
    }
}