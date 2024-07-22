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

    // 과열 시스템 변수
    private float heatGauge = 0f;
    private float maxHeat = 100f;
    private float cooldownTime = 5f;
    private bool isOverheated = false;
    public float heatIncreaseRate = 10f; // 초당 증가하는 열량
    public float heatDecreaseRate = 5f; // 초당 감소하는 열량

    private void Start()
    {
        audioSource.clip = audioClip;
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
        }
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldownTime);
        isOverheated = false;
        heatGauge = 0f;
    }

    // 현재 열 게이지 값을 반환하는 메서드
    public float GetHeatGaugePercentage()
    {
        return (heatGauge / maxHeat) * 100f;
    }
}