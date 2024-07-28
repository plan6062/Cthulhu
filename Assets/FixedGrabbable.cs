using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FixedGrabbable : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isGrabbed = false;

    [SerializeField]
    private ParticleSystem debrisParticleSystem;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip grabSound; 

    private void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;
        grabInteractable.movementType = XRBaseInteractable.MovementType.Kinematic;
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        PlayEffects();
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        ResetTransform();
    }

    private void LateUpdate()
    {
        if (!isGrabbed)
        {
            ResetTransform();
        }
    }

    private void ResetTransform()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    private void PlayEffects()
    {
        // 파티클 효과 재생
        if (debrisParticleSystem != null)
        {
            debrisParticleSystem.Play();
        }

        // 오디오 효과 재생
        if (audioSource != null && grabSound != null)
        {
            audioSource.PlayOneShot(grabSound);
        }
    }
}