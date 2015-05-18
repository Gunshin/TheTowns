using UnityEngine;
using System.Collections.Generic;
using System;

public class EventObserver
{

    Action<int, List<object>> onEvent;

    public EventObserver(Action<int, List<object>> onEvent_)
    {
        onEvent = onEvent_;
    }

    public void OnAction(int event_, List<object> params_)
    {
        onEvent(event_, params_);
    }

}
