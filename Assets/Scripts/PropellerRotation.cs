using UnityEngine;

public class PropellerRotation : MonoBehaviour
{
    public float rotationSpeed = 1000f;
    public bool isRotating { get; private set; } = false; 
    public AudioSource audioSource;
    public AudioClip audioClip;
    
    public Vector3 direction;
    public Transform target;

    private void Start(){
        audioSource.clip = audioClip;
    }
    public void StartRotation()
    {
        isRotating = true;
        audioSource.Play();
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

        // 목표 물체를 향한 방향 계산 (XZ 평면에서만)
        direction = (targetPosition - currentPosition).normalized;
        
        if (isRotating)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            
        }
    }
}
