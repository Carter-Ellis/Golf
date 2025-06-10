using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    public GameObject achievementPrefab;
    public Transform contentParent;
    public Sprite goldStar;
    public Sprite grayStar;
    Inventory inv;
    private List<GameObject> entries = new List<GameObject>();
    void Start()
    {
        inv = FindObjectOfType<Inventory>();
        for (int i = 0; i < (int)Achievement.TYPE.MAX; i++)
        {
            entries.Add(Instantiate(achievementPrefab, contentParent));
        }
    }

    public void LoadAchievements()
    {
        for (int i = 0; i < (int)Achievement.TYPE.MAX; i++)
        {
            GameObject titleObj = entries[i].transform.Find("Achievement Title")?.gameObject;
            GameObject descObj = entries[i].transform.Find("Achievement Description")?.gameObject;
            titleObj.GetComponent<TextMeshProUGUI>().text = Achievement.GetName((Achievement.TYPE)i);
            descObj.GetComponent<TextMeshProUGUI>().text = Achievement.GetDescription((Achievement.TYPE)i);

            if (inv.achievements[i])
            {
                entries[i].GetComponentInChildren<Image>().sprite = goldStar;
            }
            else
            {
                entries[i].GetComponentInChildren<Image>().sprite = grayStar;
            }

        }
    }
}
