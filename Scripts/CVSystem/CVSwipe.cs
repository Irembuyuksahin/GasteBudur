using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CVSwipe : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 initialPos;

    private float distanceMoved;
    private float minXValue = -1600f;
    private float maxXValue = 1600f;

    private bool isDragable = true;

    public static bool beingDragged = false;

    [SerializeField] private AudioClip swipeEffectClip;
    [SerializeField] private DoubleIntEvent onCVApproved;

    private void OnEnable()
    {
        NewsActions.OnCheckBothPickedFalse += IsDragableTrue;
        NewsActions.OnCheckBothPickedTrue += IsDragableFalse;
    }

    private void OnDisable()
    {
        NewsActions.OnCheckBothPickedFalse -= IsDragableTrue;
        NewsActions.OnCheckBothPickedTrue -= IsDragableFalse;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragable && !CVTextScript.isSentencePickable)
        {
            transform.localPosition = new Vector2(Mathf.Clamp(transform.localPosition.x + eventData.delta.x, minXValue, maxXValue), transform.localPosition.y);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPos = transform.localPosition;
        beingDragged = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        distanceMoved = Mathf.Abs(transform.localPosition.x - initialPos.x);

        if (distanceMoved <= 850f)
        {
            transform.localPosition = initialPos;
        }
        else
        {
            if (transform.localPosition.x < -850f)
            {
                bool isFound = false;

                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).CompareTag("Rule1") ||
                        transform.GetChild(i).CompareTag("Rule2") ||
                        transform.GetChild(i).CompareTag("Rule3"))
                    {
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    NewsActions.OnEthicsDecrease();
                }

                NewsActions.OnSwipeRejected();
                NewsActions.OnSwiped();
            }
            else if (transform.localPosition.x > 850f)
            {
                int expectation = int.Parse(transform.Find("Maas Beklentisi Degeri").GetComponent<TMP_Text>().text);
                int skillRating = int.Parse(transform.Find("Beceri Yýldýz").tag);
                onCVApproved.Raise(expectation, skillRating);

                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).CompareTag("Rule1") ||
                        transform.GetChild(i).CompareTag("Rule2") ||
                        transform.GetChild(i).CompareTag("Rule3"))
                    {
                        NewsActions.OnEthicsDecrease();
                        break;
                    }
                }

                NewsActions.OnSwipeAccepted();
                NewsActions.OnSwiped();
            }

            SoundFxManager.instance.PlaySoundFxClip(swipeEffectClip, transform, 1f);
            gameObject.SetActive(false);
        }

        beingDragged = false;
    }

    public void IsDragable()
    {
        isDragable = !isDragable;
    }

    public void IsDragableFalse()
    {
        isDragable = false;
    }

    public void IsDragableTrue()
    {
        isDragable = true;
    }
}