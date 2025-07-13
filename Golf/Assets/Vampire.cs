using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Vampire : MonoBehaviour
{
    [SerializeField] private Image ballImage;
    private bool isFading = false;

    public void onClick()
    {
        if (!isFading)
        {
            Inventory inv = FindObjectOfType<Inventory>();
            if (!inv.achievements[(int)Achievement.TYPE.VAMPIRE])
            {
                Achievement.Give(Achievement.TYPE.VAMPIRE);
                inv.SavePlayer();
            }
            Audio.playSFX(FMODEvents.instance.windAbility);
            StartCoroutine(FadeOutPauseIn(ballImage, 1f, 5f));
        }
    }

    private IEnumerator FadeOutPauseIn(Image image, float fadeDuration, float pauseDuration)
    {
        isFading = true;
        Color originalColor = image.color;

        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Wait while invisible
        yield return new WaitForSeconds(pauseDuration);

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        image.color = originalColor;
        isFading = false;
    }
}
