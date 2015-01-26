using UnityEngine;
using System.Collections.Generic;

public class GameInterface : MonoBehaviour
{
    List<Town> towns = new List<Town>();
    List<Army> armies = new List<Army>();

    void Start()
    {
    }

    void Update()
    {

    }

    public void RegisterTown(Town town_)
    {
        towns.Add(town_);
    }

    public void RegisterArmy(Army army_)
    {
        armies.Add(army_);
    }
}
