using UnityEngine;
using UnityEngine.UI;

public class EthicsBar : MonoBehaviour
{
    [Header("Ethics Value")]
    [Range(0, 100)]
    public static float ethicsValue = 100f;

    [Header("Images")]
    [SerializeField] private Image barImage;
    [SerializeField] private Image scale;
    [SerializeField] private Sprite rightScales;
    [SerializeField] private Sprite leftScales;
    [SerializeField] private Sprite balancedScales;

    [Header("Events")]
    [SerializeField] private IntEvent onAchievementEarned;

    private bool isEthicsDecreasedFirstTime = false;

    private Color redColor;
    private Color yellowColor;
    private Color greenColor;

    private void Awake()
    {
        redColor = new Color(0.6226415f, 0.2085262f, 0.2085262f);
        yellowColor = new Color(0.7169812f, 0.61586f, 0.2401211f);
        greenColor = new Color(0.3467449f, 0.6603774f, 0.2273941f);
    }

    private void OnEnable()
    {
        NewsActions.OnEthicsDecrease += DecreaseEthics;
    }

    private void OnDisable()
    {
        NewsActions.OnEthicsDecrease -= DecreaseEthics;
    }

    private void Update()
    {
        float normalized = ethicsValue / 100f;
        Color targetColor;

        if (normalized < 0.5f)
        {
            float t = normalized / 0.5f;
            targetColor = Color.Lerp(redColor, yellowColor, t);
        }
        else
        {
            float t = (normalized - 0.5f) / 0.5f;
            targetColor = Color.Lerp(yellowColor, greenColor, t);
        }

        barImage.color = targetColor;
        ChangeEthicsImage();
    }

    private void DecreaseEthics()
    {
        ethicsValue -= 20;

        if (!isEthicsDecreasedFirstTime)
        {
            onAchievementEarned.Raise(7);
            isEthicsDecreasedFirstTime = true;
        }
    }

    private void ChangeEthicsImage()
    {
        if (ethicsValue < 50)
        {
            scale.sprite = leftScales;
        }
        else if (ethicsValue == 50)
        {
            scale.sprite = balancedScales;
        }
        else
        {
            scale.sprite = rightScales;
        }
    }
}