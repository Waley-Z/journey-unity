using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
}

public static class EventManager
{
    static readonly Dictionary<Type, Action<GameEvent>> s_Events = new();

    static readonly Dictionary<Delegate, Action<GameEvent>> s_EventLookups = new();

    public static void AddListener<T>(Action<T> evt) where T : GameEvent
    {
        if (!s_EventLookups.ContainsKey(evt))
        {
            void newAction(GameEvent e) => evt((T)e);
            s_EventLookups[evt] = newAction;

            if (s_Events.TryGetValue(typeof(T), out Action<GameEvent> internalAction))
                s_Events[typeof(T)] = internalAction += newAction;
            else
                s_Events[typeof(T)] = newAction;
        }
    }

    public static void RemoveListener<T>(Action<T> evt) where T : GameEvent
    {
        if (s_EventLookups.TryGetValue(evt, out var action))
        {
            if (s_Events.TryGetValue(typeof(T), out var tempAction))
            {
                tempAction -= action;
                if (tempAction == null)
                    s_Events.Remove(typeof(T));
                else
                    s_Events[typeof(T)] = tempAction;
            }

            s_EventLookups.Remove(evt);
        }
    }

    public static void Broadcast(GameEvent evt)
    {
        Debug.Log("Event Broadcast: " + evt.GetType());
        if (s_Events.TryGetValue(evt.GetType(), out var action))
        {
            action.Invoke(evt);
        }
    }

    public static void Clear()
    {
        s_Events.Clear();
        s_EventLookups.Clear();
    }
}
