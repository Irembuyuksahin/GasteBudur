using UnityEngine;

public class Threshold : MonoBehaviour
{
    [Header("Threshold Type")]
    [SerializeField] private bool firstThreshold;

    private ThresholdManager thresholdManager;

    private void Awake()
    {
        thresholdManager = GetComponentInParent<ThresholdManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (firstThreshold)
            {
                ElevatorSceneActions.OnFirstThresholdCrossed();
                thresholdManager.InstantiateTempRoom();
                thresholdManager.TeleportRoom();
            }
            else
            {
                ElevatorSceneActions.OnLastThresholdCrossed();
                thresholdManager.CheckIfPlayerTurnedBack();

                foreach (Transform sibling in transform.parent)
                {
                    if (sibling != transform)
                    {
                        sibling.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        ElevatorSceneActions.OnElevatorOn += TriggerOff;
    }

    private void OnDisable()
    {
        ElevatorSceneActions.OnElevatorOn -= TriggerOff;
    }

    private void TriggerOff()
    {
        gameObject.SetActive(false);
    }
}