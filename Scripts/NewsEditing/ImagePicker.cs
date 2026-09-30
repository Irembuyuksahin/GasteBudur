using UnityEngine;
using UnityEngine.UI;

public class ImagePicker : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite normalImage;
    [SerializeField] private Sprite wordSearchImage;
    [SerializeField] private Sprite crissCrossImage;
    [SerializeField] private Sprite sudokuImage;
    [SerializeField] private Sprite muzCumhuriyetiAd;
    [SerializeField] private Sprite cikolatyaAd;

    [Header("Panel")]
    [SerializeField] private GameObject currentPanel;
    [SerializeField] private GameObject text;

    [Header("Button ID")]
    [SerializeField] private int buttonId;

    public void ChangePanelImage()
    {
        text.SetActive(false);

        switch (buttonId)
        {
            case 0:
                currentPanel.GetComponent<Image>().sprite = wordSearchImage;
                break;
            case 1:
                currentPanel.GetComponent<Image>().sprite = crissCrossImage;
                break;
            case 2:
                currentPanel.GetComponent<Image>().sprite = sudokuImage;
                break;
            case 3:
                currentPanel.GetComponent<Image>().sprite = muzCumhuriyetiAd;
                break;
            case 4:
                currentPanel.GetComponent<Image>().sprite = cikolatyaAd;
                break;
        }
    }

    public void ResetState()
    {
        currentPanel.GetComponent<Image>().sprite = normalImage;
        text.SetActive(true);
    }

    public void TurnOffButtons()
    {
        transform.parent.gameObject.SetActive(false);
    }
}