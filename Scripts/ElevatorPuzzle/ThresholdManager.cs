using UnityEngine;

public class ThresholdManager : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject mainRoom;
    [SerializeField] private GameObject elevatorRoom;
    [SerializeField] private GameObject tempRoomPrefab;

    private GameObject tempRoom;
    private GameObject tempElevatorRoom;

    private bool firstThresholdCrossed = false;
    private bool lastThresholdCrossed = false;

    private Vector3 roomInitialPos;
    private Vector3 roomTargetPos;

    [SerializeField] private IntEvent onAchievementEarned;

    private void Awake()
    {
        roomInitialPos = new Vector3(0, 0, 0);
        roomTargetPos = new Vector3(0, 0, 52);
    }

    private void OnEnable()
    {
        ElevatorSceneActions.OnFirstThresholdCrossed += FirstThresholdCrossed;
        ElevatorSceneActions.OnLastThresholdCrossed += LastThresholdCrossed;
    }

    private void OnDisable()
    {
        ElevatorSceneActions.OnFirstThresholdCrossed -= FirstThresholdCrossed;
        ElevatorSceneActions.OnLastThresholdCrossed -= LastThresholdCrossed;
    }

    private void FirstThresholdCrossed()
    {
        firstThresholdCrossed = !firstThresholdCrossed;
    }

    private void LastThresholdCrossed()
    {
        lastThresholdCrossed = !lastThresholdCrossed;
    }

    private void ResetThresholdData()
    {
        firstThresholdCrossed = false;
        lastThresholdCrossed = false;
    }

    public void TeleportRoom()
    {
        if (mainRoom.transform.position == roomInitialPos)
        {
            mainRoom.transform.position = roomTargetPos;
            mainRoom.transform.Rotate(0, 180, 0, Space.Self);
        }
        else
        {
            mainRoom.transform.position = roomInitialPos;
            mainRoom.transform.Rotate(0, 180, 0, Space.Self);
        }
    }

    public void CheckIfPlayerTurnedBack()
    {
        if (lastThresholdCrossed && firstThresholdCrossed)
        {
            ResetThresholdData();
        }
        else if (lastThresholdCrossed && !firstThresholdCrossed)
        {
            if (mainRoom.transform.position == roomInitialPos)
            {
                tempElevatorRoom = Instantiate(elevatorRoom, roomTargetPos, Quaternion.Euler(new Vector3(0, 0, 0)));
                Destroy(tempRoom);
                tempRoom = Instantiate(tempRoomPrefab, roomInitialPos, Quaternion.Euler(new Vector3(0, 0, 0)));
                tempRoom.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                tempElevatorRoom = Instantiate(elevatorRoom, roomInitialPos, Quaternion.Euler(new Vector3(0, 180, 0)));
                Destroy(tempRoom);
                tempRoom = Instantiate(tempRoomPrefab, roomTargetPos, Quaternion.Euler(new Vector3(0, 180, 0)));
                tempRoom.transform.GetChild(0).gameObject.SetActive(false);
            }

            mainRoom.SetActive(false);
            ResetThresholdData();
            ElevatorSceneActions.OnElevatorOn();
            onAchievementEarned.Raise(6);
        }
    }

    public void InstantiateTempRoom()
    {
        if (mainRoom.transform.position == roomInitialPos)
        {
            if (tempRoom != null)
            {
                Destroy(tempRoom);
            }

            tempRoom = Instantiate(tempRoomPrefab, roomInitialPos, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        else
        {
            if (tempRoom != null)
            {
                Destroy(tempRoom);
            }

            tempRoom = Instantiate(tempRoomPrefab, roomTargetPos, Quaternion.Euler(new Vector3(0, 180, 0)));
        }
    }
}