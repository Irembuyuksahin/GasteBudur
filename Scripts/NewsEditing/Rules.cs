using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rules : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        NewsActions.OnBothSentencesPicked += DisableHighlight;
    }

    private void OnDisable()
    {
        NewsActions.OnBothSentencesPicked -= DisableHighlight;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MakeUnderline();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MakeNormal();
    }

    private void MakeUnderline()
    {
        textMeshProUGUI.fontStyle = FontStyles.Underline;
    }

    private void MakeNormal()
    {
        textMeshProUGUI.fontStyle = FontStyles.Normal;
    }

    public void MakeHighlight()
    {
        textMeshProUGUI.text = "<mark=#E0BA4D66><color=black>" + textMeshProUGUI.text + "</color></mark>";
    }

    public void DisableHighlight()
    {
        textMeshProUGUI.text = textMeshProUGUI.text.Replace("<mark=#E0BA4D66>", "").Replace("</mark>", "");
    }
}