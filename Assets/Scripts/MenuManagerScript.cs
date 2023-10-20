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

    private GameObject gameManagerInstance;
    private GameObject mainCameraInstance;
    // Start is called before the first frame update
    void Awake()
    {

        Debug.Log("Menu Manager awake");
        UIcontroller = Instantiate(CanvasPrefab);
        UIcontroller.transform.GetChild(0).GetComponent<MainMenuScript>().SetActive();

        mainCameraInstance = Instantiate(MainCamera, new Vector3(4, (float)4.5, -10), transform.rotation);

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

        gameManagerInstance = Instantiate(GameManagerPrefab);
    }

    public void NextLevel() 
    {
        UIcontroller.transform.GetChild(3).GetComponent<GameWinScript>().SetInactive();
        currentLevel++;
        gameManagerInstance.SetActive(false); //Set to inactive so that when the new Board/riders try to look for a gameManager, they find the new one and not the old one)
        Destroy(gameManagerInstance); //wipe the GameManager for that level and create a new one with the new level set
        gameManagerInstance = Instantiate(GameManagerPrefab);
    }

    public void RestartLevel() 
    {
        UIcontroller.transform.GetChild(2).GetComponent<GameOverScript>().SetInactive();
        gameManagerInstance.SetActive(false); //Set to inactive so that when the new Board/riders try to look for a gameManager, they find the new one and not the old one)
        Destroy(gameManagerInstance); //wipe the GameManager for that level and create a new one with the new level set
        gameManagerInstance = Instantiate(GameManagerPrefab);
    }

    public void ReplayRide() 
    {
        UIcontroller.transform.GetChild(2).GetComponent<GameOverScript>().SetInactive();
        gameManagerInstance.GetComponent<GameManagerScript>().StartRiders();
        SetPlayingState(true);
    }


    public int GetCurrentLevel() 
    {
        return currentLevel;
    }
    public void SetPlayingState(bool playing)
    {
        gameManagerInstance.GetComponent<GameManagerScript>().SetPlayingState(true);
    }

    public void ResetCamera(float x, float y) 
    {
        Debug.Log("Camera - (" + x + ")(" + y + ")");
        mainCameraInstance.SetActive(true);
        Destroy(mainCameraInstance);
        mainCameraInstance = Instantiate(MainCamera, new Vector3(x - 0.5f, y - 0.5f, -10), transform.rotation);
    }
}
