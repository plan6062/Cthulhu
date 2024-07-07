using UnityEngine;

public class CRTEffectSprite : MonoBehaviour
{
    public Material crtMaterial;
    public float scanlineIntensity = 0.1f;
    public float scanlineCount = 100f;
    public Color greenTint = new Color(0, 1, 0, 0.5f);

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = new Material(crtMaterial);
    }

    void Update()
    {
        spriteRenderer.material.SetFloat("_ScanlineIntensity", scanlineIntensity);
        spriteRenderer.material.SetFloat("_ScanlineCount", scanlineCount);
        spriteRenderer.material.SetColor("_GreenTint", greenTint);
    }
}