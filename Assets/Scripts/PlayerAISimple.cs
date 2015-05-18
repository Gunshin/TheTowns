using UnityEngine;
using System.Collections.Generic;

public class PlayerAISimple : Player
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
                    {
                        Town originatingTown = (Town)params_[0];// params 0 should be the originating town
                        Town targetTown = (Town)params_[1];// params 0 should be the originating town

                        // if we sent the attack, ignore it
                        // if we are not the target, ignore it
                        if(originatingTown.GetPlayer() == this || targetTown.GetPlayer() != this)
                        {
                            return;
                        }

                        ReinforceTown(originatingTown);

                        break;
                    }
                case GameInterface.Events.MaxDefenders:
                    {
                        break;
                    }
                case GameInterface.Events.MaxPopulation:
                    {
                        Town originatingTown = (Town)params_[0];// params 0 should be the originating town

                        AttackSomethingAlready(originatingTown);

                        break;
                    }
                case GameInterface.Events.Reinforce:
                    {
                        break;
                    }
                case GameInterface.Events.OverPopulated:
                    {
                        Town originatingTown = (Town)params_[0];// params 0 should be the originating town

                        AttackSomethingAlready(originatingTown);

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        });

        EventManager.instance.AddObserver(observer);

    }

    public void AttackSomethingAlready(Town originatingTown_)
    {
        if (originatingTown_.GetPlayer() != this)
        {
            return; // lets not do anything if we do not own this town
        }

        List<Player> players = GameInterface.instance.GetAllPlayers();

        foreach (Player player in players)
        {
            if (player == this)
            {
                continue; // skip this player
            }

            List<Town> towns = GameInterface.instance.GetPlayerTowns(player);

            foreach (Town town in towns)
            {
                if (town.GetTotalPopulation() < originatingTown_.GetAttackerPopulation())
                {
                    originatingTown_.SendAttack(town);
                    return;
                }
            }
        }

        // if that part did not send an attack, that means the originating town does not have enough attackers to assault any non-owned towns
        // lets do a combined attack from all towns on a single town

        int totalAttackerPop = GetTotalAttackingPopulation();

        foreach (Player player in players)
        {
            if (player == this)
            {
                continue; // skip this player
            }

            List<Town> towns = GameInterface.instance.GetPlayerTowns(player);

            foreach (Town town in towns)
            {
                if (totalAttackerPop > town.GetTotalPopulation()) // if our total attacking population is greater than the targets total population, ATTACK!
                {
                    List<Town> ownedTowns = GameInterface.instance.GetPlayerTowns(this);

                    foreach (Town owned in ownedTowns)
                    {
                        owned.SendAttack(town);
                    }

                    return;
                }
            }
        }
    }

    public int GetTotalAttackingPopulation()
    {
        List<Town> towns = GameInterface.instance.GetPlayerTowns(this);
        int totalPop = 0;

        foreach(Town town in towns)
        {
            totalPop += town.GetAttackerPopulation();
        }

        return totalPop;
    }

    public void ReinforceTown(Town town_)
    {

        List<Town> ownedTowns = GameInterface.instance.GetPlayerTowns(this);

        foreach (Town owned in ownedTowns)
        {
            owned.SendAttack(town_);
        }

    }
}
