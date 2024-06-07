using UnityEngine;

public class PropellerRotation : MonoBehaviour
{
    public float rotationSpeed = 1000f;
    public bool isRotating { get; private set; } = false; 

    public void StartRotation()
    {
        isRotating = true;
    }

    public void StopRotation()
    {
        isRotating = false;
    }

    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }
}
