using TMPro;
using UnityEngine;

public class OptionButtonController : MonoBehaviour
{
    public static string newSentence;

    [SerializeField] private IntEvent onSentenceChanged;
    [SerializeField] private int relatedSentenceId;

    public void AssignSentence()
    {
        newSentence = GetComponentInChildren<TMP_Text>().text;
        TextEditor.clickedText.GetComponent<TMP_Text>().text = newSentence;
        UIManager.optionButtonsOn = false;

        DisableThisObjectParent();
        onSentenceChanged.Raise(relatedSentenceId);
        NewsActions.OnOptionButtonsOff();
    }

    private void DisableThisObjectParent()
    {
        transform.parent.gameObject.SetActive(false);
    }
}