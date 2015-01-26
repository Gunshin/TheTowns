using UnityEngine;

public class Player : MonoBehaviour
{

    Town selectedTown;

    public void SetSelectedTown(Town town_)
    {
        selectedTown = town_;
    }

    public Town GetSelectedTown()
    {
        return selectedTown;
    }


}
