using UnityEngine;

public class GameInput : MonoBehaviour
{
    Player player = new Player();

    void Start()
    {

    }

    void Update()
    {

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit, 1000))
        {

            Town town = hit.collider.gameObject.GetComponent<Town>();

            if(town != null)
            {

            }

        }
    }
}
