using UnityEngine;
using System.Collections.Generic;

public class GameInput : MonoBehaviour
{

    [SerializeField]
    Canvas menu;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.gameObject.SetActive(!menu.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Town town = GetTown();
            if (town != null && town.GetPlayer() == GameInterface.GetInstance().GetHumanPlayer())
            {
                GameInterface.GetInstance().GetHumanPlayer().AddSelectedTown(town);
            }
            else
            {
                GameInterface.GetInstance().GetHumanPlayer().Deselect();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            Town town = GetTown();
            if(town != null)
            {
                AttackTown(town, GameInterface.GetInstance().GetHumanPlayer().GetSelectedTowns());
            }
        }

        
    }

    void AttackTown(Town targetTown_, List<Town> attackingTowns_)
    {
        attackingTowns_.ForEach((x) =>
            {
                x.SendAttack(targetTown_);
            });
    }

    Town GetTown()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit, 1000))
        {
            return hit.collider.gameObject.GetComponent<Town>();
        }

        return null;
    }

    Army GetArmy()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(mouseRay, out hit, 1000);
        return hit.collider.gameObject.GetComponent<Army>();
    }
}
