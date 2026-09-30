using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CVTextScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static GameObject clickedText;
    public static bool isSentencePickable = false;
    public static bool isSentencePicked = false;

    private TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private AudioClip highlightEffectClip;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        NewsActions.OnControlButtonClicked += IsPickableTrue;
        NewsActions.OnBothSentencesPicked += DisableHighlight;
    }

    private void OnDisable()
    {
        NewsActions.OnControlButtonClicked -= IsPickableTrue;
        NewsActions.OnBothSentencesPicked -= DisableHighlight;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSentencePickable)
        {
            if (clickedText != null)
            {
                clickedText.GetComponent<CVTextScript>().DisableHighlight();
            }

            clickedText = gameObject;

            MakeHighlight();
            SoundFxManager.instance.PlaySoundFxClip(highlightEffectClip, transform, 1f);

            isSentencePicked = true;
            NewsActions.OnSentenceClicked();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!gameObject.CompareTag("Correct"))
        {
            MakeUnderline();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MakeNormal();
    }

    private void MakeUnderline()
    {
        textMeshProUGUI.fontStyle = FontStyles.Underline;
    }

    private void MakeHighlight()
    {
        textMeshProUGUI.text = "<mark=#E0BA4D66><color=black>" + textMeshProUGUI.text + "</color></mark>";
    }

    public void DisableHighlight()
    {
        textMeshProUGUI.text = textMeshProUGUI.text.Replace("<mark=#E0BA4D66>", "").Replace("</mark>", "");
    }

    private void MakeNormal()
    {
        textMeshProUGUI.fontStyle = FontStyles.Normal;
    }

    private void IsPickableTrue()
    {
        isSentencePickable = true;
    }

    public static void ResetState()
    {
        if (clickedText != null)
        {
            clickedText.GetComponent<CVTextScript>().DisableHighlight();
        }

        isSentencePickable = false;
        isSentencePicked = false;
        clickedText = null;
    }
}