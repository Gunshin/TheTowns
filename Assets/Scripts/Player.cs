using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [SerializeField]
    List<Town> selectedTowns = new List<Town>();

    Color playerColor;

    public void AddSelectedTown(Town town_)
    {
        if(!selectedTowns.Contains(town_))
        {
            selectedTowns.Add(town_);
            town_.Select();
        }
    }

    public void Deselect()
    {
        foreach(Town town in selectedTowns)
        {
            town.Deselect();
        }

        selectedTowns.Clear();
    }

    public List<Town> GetSelectedTowns()
    {
        return selectedTowns;
    }

    public void SetColour(Color color_)
    {
        playerColor = color_;
    }

    public Color GetColour()
    {
        return playerColor;
    }

}
