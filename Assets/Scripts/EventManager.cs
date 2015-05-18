using UnityEngine;
using System.Collections.Generic;

public class EventManager
{
    public static EventManager instance; // only need one for now

    List<EventObserver> observers = new List<EventObserver>();

    public EventManager()
    {
    }

    public void AddObserver(EventObserver observer_)
    {
        observers.Add(observer_);
    }

    public void RemoveObserver(EventObserver observer_)
    {
        observers.Remove(observer_);
    }

    public void Activate(int action_, List<object> params_)
    {

        foreach (EventObserver obs in observers)
        {
            obs.OnAction(action_, params_);
        }

    }

}
