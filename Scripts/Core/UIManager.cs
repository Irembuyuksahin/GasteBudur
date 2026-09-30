using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private GameObject controlButton;
    [SerializeField] private GameObject puzzleButtons;
    [SerializeField] private GameObject adButtons;

    [Header("Panels")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private GameObject[] newsPanels;
    [SerializeField] private GameObject deskPanel;
    [SerializeField] private GameObject mailPanel;

    [Header("Events")]
    [SerializeField] private IntEvent onAchievementEarned;
    [SerializeField] private IntEvent onMailSend;

    [SerializeField] private GameObject pcTransitionButton;
    [SerializeField] private GameObject deskExitButton;
    [SerializeField] private GameObject crosshairImage;
    [SerializeField] private GameObject newsPileObject;
    [SerializeField] private TextMeshProUGUI basketCounterText;

    [SerializeField] private AudioClip mailClip;
    [SerializeField] private AudioClip correctControlClip;
    [SerializeField] private AudioClip falseControlClip;

    public int FinishedNewsCount = 0;

    public static bool isChecking = false;
    public static bool optionButtonsOn;

    private bool isPuzzleButtonsOn = false;
    private bool isAdButtonsOn = false;

    private int mistakeCount = 0;

    private void OnEnable()
    {
        NewsActions.OnSentenceClicked += CheckBothPicked;
        NewsActions.OnRuleClicked += CheckBothPicked;
        NewsActions.OnSwiped += IncreaseNewsCount;
        NewsActions.OnMistake += SendMistakeMail;
        NewsActions.OnMistake += FirstMistakeAchievementCheck;
        NewsActions.OnDeskFocused += ActivateDeskPanel;
        NewsActions.OnDeskLeave += DeactivateDeskPanel;
        NewsActions.OnComputerFocused += ActivateMailPanel;
        NewsActions.OnComputerLeave += DeactivateMailPanel;
    }

    private void OnDisable()
    {
        NewsActions.OnSentenceClicked -= CheckBothPicked;
        NewsActions.OnRuleClicked -= CheckBothPicked;
        NewsActions.OnSwiped -= IncreaseNewsCount;
        NewsActions.OnMistake -= SendMistakeMail;
        NewsActions.OnMistake -= FirstMistakeAchievementCheck;
        NewsActions.OnDeskFocused -= ActivateDeskPanel;
        NewsActions.OnDeskLeave -= DeactivateDeskPanel;
        NewsActions.OnComputerFocused -= ActivateMailPanel;
        NewsActions.OnComputerLeave -= DeactivateMailPanel;
    }

    private void Update()
    {
        if (SwipeEffect.beingDragged)
        {
            if (FinishedNewsCount < 9)
            {
                newsPanels[FinishedNewsCount + 1].SetActive(true);
            }

            if (FinishedNewsCount == 9)
            {
                newsPileObject.SetActive(false);
            }
        }
        else
        {
            if (FinishedNewsCount < 9)
            {
                newsPanels[FinishedNewsCount + 1].SetActive(false);
            }

            if (FinishedNewsCount != 10)
            {
                newsPileObject.SetActive(true);
            }
        }

        if (FinishedNewsCount == 6 && !isPuzzleButtonsOn)
        {
            puzzleButtons.SetActive(true);
            isPuzzleButtonsOn = true;
        }

        if (FinishedNewsCount == 3 && !isAdButtonsOn)
        {
            adButtons.SetActive(true);
            isAdButtonsOn = true;
        }

        if (isChecking || CompareRuleAndSentence())
        {
            pcTransitionButton.GetComponent<Button>().interactable = false;
            deskExitButton.GetComponent<Button>().interactable = false;
            controlButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            pcTransitionButton.GetComponent<Button>().interactable = true;
            deskExitButton.GetComponent<Button>().interactable = true;
            controlButton.GetComponent<Button>().interactable = true;
        }

        basketCounterText.text = TrashBinManager.basketCount.ToString();
    }

    public void CheckBothPicked()
    {
        if (RuleBook.isPicked && TextEditor.isSentencePicked)
        {
            NewsActions.OnMarkerReturn();
            StartCoroutine(CheckBothPickedRoutine(CompareRuleAndSentence()));
        }
    }

    public void NewSentenceButtonsOn()
    {
        if (CompareRuleAndSentence())
        {
            optionButtonsOn = true;
            TextEditor.clickedText.transform.Find("Options").gameObject.SetActive(true);
            NewsActions.OnOptionButtonsOn();
        }
    }

    public void IncreaseNewsCount()
    {
        FinishedNewsCount++;
        HandleMails();
        NewsActions.OnNewsCountIncreased();
        NoMistakeAchievementCheck();
    }

    private bool CompareRuleAndSentence()
    {
        if (RuleBook.pickedRule == null || TextEditor.clickedText == null)
        {
            return false;
        }

        return RuleBook.pickedRule.tag == TextEditor.clickedText.tag;
    }

    private IEnumerator CheckBothPickedRoutine(bool isCorrect)
    {
        NewsActions.OnCheckBothPicked();
        isChecking = true;

        RuleBook.isPickable = false;
        TextEditor.isSentencePickable = false;

        yield return new WaitForSeconds(0.7f);

        SoundFxManager.instance.PlaySoundFxClip(isCorrect ? correctControlClip : falseControlClip, transform, 1f);

        yield return new WaitForSeconds(2f);

        NewsActions.OnBothSentencesPicked();
        NewSentenceButtonsOn();

        RuleBook.isPicked = false;
        TextEditor.isSentencePicked = false;

        isChecking = false;
        NewsActions.OnCheckBothPicked();
    }

    public void NotificationAnimPlay()
    {
        SoundFxManager.instance.PlaySoundFxClip(mailClip, transform, 1f);
        notificationPanel.GetComponent<Animator>().SetTrigger("newMail");
    }

    private void HandleMails()
    {
        switch (FinishedNewsCount)
        {
            case 1:
                StartCoroutine(SendMailWithDelay(4));
                break;
            case 3:
                StartCoroutine(SendMailWithDelay(2));
                break;
            case 6:
                StartCoroutine(SendMailWithDelay(3));
                break;
            case 10:
                onMailSend.Raise(5);
                NotificationAnimPlay();
                break;
        }
    }

    private IEnumerator SendMailWithDelay(int mailId)
    {
        float randomDelay = Random.Range(3f, 10f);
        yield return new WaitForSeconds(randomDelay);

        onMailSend.Raise(mailId);
        NotificationAnimPlay();

        if (mailId == 3)
        {
            NewsActions.OnPaperTaskMail();
        }

        if (mailId == 2)
        {
            NewsActions.OnWaterTaskMail();
        }
    }

    private void SendMistakeMail()
    {
        onMailSend.Raise(0);
        NotificationAnimPlay();
    }

    private void FirstMistakeAchievementCheck()
    {
        mistakeCount++;

        if (mistakeCount == 1)
        {
            onAchievementEarned.Raise(5);
        }
    }

    private void NoMistakeAchievementCheck()
    {
        if (mistakeCount == 0 && FinishedNewsCount == 10)
        {
            onAchievementEarned.Raise(4);
        }
    }

    private void ActivateDeskPanel()
    {
        deskPanel.SetActive(true);
        crosshairImage.SetActive(false);
    }

    private void DeactivateDeskPanel()
    {
        deskPanel.SetActive(false);
        crosshairImage.SetActive(true);
    }

    private void ActivateMailPanel()
    {
        mailPanel.SetActive(true);
        crosshairImage.SetActive(false);
    }

    private void DeactivateMailPanel()
    {
        mailPanel.SetActive(false);
    }
}