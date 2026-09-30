using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeEffect : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private NewsPanel newsPanel;
    private Vector3 initialPos;

    private float distanceMoved;
    private float minXValue = 0f;
    private float maxXValue = 1250f;

    private bool isDragable = true;

    public static bool beingDragged = false;

    [SerializeField] private AudioClip swipeEffectClip;

    private void Start()
    {
        newsPanel = GetComponent<NewsPanel>();
    }

    private void OnEnable()
    {
        NewsActions.OnOptionButtonsOn += IsDragable;
        NewsActions.OnOptionButtonsOff += IsDragable;
        NewsActions.OnCheckBothPicked += IsDragable;
    }

    private void OnDisable()
    {
        NewsActions.OnOptionButtonsOn -= IsDragable;
        NewsActions.OnOptionButtonsOff -= IsDragable;
        NewsActions.OnCheckBothPicked -= IsDragable;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragable && !TextEditor.isSentencePickable)
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

        if (distanceMoved < 650f)
        {
            transform.localPosition = initialPos;
        }
        else
        {
            SoundFxManager.instance.PlaySoundFxClip(swipeEffectClip, transform, 1f);

            gameObject.SetActive(false);
            newsPanel.CheckNewsPanel();
            NewsActions.OnSwiped();
        }

        beingDragged = false;
    }

    public void IsDragable()
    {
        isDragable = !isDragable;
    }
}