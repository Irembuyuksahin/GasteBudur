using UnityEngine;

[CreateAssetMenu(fileName = "Achievements", menuName = "Scriptable Objects/Achievements")]
public class Achievements : ScriptableObject
{
    public int achievementId;
    public string achievementTitle;
    public Sprite achievementSprite;

    [TextArea(3, 10)]
    public string achievementDescription;
}