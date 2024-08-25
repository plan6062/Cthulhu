using UnityEngine;

public class CandleLightEffect : MonoBehaviour
{
    public float minIntensity = 1f;
    public float maxIntensity = 7f;
    public float minRadius = 1f;
    public float maxRadius = 3f;
    public float flickerSpeed = 5f;

    private Light pointLight;
    private float baseIntensity;
    private float baseRadius;

    void Start()
    {
        pointLight = GetComponent<Light>();
        if (pointLight == null)
        {
            Debug.LogError("Point Light 컴포넌트를 찾을 수 없습니다.");
            enabled = false;
            return;
        }

        baseIntensity = (minIntensity + maxIntensity) / 2f;
        baseRadius = (minRadius + maxRadius) / 2f;
    }

    void Update()
    {
        // 부드러운 노이즈 생성
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0);
        
        // 빛의 강도 변화
        float intensityVariation = Mathf.Lerp(minIntensity, maxIntensity, noise);
        pointLight.intensity = Mathf.Lerp(baseIntensity, intensityVariation, 0.5f);

        // 빛의 범위(크기) 변화
        float radiusVariation = Mathf.Lerp(minRadius, maxRadius, noise);
        pointLight.range = Mathf.Lerp(baseRadius, radiusVariation, 0.5f);
    }
}