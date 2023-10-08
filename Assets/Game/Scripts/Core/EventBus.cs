using System;
using System.Collections.Generic;

public static class EventBus
{
    private static Dictionary<string, List<object>> _eventActions = new Dictionary<string, List<object>>();

    public static void Subscribe<T>(Action<T> action)
    {
        string key = typeof(T).Name;
        if (_eventActions.ContainsKey(key))
        {
            _eventActions[key].Add(action);
        }
        else
        {
            _eventActions.Add(key, new List<object>() { action });
        }
    }

    public static void Unsubscribe<T>(Action<T> action)
    {
        string key = typeof(T).Name;
        if (_eventActions.ContainsKey(key))
        {
            _eventActions[key].Remove(action);
        }
    }

    public static void Invoke<T>(T args)
    {
        string key = typeof(T).Name;
        if (_eventActions.ContainsKey(key))
        {
            foreach (object obj in _eventActions[key])
            {
                Action<T> action = obj as Action<T>;
                action?.Invoke(args);
            }
        }
    }
}




