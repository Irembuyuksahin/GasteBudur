using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private GameObject thresholds;

    private void Awake()
    {
        thresholds = GameObject.Find("Eţikler");
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (Transform threshold in thresholds.transform)
        {
            threshold.gameObject.SetActive(true);
        }
    }
}