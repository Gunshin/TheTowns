using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [SerializeField]
    List<Town> selectedTowns = new List<Town>();

    public void AddSelectedTown(Town town_)
    {
        if(!selectedTowns.Contains(town_))
        {
            selectedTowns.Add(town_);
        }
    }

    public List<Town> GetSelectedTowns()
    {
        return selectedTowns;
    }


}
