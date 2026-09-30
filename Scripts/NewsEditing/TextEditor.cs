using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextEditor : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static GameObject clickedText;
    public static bool isSentencePickable = false;
    public static bool isSentencePicked = false;

    [Header("ID Number")]
    [SerializeField] private int sentenceId;

    [Header("Events")]
    [SerializeField] private IntEvent onSentenceChanged;

    [SerializeField] private AudioClip highlightEffectClip;

    private TextMeshProUGUI textMeshProUGUI;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        NewsActions.OnControlButtonClicked += IsPickableTrue;
        NewsActions.OnBothSentencesPicked += DisableHighlight;
        onSentenceChanged.AddListener(ChangeTag);
    }

    private void OnDisable()
    {
        NewsActions.OnControlButtonClicked -= IsPickableTrue;
        NewsActions.OnBothSentencesPicked -= DisableHighlight;
        onSentenceChanged.RemoveListener(ChangeTag);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSentencePickable)
        {
            if (!gameObject.CompareTag("Correct"))
            {
                if (!UIManager.optionButtonsOn)
                {
                    if (clickedText != null)
                    {
                        clickedText.GetComponent<TextEditor>().DisableHighlight();
                    }

                    clickedText = gameObject;

                    SoundFxManager.instance.PlaySoundFxClip(highlightEffectClip, transform, 1f);
                    MakeHighlight();

                    isSentencePicked = true;
                    NewsActions.OnSentenceClicked();
                }
            }
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

    public void ChangeTag(int id)
    {
        if (sentenceId == id)
        {
            gameObject.tag = "Correct";
        }
    }

    public static void ResetState()
    {
        if (clickedText != null)
        {
            clickedText.GetComponent<TextEditor>().DisableHighlight();
        }

        isSentencePickable = false;
        isSentencePicked = false;
        clickedText = null;
    }
}