using UnityEngine;
using System.Collections;

public class MainMenuFunctions : MonoBehaviour
{

    public void LoadScene(string name_)
    {
        Application.LoadLevel(name_);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
