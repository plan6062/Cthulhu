using UnityEngine;

public class CorpseShakeAndSink : MonoBehaviour
{
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 10f;
    public float sinkSpeed = 2f;
    public float detectionRange = 3f;
    public GameObject player;

    private Vector3 initialPosition;
    private bool isSinking = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (player == null) return;

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
    }
}