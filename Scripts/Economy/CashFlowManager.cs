using UnityEngine;

public class CashFlowManager : MonoBehaviour
{
    private int incomeValue = 0;
    private int tempExpectation;
    private int tempSkillRating;
    private int gainMoneyCounter = 0;

    [SerializeField] private DoubleIntEvent onCVApproved;
    [SerializeField] private IntEvent onMoneyChange;

    private void OnEnable()
    {
        onCVApproved.AddListener(HandleWorkerInfos);
        NewsActions.OnSwiped += GainMoney;
    }

    private void OnDisable()
    {
        onCVApproved.RemoveListener(HandleWorkerInfos);
        NewsActions.OnSwiped -= GainMoney;
    }

    private void HandleWorkerInfos(int expectation, int skillRating)
    {
        tempExpectation = expectation;
        tempSkillRating = skillRating;

        LoseMoney();
        CalculateIncomeValue();
    }

    private void CalculateIncomeValue()
    {
        switch (tempSkillRating)
        {
            case 1:
                incomeValue += 500;
                break;
            case 2:
                incomeValue += 1000;
                break;
            case 3:
                incomeValue += 1500;
                break;
        }
    }

    private void LoseMoney()
    {
        onMoneyChange.Raise(-tempExpectation);
    }

    private void GainMoney()
    {
        gainMoneyCounter++;

        if (gainMoneyCounter == 3)
        {
            gainMoneyCounter = 0;
            onMoneyChange.Raise(incomeValue);
        }
    }
}