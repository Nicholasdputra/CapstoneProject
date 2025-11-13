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