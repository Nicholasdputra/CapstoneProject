using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Void Event Channel")]
public class VoidEventChannel : ScriptableObject
{
    public Action OnEventRaised;

    public void RaiseEvent()
    {
        Debug.Log("Void Event Raised: " + name);
        OnEventRaised?.Invoke();
    }
}