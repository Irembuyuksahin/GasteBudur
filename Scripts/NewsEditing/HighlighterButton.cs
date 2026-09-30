using UnityEngine;
using UnityEngine.UI;

public class HighlighterButton : MonoBehaviour
{
    [SerializeField] private GameObject uncappedMarker;
    [SerializeField] private AudioClip markerCapOpenClip;
    [SerializeField] private AudioClip markerCapCloseClip;

    private GameObject tempMarker;

    private void OnEnable()
    {
        NewsActions.OnMarkerReturn += ButtonReturn;
        NewsActions.OnMarkerReturn += PlayMarkerCloseClip;
        NewsActions.OnOptionButtonsOn += InteractableFalse;
        NewsActions.OnOptionButtonsOff += InteractableTrue;
    }

    private void OnDisable()
    {
        NewsActions.OnMarkerReturn -= ButtonReturn;
        NewsActions.OnMarkerReturn -= PlayMarkerCloseClip;
        NewsActions.OnOptionButtonsOn -= InteractableFalse;
        NewsActions.OnOptionButtonsOff -= InteractableTrue;
    }

    public void ControlButtonPressed()
    {
        NewsActions.OnControlButtonClicked();
        tempMarker = Instantiate(uncappedMarker, Input.mousePosition, uncappedMarker.transform.rotation, transform.parent);
        GetComponent<Button>().interactable = false;
        GetComponent<Image>().enabled = false;
    }

    public void ButtonReturn()
    {
        GetComponent<Button>().interactable = true;
        GetComponent<Image>().enabled = true;
        Destroy(tempMarker);
        tempMarker = null;
    }

    public void PickableOff()
    {
        RuleBook.ResetState();
        TextEditor.ResetState();
    }

    public void InteractableFalse()
    {
        GetComponent<Button>().interactable = false;
    }

    private void InteractableTrue()
    {
        GetComponent<Button>().interactable = true;
    }

    public void PlayMarkerOpenClip()
    {
        SoundFxManager.instance.PlaySoundFxClip(markerCapOpenClip, transform, 0.5f);
    }

    public void PlayMarkerCloseClip()
    {
        SoundFxManager.instance.PlaySoundFxClip(markerCapCloseClip, transform, 1f);
    }
}