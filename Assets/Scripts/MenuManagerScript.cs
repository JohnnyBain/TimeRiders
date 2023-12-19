using UnityEngine;

public class MenuManagerScript : MonoBehaviour
{
    [SerializeField] GameObject GameManagerPrefab;
    [SerializeField] GameObject CanvasPrefab;
    [SerializeField] GameObject MainCamera;

    private static MenuManagerScript Instance;

    private GameObject UIcontroller;
    private int currentLevel; 

    private GameObject gameManagerInstance;
    private GameObject mainCameraInstance;

    /* Awake:
    * Description: This method is called the instant the MenuManagerScript is created
    *              The menu manager is the only class in the project that is not called. It will be loaded into the unity scene 
    */
    void Awake()
    {
        if (Instance == null) //singleton
        {
            Instance = this;
            Debug.Log("Menu Manager awake");
            UIcontroller = Instantiate(CanvasPrefab); //creating an instance of the CanvasPrefab (the CanvasPrefab is the parent of all the different menu screens)
            UIcontroller.transform.GetChild(0).GetComponent<MainMenuScript>().SetActive(); //set the main menu to active

            mainCameraInstance = Instantiate(MainCamera, new Vector3(4, (float)4.5, -10), transform.rotation); //create the main camera
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    /* OnDestroy:
    * Description: This method is called when the MenuManager object is destroyed. It destroys any of the objects that it created.
    */
    private void OnDestroy()
    {
        Destroy(gameManagerInstance);
        Destroy(mainCameraInstance);
        Destroy(UIcontroller);
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

    public void ShowCompletedRideMenu() 
    {
        UIcontroller.transform.GetChild(4).GetComponent<CompleteRideScript>().SetActive();
    }

    /* LoadLevel:
     * Description: This method creates the game manager instance after setting the level (the game manager then fetches the level that it needs to load
     * 
     */
    public void LoadLevel(int levelNumber)
    {
        if (gameManagerInstance != null) 
        {
            gameManagerInstance.SetActive(false); //Set to inactive so that when the new Board/riders try to look for a gameManager, they find the new one and not the old one)
            Destroy(gameManagerInstance); //wipe the GameManager for that level and create a new one with the new level set
        } 
        currentLevel = levelNumber;
        Debug.Log("Load level - " + levelNumber);
        UIcontroller.transform.GetChild(1).GetComponent<LevelSelectScript>().SetInactive(); // turning off the level select menu

        gameManagerInstance = Instantiate(GameManagerPrefab);
    }
    
    /* NextLevel:
     * Description: This method concludes the current level by destroying the GameManagerInstance
     *              It then creates a new gameManagerIsntance after iterating to the next level
     */
    public void NextLevel() 
    {
        UIcontroller.transform.GetChild(3).GetComponent<GameWinScript>().SetInactive();
        currentLevel++;
        gameManagerInstance.SetActive(false); //Set to inactive so that when the new Board/riders try to look for a gameManager, they find the new one and not the old one)
        Destroy(gameManagerInstance); //wipe the GameManager for that level and create a new one with the new level set
        gameManagerInstance = Instantiate(GameManagerPrefab);
    }

    /* RestartLevel:
     * Description: This method concludes the current level by destroying the GameManagerInstance
     *              It then restarts the same level and turns the GameOver menu off
     */
    public void RestartLevel() 
    {
        UIcontroller.transform.GetChild(2).GetComponent<GameOverScript>().SetInactive();
        gameManagerInstance.SetActive(false); //Set to inactive so that when the new Board/riders try to look for a gameManager, they find the new one and not the old one)
        Destroy(gameManagerInstance); //wipe the GameManager for that level and create a new one with the new level set
        gameManagerInstance = Instantiate(GameManagerPrefab);
    }

    /* RestartLevel:
     * Description: This method does not destroy the game manager,
     *              It sets the game state to play, calls StartRiders to restart the riders, and hides the game over menu
     */
    public void ReplayRide() 
    {
        UIcontroller.transform.GetChild(2).GetComponent<GameOverScript>().SetInactive();
        gameManagerInstance.GetComponent<GameManagerScript>().StartRiders();
        SetPlayingState(true);
    }

    /* ResetCamera:
     * [x] - The x location the camera should be located at
     * [y] - The y location the camera should be located at 
     * 
     * Description: This is called by the GameBoardScript when the board is initialised.
     *              After the file has been loaded in, the dimensions are processed and 
     *              this method is passed the coordinates to spawn the camera. 
     *              The location is offset by 0.5 because tile {0,0} is the center of the first tile, not the bottom left corner
     * 
     */
    public void ResetCamera(float x, float y)
    {
        Debug.Log("Camera - (" + x + ")(" + y + ")");
        mainCameraInstance.SetActive(true);
        Destroy(mainCameraInstance);
        mainCameraInstance = Instantiate(MainCamera, new Vector3(x - 0.5f, y - 0.5f, -10), transform.rotation);
    }

    /* SelectRider:
     * [riderID] - the riderID that the user would like to select and play next
     * 
     * Description: This is called by the CompleteRideScript. After a ride is complete or at the start of the level, the user is 
     *              given the choice as to which ride they would like to play next. This method is called from the menu presented to the 
     *              user and sets the GameManagers currentRider based off of the integer parameter
     */
    public void SelectRider(int riderID) 
    {
        gameManagerInstance.GetComponent<GameManagerScript>().SelectRider(riderID);
        UIcontroller.transform.GetChild(4).GetComponent<CompleteRideScript>().SetInactive(); //turning off the SelectRider menu
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


    /*  Test
     * 
     */
    public GameObject GetGameInstance()
    {
        return gameManagerInstance;
    }

    /* GetUIController:
     * Description: returns the UIcontroller. It's called in test scripts that need to find out what menu is currently being displayed
     * 
     */
    public GameObject GetUIController() 
    {
        return UIcontroller;
    }

    //Setters -------------------------

    /* SetPlayingState:
     * [playing] - the state that the game should be set to (true = playing, false = paused)
     * 
     * Description: Calls the sister function in Game Manager that sets the bool variable the game 
     *              loop looks at before decided to run game logic or not
     * 
     */
    public void SetPlayingState(bool playing)
    {
        gameManagerInstance.GetComponent<GameManagerScript>().SetPlayingState(true);
    }
    
}
