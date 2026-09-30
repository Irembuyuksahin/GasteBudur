using TMPro;
using UnityEngine;

public class MoneyScript : MonoBehaviour
{
    [SerializeField] private IntEvent onMoneyChange;

    private TMP_Text moneyText;

    public static int moneyAmount = 15000;

    private void Awake()
    {
        moneyText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        onMoneyChange.AddListener(ChangeMoneyAmount);
    }

    private void OnDisable()
    {
        onMoneyChange.RemoveListener(ChangeMoneyAmount);
    }

    private void ChangeMoneyAmount(int amount)
    {
        int newValue = int.Parse(moneyText.text) + amount;
        moneyAmount = newValue;
        moneyText.text = newValue.ToString();
    }
}