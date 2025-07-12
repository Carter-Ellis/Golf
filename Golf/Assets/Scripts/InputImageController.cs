using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputImageController : MonoBehaviour
{

    public Image[] images;

    void Start()
    {
        updateSprites();

    }

    public void OnControllerChange()
    {
        updateSprites();
    }

    private void updateSprites()
    {
        foreach (Image img in images)
        {
            if (img == null) { continue; }
            PlayerInput.Axis type = PlayerInput.getType(img.gameObject.name);
            Sprite sprite = PlayerInput.getSprite(type);
            float ratio = sprite.bounds.size.x / sprite.bounds.size.y;
            RectTransform rect = img.GetComponent<RectTransform>();
            Vector2 size = rect.sizeDelta;
            size.x = ratio * size.y;
            rect.sizeDelta = size;
            img.sprite = sprite;

        }
    }

}
