using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinScreen : MonoBehaviour
{
    GameManagerScript GMScript;
    public void Setup() 
    {
        gameObject.SetActive(true);
        GMScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();

    }
    public void NextLevelButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitButton() 
    {
        Application.Quit();
        //GMScript.Exit();
        Debug.Log("Game is exiting");
    }
}
