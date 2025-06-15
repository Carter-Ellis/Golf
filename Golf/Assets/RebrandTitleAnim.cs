using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RebrandTitleAnim : MonoBehaviour
{
    public Image[] letterImages;
    public float amplitude = 20f;
    public float frequency = 2f;
    public float waveSpeed = 2f;

    private Vector2[] originalPositions;

    void Start()
    {
        originalPositions = new Vector2[letterImages.Length];
        for (int i = 0; i < letterImages.Length; i++)
        {
            originalPositions[i] = letterImages[i].rectTransform.anchoredPosition;
        }
    }

    void Update()
    {
        float time = Time.time * waveSpeed;
        for (int i = 0; i < letterImages.Length; i++)
        {
            float offsetY = Mathf.Sin(time + i * frequency) * amplitude;
            Vector2 newPos = originalPositions[i] + Vector2.up * offsetY;
            letterImages[i].rectTransform.anchoredPosition = newPos;
        }
    }
}
