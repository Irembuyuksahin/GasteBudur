using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class RuleBook : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject[] rules;
    [SerializeField] private AudioClip highlightEffectClip;

    public static GameObject pickedRule;
    public static bool isPickable = false;
    public static bool isPicked = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isPickable)
        {
            if (rules.Contains(eventData.pointerEnter))
            {
                if (pickedRule != null)
                {
                    pickedRule.GetComponent<Rules>().DisableHighlight();
                }

                pickedRule = eventData.pointerEnter;

                SoundFxManager.instance.PlaySoundFxClip(highlightEffectClip, transform, 1f);
                pickedRule.GetComponent<Rules>().MakeHighlight();

                isPicked = true;
                NewsActions.OnRuleClicked();
            }
        }
    }

    private void OnEnable()
    {
        NewsActions.OnControlButtonClicked += IsPickableTrue;
    }

    private void OnDisable()
    {
        NewsActions.OnControlButtonClicked -= IsPickableTrue;
    }

    private void IsPickableTrue()
    {
        isPickable = true;
    }

    public static void ResetState()
    {
        if (pickedRule != null)
        {
            pickedRule.GetComponent<Rules>().DisableHighlight();
        }

        isPickable = false;
        isPicked = false;
        pickedRule = null;
    }
}