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
    private bool hasTriggeredReefOverheat = false;

    // 과열 시스템 변수
    private float heatGauge = 0f;
    private float maxHeat = 100f;
    private float cooldownTime = 5f;
    private bool isOverheated = false;
    public float heatIncreaseRate = 10f;
    public float heatDecreaseRate = 5f;

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
        if (!isOverheated)
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

        if (reef != null && !hasTriggeredReefOverheat)
        {
            float distanceToReef = Vector3.Distance(transform.position, reef.transform.position);

            if (distanceToReef < reefDetectionRange)
            {
                TriggerReefOverheat();
            }
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
        if(MainTimeManager.Instance.GetCurrentStage() == Stage.Stage1_SwimStop || MainTimeManager.Instance.GetCurrentStage() == Stage.Stage1_FindBahamut ){
            audioSource.volume = 0;
        
        }
        CheckOverheat();
    }

    private void TriggerReefOverheat()
    {
        heatGauge = maxHeat;
        hasTriggeredReefOverheat = true;
        CheckOverheat();
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
        StartRotation();
    }

    public float GetHeatGaugePercentage()
    {
        return (heatGauge / maxHeat) * 100f;
    }
}