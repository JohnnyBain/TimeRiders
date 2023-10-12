using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private GameManagerScript GMScript;

    // Start is called before the first frame update
    void Awake()
    {
        GMScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        GMScript.SetPlayingState(false);

        
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
