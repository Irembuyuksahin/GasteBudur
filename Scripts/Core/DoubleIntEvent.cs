using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/DoubleIntEvent", fileName = "newDoubleIntEvent")]
public class DoubleIntEvent : ScriptableObject
{
    private Action<int, int> _action;

    public void AddListener(Action<int, int> action)
    {
        _action += action;
    }

    public void RemoveListener(Action<int, int> action)
    {
        _action -= action;
    }

    public void Raise(int value1, int value2)
    {
        _action?.Invoke(value1, value2);
    }
}