using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailManager : MonoBehaviour
{
    public IntEvent OnMailSend;

    [SerializeField] private Mail[] mails;
    [SerializeField] private List<GameObject> mailButtons = new List<GameObject>();
    [SerializeField] private AudioClip clickEffectClip;

    public GameObject mailButtonPrefab;
    public GameObject incomingBox;

    public TMP_Text mailSubject;
    public TMP_Text mailAddress;
    public TMP_Text mailBody;

    private void OnEnable()
    {
        OnMailSend.AddListener(InstantiateMailButton);
    }

    private void OnDisable()
    {
        OnMailSend.RemoveListener(InstantiateMailButton);
    }

    public void AssignMail(int buttonId)
    {
        mailSubject.text = mails[buttonId].mailSubject;
        mailAddress.text = mails[buttonId].mailAddress;
        mailBody.text = mails[buttonId].mailBody;
    }

    public void InstantiateMailButton(int mailId)
    {
        GameObject tempMailButton = Instantiate(mailButtonPrefab, incomingBox.transform);
        tempMailButton.GetComponent<Button>().onClick.AddListener(() => AssignMail(mailId));
        tempMailButton.GetComponent<Button>().onClick.AddListener(PlayClickEffect);
        tempMailButton.GetComponentInChildren<TMP_Text>().text = mails[mailId].mailSubject;
        mailButtons.Add(tempMailButton);
    }

    public void PlayClickEffect()
    {
        SoundFxManager.instance.PlaySoundFxClip(clickEffectClip, transform, 0.2f);
    }
}