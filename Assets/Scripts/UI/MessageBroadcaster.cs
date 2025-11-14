using System.Collections.Generic;
using UnityEngine;

public class MessageBroadcaster
{
    private static MessageBroadcaster instance;
    public static MessageBroadcaster Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MessageBroadcaster();
            }
            return instance;
        }
    }

    private readonly List<IMessageObserver> observers = new List<IMessageObserver>();

    public void Subscribe(IMessageObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void Unsubscribe(IMessageObserver observer)
    {
        observers.Remove(observer);
    }

    public void BroadcastMessage(string message)
    {
        for (int i = observers.Count - 1; i >= 0; i--)
        {
            if (observers[i] != null)
            {
                observers[i].OnMessageReceived(message);
            }
        }
    }

    public void SendMessageToObject(GameObject target, string message)
    {
        if (target == null) return;

        WorldSpaceUI worldUI = target.GetComponent<WorldSpaceUI>();
        if (worldUI != null)
        {
            worldUI.ShowMessageDirect(message);
        }

        for (int i = observers.Count - 1; i >= 0; i--)
        {
            if (observers[i] != null && observers[i] is MonoBehaviour mb && mb.gameObject != target)
            {
                observers[i].OnMessageReceived(message);
            }
        }
    }
}
