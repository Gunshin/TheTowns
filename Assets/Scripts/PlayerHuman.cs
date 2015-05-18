using UnityEngine;
using System.Collections;

public class PlayerHuman : Player
{

    EventObserver observer;

    // Use this for initialization
    void Start()
    {

        observer = new EventObserver((eventID_, params_) =>
        {
            GameInterface.Events id = (GameInterface.Events)eventID_;
            switch (id)
            {
                case GameInterface.Events.Attack:
                    break;
                case GameInterface.Events.MaxDefenders:
                    break;
                case GameInterface.Events.MaxPopulation:
                    break;
                case GameInterface.Events.Reinforce:
                    break;
                default:
                    break;
            }
        });

    }

}
