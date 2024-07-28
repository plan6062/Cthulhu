using UnityEngine;

public class CorpseSink : MonoBehaviour
{
    public float sinkSpeed = 2f;
    public float detectionRange = 3f;
    public GameObject player;

    private bool isSinking = false;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
        if (player == null)
        {
            Debug.LogError("플레이어 오브젝트가 할당되지 않았습니다.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        Debug.Log($"플레이어와의 거리: {distance}, 현재 Y 위치: {transform.position.y}, isSinking: {isSinking}");

        if (!isSinking && distance < detectionRange)
        {
            isSinking = true;
            Debug.Log("가라앉기 시작!");
        }

        if (isSinking)
        {
            Vector3 newPosition = transform.position;
            newPosition.y -= sinkSpeed * Time.deltaTime;
            transform.position = newPosition;
            Debug.Log($"새로운 Y 위치: {newPosition.y}");
        }
    }
}