using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinScript : MonoBehaviour
{
    MenuManagerScript MMScript;
    private void Awake()
    {
        MMScript = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManagerScript>();
    }
    public void SetActive() 
    {
        gameObject.SetActive(true);
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }
    public void NextLevelButton()
    {
        MMScript.NextLevel();
    }

    public void QuitButton() 
    {
        Application.Quit();
        //GMScript.Exit();
        Debug.Log("Game is exiting");
    }
}
