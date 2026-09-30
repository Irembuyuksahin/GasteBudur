using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementMenu : MonoBehaviour
{
    [SerializeField] private Achievements[] achievements;
    [SerializeField] private GameObject[] achievementImages;

    private void Start()
    {
        for (int achievementId = 0; achievementId <= 12; achievementId++)
        {
            if (AchivementsManager.AchievementStatus[achievementId])
            {
                achievementImages[achievementId].GetComponent<Image>().sprite = achievements[achievementId].achievementSprite;
                achievementImages[achievementId].transform.Find("TitleText").GetComponent<TMP_Text>().text = achievements[achievementId].achievementTitle;
                achievementImages[achievementId].transform.Find("DescriptionText").GetComponent<TMP_Text>().text = achievements[achievementId].achievementDescription;
            }
        }
    }
}