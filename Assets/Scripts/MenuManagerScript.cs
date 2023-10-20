using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManagerScript : MonoBehaviour
{
    [SerializeField] GameObject GameManagerPrefab;
    [SerializeField] GameObject CanvasPrefab;
    [SerializeField] GameObject MainCamera;

    private GameObject UIcontroller;
    private GameObject gameManagerInstance;
    private int currentLevel;

    /* Awake:
     * Description: This method is called the instant the MenuManagerScript is created
     *              The menu manager is the only class in the project that is not called. It will be loaded into the unity scene 
     */
    void Awake()
    {
        UIcontroller = Instantiate(CanvasPrefab); //creating an instance of the CanvasPrefab (the CanvasPrefab is the parent of all the different menu screens)
        UIcontroller.transform.GetChild(0).GetComponent<MainMenuScript>().SetActive(); //set the main menu to active

        GameObject camera = Instantiate(MainCamera, new Vector3(4, (float)4.5, -10), transform.rotation); //create the main camera
    }

    /* LoadLevel:
     * Description: This method creates the game manager instance after setting the level (the game manager then fetches the level that it needs to load
     * 
     */
    public void LoadLevel(int levelNumber)
    {
        currentLevel = levelNumber;
        UIcontroller.transform.GetChild(1).GetComponent<LevelSelectScript>().SetInactive(); // turning off the level select menu

        gameManagerInstance = Instantiate(GameManagerPrefab);
    }
    /* NextLevel:
     * Description: This method concludes the current level by destroying the GameManagerInstance
     *              It then creates a new gameManagerIsntance after iterating to the next level
     */
    public void NextLevel()
    {
        UIcontroller.transform.GetChild(3).GetComponent<GameWinScript>().SetInactive(); //turning off the GameWin menu
        Destroy(gameManagerInstance); //wipe the GameManager for that level
        currentLevel++;
        gameManagerInstance = Instantiate(GameManagerPrefab);//create a new one with the new level set
    }
    /* ShowGameOverMenu:
     * Description: This method turns on the Game Over screen so it is shown to the player
     */
    public void ShowGameOverMenu() 
    {
        UIcontroller.transform.GetChild(2).GetComponent<GameOverScript>().SetActive(); 
    }
    /* ShowGameWinMenu:
     * Description: This method turns on the Game Win screen so it is shown to the player
     */
    public void ShowGameWinMenu() 
    {
        UIcontroller.transform.GetChild(3).GetComponent<GameWinScript>().SetActive();
    }



    //Getters -------------------------

    /* GetCurrentLevel:
     * Description: This is called by the GameBoardScript when it is created, it fetches the current level that it should be creating 
     * 
     */
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

}
