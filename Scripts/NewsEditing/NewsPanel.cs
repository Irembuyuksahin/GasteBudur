using UnityEngine;

public class NewsPanel : MonoBehaviour
{
    public void CheckNewsPanel()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).CompareTag("BasicNews") &&
                !transform.GetChild(i).CompareTag("Correct") &&
                !transform.GetChild(i).CompareTag("Subhead"))
            {
                NewsActions.OnMistake();
                break;
            }
        }
    }
}