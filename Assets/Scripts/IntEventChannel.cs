using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IntEventChannel", menuName = "Events/Int Event Channel", order = 1)]
public class IntEventChannel : ScriptableObject
{
    public Action<int> OnEventRaised;

    public void RaiseEvent(int value)
    {
        // Debug.Log("Int Event Raised: " + name + " with value: " + value);
        OnEventRaised?.Invoke(value);
        // if (OnEventRaised != null)
        // {
        //     foreach (var d in OnEventRaised.GetInvocationList())
        //     {
        //         Debug.Log($"[Event Debug] There is a Listener: {d.Method.Name} — for {OnEventRaised.Method.Name} which is attached to: {d.Target}");
        //     }
        // }
        // else
        // {
        //     Debug.Log($"[Event Debug] No listeners subscribed to: {OnEventRaised.Method.Name} event.");
        // }
    }
}