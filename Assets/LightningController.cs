using UnityEngine;
using System.Collections;

public class LightningController : Actor
{
    public Light directionalLight;
    public float lightningInterval = 5f;
    public float flashDuration = 0.2f;
    public float lightIntensity = 1f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] thunderSounds;
    [Range(0f, 2f)]
    public float thunderDelay = 0.5f;
    [Range(0f, 1f)]
    public float volumeMin = 0.5f;
    [Range(0f, 1f)]
    public float volumeMax = 1f;

    private float originalIntensity;

    private void Start()
    {
        SetupLight();
        SetupAudio();
        StartCoroutine(LightningFlash());
    }

    private void SetupLight()
    {
        if (directionalLight == null)
        {
            directionalLight = GetComponent<Light>();
        }

        if (directionalLight != null)
        {
            originalIntensity = directionalLight.intensity;
            directionalLight.intensity = 0;
        }
        else
        {
            Debug.LogError("directional light 할당 필요");
        }
    }

    private void SetupAudio()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (thunderSounds == null || thunderSounds.Length == 0)
        {
            Debug.LogWarning("오디오 할당 필요");
        }
    }

    private IEnumerator LightningFlash()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(lightningInterval * 0.5f, lightningInterval * 1.5f));
            
            // 천둥 칠 때
            directionalLight.intensity = lightIntensity;
            StartCoroutine(PlayThunderSound());
            yield return new WaitForSeconds(flashDuration);
            
            // 없을 때
            directionalLight.intensity = 0;
            
            // 여러번 천둥 치는 효과 추가
            if (Random.value > 0.5f)
            {
                yield return new WaitForSeconds(0.1f);
                directionalLight.intensity = lightIntensity * 0.5f;
                yield return new WaitForSeconds(flashDuration * 0.5f);
                directionalLight.intensity = 0;
            }
        }
    }

    private IEnumerator PlayThunderSound()
    {
        if (thunderSounds != null && thunderSounds.Length > 0 && audioSource != null)
        {
            yield return new WaitForSeconds(thunderDelay);
            
            AudioClip randomThunder = thunderSounds[Random.Range(0, thunderSounds.Length)];
            audioSource.clip = randomThunder;
            audioSource.volume = Random.Range(volumeMin, volumeMax);
            audioSource.Play();
        }
    }

    private void OnDisable()
    {
        // Restore original intensity when the script is disabled
        if (directionalLight != null)
        {
            directionalLight.intensity = originalIntensity;
        }
    }
}