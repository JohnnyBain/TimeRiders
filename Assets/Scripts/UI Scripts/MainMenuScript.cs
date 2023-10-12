using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private MenuManagerScript MMScript;

    // Start is called before the first frame update
    void Awake()
    {
        MMScript = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManagerScript>();

        gameObject.SetActive(true);
       
    }

    public void LevelSelectButton()
    {
        gameObject.SetActive(false);
        gameObject.transform.parent.GetChild(1).GetComponent<LevelSelectScript>().SetActive();
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void SetActive() 
    {
        gameObject.SetActive(true);
    }

    public void SetInactive() 
    {
        gameObject.SetActive(false);
    }
}
