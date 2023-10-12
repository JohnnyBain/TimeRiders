using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManagerScript : MonoBehaviour
{
    [SerializeField] GameObject GameManagerPrefab;
    [SerializeField] GameObject CanvasPrefab;
    [SerializeField] GameObject MainCamera;
    private bool playingState;
    private GameObject UIcontroller;
    private int currentLevel;
    private GameObject GameManagerInstance;
    // Start is called before the first frame update
    void Awake()
    {

        Debug.Log("Menu Manager awake");
        UIcontroller = Instantiate(CanvasPrefab);
        UIcontroller.transform.GetChild(0).GetComponent<MainMenuScript>().SetActive();

        GameObject camera = Instantiate(MainCamera, new Vector3(4, (float)4.5, -10), transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGameOverMenu() 
    {
        UIcontroller.transform.GetChild(2).GetComponent<GameOverScript>().SetActive();
    }

    public void ShowGameWinMenu() 
    {
        UIcontroller.transform.GetChild(3).GetComponent<GameWinScript>().SetActive();
    }

    public void LoadLevel(int levelNumber)
    {
        currentLevel = levelNumber;
        Debug.Log("Load level - " + levelNumber);
        UIcontroller.transform.GetChild(1).GetComponent<LevelSelectScript>().SetInactive();

        GameManagerInstance = Instantiate(GameManagerPrefab);
    }

    public void NextLevel() 
    {
        UIcontroller.transform.GetChild(3).GetComponent<GameWinScript>().SetInactive();
        currentLevel++;
        Destroy(GameManagerInstance); //wipe the GameManager for that level and create a new one with the new level set
        Instantiate(GameManagerPrefab);
    }


    public int GetCurrentLevel() 
    {
        return currentLevel;
    }
    public void SetPlayingState(bool playing)
    {
        playingState = playing;
    }

}
