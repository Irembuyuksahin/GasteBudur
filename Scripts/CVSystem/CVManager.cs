using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CVManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private GameObject controlButton;

    [Header("Panels")]
    [SerializeField] private GameObject[] cvPanels;
    [SerializeField] private GameObject notificationPanel;

    [Header("Objects")]
    [SerializeField] private GameObject cvPanel;
    [SerializeField] private GameObject mailPanel;
    [SerializeField] private GameObject pcTransitionButton;
    [SerializeField] private GameObject deskExitButton;
    [SerializeField] private GameObject crosshairImage;
    [SerializeField] private GameObject yinObject;
    [SerializeField] private GameObject cvPileObject;
    [SerializeField] private TextMeshProUGUI basketCounterText;

    [Header("Audio")]
    [SerializeField] private AudioClip mailClip;
    [SerializeField] private AudioClip correctControlClip;
    [SerializeField] private AudioClip falseControlClip;

    [Header("Events")]
    [SerializeField] private IntEvent onMailSend;
    [SerializeField] private IntEvent onAchievementEarned;
    [SerializeField] private IntEvent onGiftEarned;

    public int FinishedCVCount = 0;
    public static bool isChecking = false;

    private bool isMoneyLost = false;
    private bool isCVDone = false;

    private void OnEnable()
    {
        NewsActions.OnSentenceClicked += CheckBothPicked;
        NewsActions.OnRuleClicked += CheckBothPicked;
        NewsActions.OnSwiped += IncreaseCVCount;
        NewsActions.OnDeskFocused += ActivateDeskPanel;
        NewsActions.OnDeskLeave += DeactivateDeskPanel;
        NewsActions.OnComputerFocused += ActivateMailPanel;
        NewsActions.OnComputerLeave += DeactivateMailPanel;
        NewsActions.OnSwipeAccepted += HandleAcceptMails;
        NewsActions.OnSwipeRejected += HandleRejectMails;
    }

    private void OnDisable()
    {
        NewsActions.OnSentenceClicked -= CheckBothPicked;
        NewsActions.OnRuleClicked -= CheckBothPicked;
        NewsActions.OnSwiped -= IncreaseCVCount;
        NewsActions.OnDeskFocused -= ActivateDeskPanel;
        NewsActions.OnDeskLeave -= DeactivateDeskPanel;
        NewsActions.OnComputerFocused -= ActivateMailPanel;
        NewsActions.OnComputerLeave -= DeactivateMailPanel;
        NewsActions.OnSwipeAccepted -= HandleAcceptMails;
        NewsActions.OnSwipeRejected -= HandleRejectMails;
    }

    private void Update()
    {
        if (FinishedCVCount < 15)
        {
            cvPanels[FinishedCVCount].SetActive(true);
        }

        if (CVSwipe.beingDragged)
        {
            if (FinishedCVCount < 14)
            {
                cvPanels[FinishedCVCount + 1].SetActive(true);
            }

            if (FinishedCVCount == 14)
            {
                cvPileObject.SetActive(false);
            }
        }
        else
        {
            if (FinishedCVCount < 14)
            {
                cvPanels[FinishedCVCount + 1].SetActive(false);
            }

            if (FinishedCVCount != 15)
            {
                cvPileObject.SetActive(true);
            }
        }

        if (FinishedCVCount == 15 && !isCVDone)
        {
            if (MoneyScript.moneyAmount < 15000 && !isMoneyLost)
            {
                NewsActions.OnEthicsDecrease();
                isMoneyLost = true;
            }

            if (EthicsBar.ethicsValue < 50)
            {
                onAchievementEarned.Raise(8);
            }
            else if (EthicsBar.ethicsValue > 50)
            {
                onAchievementEarned.Raise(9);
            }

            yinObject.SetActive(true);
            isCVDone = true;
        }

        if (isChecking)
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
        if (CVRuleBook.isPicked && CVTextScript.isSentencePicked)
        {
            NewsActions.OnMarkerReturn();
            StartCoroutine(CheckBothPickedRoutine(CompareRuleAndSentence()));
        }
    }

    private IEnumerator CheckBothPickedRoutine(bool isCorrect)
    {
        isChecking = true;
        NewsActions.OnCheckBothPickedTrue();

        CVRuleBook.isPickable = false;
        CVTextScript.isSentencePickable = false;

        yield return new WaitForSeconds(0.7f);

        SoundFxManager.instance.PlaySoundFxClip(isCorrect ? correctControlClip : falseControlClip, transform, 1f);

        yield return new WaitForSeconds(2f);

        NewsActions.OnBothSentencesPicked();

        CVRuleBook.isPicked = false;
        CVTextScript.isSentencePicked = false;

        isChecking = false;
        NewsActions.OnCheckBothPickedFalse();
    }

    private bool CompareRuleAndSentence()
    {
        if (CVRuleBook.pickedRule == null || CVTextScript.clickedText == null)
        {
            return false;
        }

        return CVRuleBook.pickedRule.tag == CVTextScript.clickedText.tag;
    }

    public void IncreaseCVCount()
    {
        FinishedCVCount++;
        HandleMails();
    }

    public void NotificationAnimPlay()
    {
        SoundFxManager.instance.PlaySoundFxClip(mailClip, transform, 1f);
        notificationPanel.GetComponent<Animator>().SetTrigger("newMail");
    }

    private void HandleMails()
    {
        switch (FinishedCVCount)
        {
            case 3:
                SendMail(1);
                break;
            case 4:
                SendMail(2);
                break;
            case 9:
                SendMail(3);
                break;
        }
    }

    private void HandleAcceptMails()
    {
        switch (FinishedCVCount)
        {
            case 4:
                SendMail(4);
                onAchievementEarned.Raise(10);
                onGiftEarned.Raise(0);
                break;
            case 5:
                SendMail(6);
                onAchievementEarned.Raise(11);
                onGiftEarned.Raise(1);
                break;
            case 10:
                SendMail(8);
                onAchievementEarned.Raise(12);
                onGiftEarned.Raise(2);
                break;
        }
    }

    private void HandleRejectMails()
    {
        switch (FinishedCVCount)
        {
            case 4:
                SendMail(5);
                break;
            case 5:
                SendMail(7);
                break;
            case 10:
                SendMail(9);
                break;
        }
    }

    private void SendMail(int mailId)
    {
        onMailSend.Raise(mailId);
        NotificationAnimPlay();
    }

    private void ActivateDeskPanel()
    {
        cvPanel.SetActive(true);
        crosshairImage.SetActive(false);
    }

    private void DeactivateDeskPanel()
    {
        cvPanel.SetActive(false);
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