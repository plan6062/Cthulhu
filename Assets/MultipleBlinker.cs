using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MultipleBlinker : MonoBehaviour
{
    [System.Serializable]
    public class BlinkingImage
    {
        public Image image;
        public float blinkSpeed = 2f;
        [Range(0, 1)]
        public float minAlpha = 0f;
    }

    public List<BlinkingImage> blinkingImages = new List<BlinkingImage>();
    private Dictionary<Image, Color> originalColors = new Dictionary<Image, Color>();

    void Start()
    {
        foreach (var blinkingImage in blinkingImages)
        {
            if (blinkingImage.image != null)
            {
                originalColors[blinkingImage.image] = blinkingImage.image.color;
            }
        }
    }

    void Update()
    {
        foreach (var blinkingImage in blinkingImages)
        {
            if (blinkingImage.image != null)
            {
                float alpha = Mathf.PingPong(Time.time * blinkingImage.blinkSpeed, 1f);
                alpha = Mathf.Lerp(blinkingImage.minAlpha, 1f, alpha);
                Color newColor = originalColors[blinkingImage.image];
                newColor.a = alpha;
                blinkingImage.image.color = newColor;
            }
        }
    }

    void OnDisable()
    {
        foreach (var blinkingImage in blinkingImages)
        {
            if (blinkingImage.image != null)
            {
                blinkingImage.image.color = originalColors[blinkingImage.image];
            }
        }
    }
}