using UnityEngine;
using VHierarchy.Libs;

public class CorpseShakeAndSink : Actor
{
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 10f;
    public float sinkSpeed = 2f;
    public float detectionRange = 15f;

    private GameObject player;
    private Vector3 initialPosition;
    private bool isSinking = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (!isSinking && distance < detectionRange)
        {
            isSinking = true;
        }

        Vector3 shakeOffset = new Vector3(
            Mathf.Sin(Time.time * shakeSpeed) * shakeAmount,
            Mathf.Sin(Time.time * shakeSpeed * 0.9f) * shakeAmount,
            Mathf.Sin(Time.time * shakeSpeed * 1.1f) * shakeAmount
        );

        Vector3 newPosition = initialPosition + shakeOffset;

        if (isSinking)
        {
            newPosition.y -= sinkSpeed * Time.deltaTime;
            initialPosition.y -= sinkSpeed * Time.deltaTime;
        }

        transform.position = newPosition;

        // 다음 스테이지로 넘어가기 위한 조건이 만족되었을 때,
        if(transform.position.y < -3)
        {
            // MainTimaManager 인스턴스의 SetStage 메서드로 스테이지를 변경한다.
            MainTimeManager.Instance.SetStage(Stage.Opening_FindBattery);

            // 이 게임오브젝트가 나중에 더 쓰일 일이 없다면 스스로를 파괴한다.
            Destroy(gameObject);
        }
    }
}