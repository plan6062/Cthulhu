using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RoundCorners : MonoBehaviour
{
    public float cornerRadius = 10f;

    void Start()
    {
        Image image = GetComponent<Image>();
        image.sprite = RoundedCornerSprite(cornerRadius);
    }

    Sprite RoundedCornerSprite(float cornerRadius)
    {
        int textureSize = 100;
        Texture2D texture = new Texture2D(textureSize, textureSize);

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), new Vector2(textureSize / 2, textureSize / 2));
                if (distance <= textureSize / 2 - cornerRadius || 
                    (x > cornerRadius && x < textureSize - cornerRadius) || 
                    (y > cornerRadius && y < textureSize - cornerRadius))
                {
                    texture.SetPixel(x, y, Color.white);
                }
                else
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }
        }

        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, textureSize, textureSize), new Vector2(0.5f, 0.5f));
    }
}