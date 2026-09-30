using UnityEngine;

[CreateAssetMenu(fileName = "Mail", menuName = "Scriptable Objects/Mail")]
public class Mail : ScriptableObject
{
    public string mailId;
    public string mailSubject;
    public string mailAddress;

    [TextArea(3, 10)]
    public string mailBody;
}