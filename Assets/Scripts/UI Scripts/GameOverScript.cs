using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
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
        MMScript.SetPlayingState(true);
    }
    public void RestartLevelButton()
    {
        MMScript.RestartLevel();
    }

    public void ReplayRiderButton() 
    {
        MMScript.ReplayRide();
    }
    public void QuitButton() 
    {
        Application.Quit();
    }
}
