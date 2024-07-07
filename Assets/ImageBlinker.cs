using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageBlinker : MonoBehaviour
{
    public Image targetImage;
    public float blinkInterval = 0.5f; // 깜빡이는 간격 (초)

    private Sprite originalSprite;

    void Start()
    {
        if (targetImage != null)
        {
            originalSprite = targetImage.sprite;
            StartCoroutine(BlinkRoutine());
        }
        else
        {
            Debug.LogError("Target Image is not assigned!");
        }
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            // 이미지 숨기기
            targetImage.sprite = null;
            yield return new WaitForSeconds(blinkInterval);

            // 이미지 보이기
            targetImage.sprite = originalSprite;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}