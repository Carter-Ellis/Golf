using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementGet : MonoBehaviour
{
    private class Banner
    {
        public Animator anim;
        public TextMeshProUGUI title;
        public Image image;
    }

    private static Banner achieveBanner = new Banner();
    private static Banner hatBanner = new Banner();

    void Start()
    {

        achieveBanner = getBanner("AchievementBanner");
        hatBanner = getBanner("HatBanner");

    }

    private Banner getBanner(string name)
    {
        Banner result = new Banner();
        GameObject banner = transform.Find(name).gameObject;
        result.anim = banner.GetComponent<Animator>();
        result.title = banner.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        result.image = banner.transform.Find("Image").GetComponent<Image>();
        return result;
    }


    public static void PlayAchievementGet(Achievement.TYPE type)
    {
        Audio.playSFX(FMODEvents.instance.achievement, Camera.main.transform.position);
        achieveBanner.anim.SetTrigger("ShowAchievement");
        achieveBanner.title.text = Achievement.GetName(type);
    }

    public static void PlayHatGet(Hat.TYPE type)
    {
        Audio.playSFX(FMODEvents.instance.secretCoin, Camera.main.transform.position);
        hatBanner.anim.SetTrigger("ShowHat");
        hatBanner.title.text = Hat.getName(type);
        hatBanner.image.sprite = Hat.getSprite(type);
        hatBanner.image.GetComponent<Animator>().runtimeAnimatorController = Hat.getAnimator(type, true);

    }

}
