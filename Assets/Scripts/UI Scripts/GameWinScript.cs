using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinScript : MonoBehaviour
{
    GameManagerScript GMScript;
    private void Awake()
    {
        GMScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }
    public void SetActive() 
    {
        gameObject.SetActive(true);
        GMScript.SetPlayingState(false);
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
