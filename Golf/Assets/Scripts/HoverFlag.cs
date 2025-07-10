using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HoverFlag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Inventory inv;

    [SerializeField] private Image coin1;
    [SerializeField] private Image coin2;
    [SerializeField] private Image coin3;
    [SerializeField] private Sprite goldCoin;
    [SerializeField] private Sprite grayCoin;
    private float fadeDuration = 0.08f;

    public int levelNum = 1;

    private Coroutine fadeCoroutine;

    private void Start()
    {
        inv = FindObjectOfType<Inventory>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerEnter;
        if (obj == null) { return; }

        UnityEngine.UI.Button btn = obj.GetComponentInParent<UnityEngine.UI.Button>();
        if (btn == null || !btn.interactable) { return; }

        // Set correct sprites
        SetCoinSprite(coin1, 1);
        SetCoinSprite(coin2, 2);
        SetCoinSprite(coin3, 3);

        // Fade in
        StartFade(Color.white);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Fade out
        StartFade(Color.clear);
    }

    private void SetCoinSprite(Image image, int coinNum)
    {
        MapData mapData = Map.get(MainMenu.selectedMap);
        bool hasCoin = mapData.isCoinCollected(levelNum, coinNum);
        image.sprite = hasCoin ? goldCoin : grayCoin;
    }

    private void StartFade(Color targetColor)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeCoins(targetColor));
    }

    private IEnumerator FadeCoins(Color targetColor)
    {
        Image[] coins = { coin1, coin2, coin3 };
        float elapsed = 0f;

        Color[] startColors = new Color[coins.Length];
        for (int i = 0; i < coins.Length; i++)
        {
            startColors[i] = coins[i].color;
        }

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);

            for (int i = 0; i < coins.Length; i++)
            {
                Color newColor = Color.Lerp(startColors[i], targetColor, t);
                coins[i].color = newColor;
            }

            yield return null;
        }

        for (int i = 0; i < coins.Length; i++)
        {
            coins[i].color = targetColor;
        }
    }
}
