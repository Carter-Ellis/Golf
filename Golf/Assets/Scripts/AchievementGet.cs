using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementGet : MonoBehaviour
{
    static Animator anim;
    static TextMeshProUGUI title;
    static Inventory inv;
    void Start()
    {
        inv = FindObjectOfType<Inventory>();
        anim = transform.Find("Background").GetComponent<Animator>();
        title = GameObject.Find("Achievement Title Txt").GetComponent<TextMeshProUGUI>();
    }


    public static void PlayAchievementGet(Achievement.TYPE type)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.achievement, Camera.main.transform.position);
        anim.SetTrigger("ShowAchievement");
        title.text = Achievement.GetName(type);
    }

}
