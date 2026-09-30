using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchivementsManager : MonoBehaviour
{
    public IntEvent OnAchievementEarned;

    [SerializeField] private Achievements[] achievements;

    [Header("Achievement Panel")]
    [SerializeField] private GameObject achievementPanel;

    public TMP_Text title;

    public static Dictionary<int, bool> AchievementStatus = new Dictionary<int, bool>();

    private void Start()
    {
        if (AchievementStatus.Count == 0)
        {
            AchievementStatus.Add(0, false);
            AchievementStatus.Add(1, false);
            AchievementStatus.Add(2, false);
            AchievementStatus.Add(3, false);
            AchievementStatus.Add(4, false);
            AchievementStatus.Add(5, false);
            AchievementStatus.Add(6, false);
            AchievementStatus.Add(7, false);
            AchievementStatus.Add(8, false);
            AchievementStatus.Add(9, false);
            AchievementStatus.Add(10, false);
            AchievementStatus.Add(11, false);
            AchievementStatus.Add(12, false);
        }
    }

    private void OnEnable()
    {
        OnAchievementEarned.AddListener(AchievementEarned);
    }

    private void OnDisable()
    {
        OnAchievementEarned.RemoveListener(AchievementEarned);
    }

    private void AchievementEarned(int achievementId)
    {
        title.text = achievements[achievementId].achievementTitle;
        achievementPanel.transform.Find("Arkaplan").GetComponent<Image>().sprite = achievements[achievementId].achievementSprite;
        achievementPanel.GetComponent<Animator>().SetTrigger("newMail");
        AchievementStatus[achievementId] = true;
    }
}