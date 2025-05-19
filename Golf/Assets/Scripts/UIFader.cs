using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFader : MonoBehaviour
{
    public Image targetImage; // Assign this in the Inspector
    public float fadeDuration = 1f;

    public void StartFade(Sprite newSprite)
    {
        targetImage.sprite = newSprite;

        // Make sure the image starts invisible
        Color startColor = targetImage.color;
        startColor.a = 0f;
        targetImage.color = startColor;

        // Start fade-in
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color color = targetImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            color.a = alpha;
            targetImage.color = color;
            yield return null;
        }

        // Ensure it's fully opaque
        color.a = 1f;
        targetImage.color = color;
    }
}
