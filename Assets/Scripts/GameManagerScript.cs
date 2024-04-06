using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum RiderStatus  //An enum that describes whether a rider is currently spawned, has not be spawned yet, or has be spawned and is complete
{
    Riding,
    Waiting,
    Complete
}
public class GameManagerScript : MonoBehaviour
{

    [SerializeField] GameObject ReplayRiderPrefab;
    [SerializeField] GameObject PlayerRiderPrefab;
    [SerializeField] GameObject GameBoardPrefab;
    [SerializeField] GameObject CanvasPrefab;

    private GameObject currentRiderInstance;
    private GameObject gameBoardInstance;
    private GameObject[] allRiders; //An array of all the riders 

    private MenuManagerScript menuManagerScript;
    private GameBoardScript gameBoardScript;
    private PlayerRiderScript currentRiderScript;

    private RiderStatus[] isRiderDone; // An array of booleans that describe whether a rider has completed their journey
    private List<Direction>[] routes; //An array of the routes each of the riders for game have taken (each route is a list of the individual direction taken). If a rider has yet to ride its rout will be null
    [SerializeField] List<Color32> Colours; //The list of colours that the riders will each be assigned (rider 1 = element 0)

    private int numberOfRiders = 0; //The number of routes/riders that a board has
    private int currentRider = 1; // determines which rider the player is currently in control of
    private int turnCount = 0;
    private float time = 0f;
    private bool playingState = true;
    [SerializeField] float timeDelay; //Seconds between each automatic step of the replay riders


    /* Awake:
     * Description: Awake is called the moment an an object with this script attached is instantiated
     *              The code in this method is run once
     *              It references the MenuManager to find out what level should be loaded
     */
    void Awake()
    {
        menuManagerScript = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManagerScript>();
 
        CreateBoardInstance();// creates the board (the board fetches the level it should be creating from the MenuManagerScript

        allRiders = new GameObject[numberOfRiders]; //creates the list that will hold each of the rider (current & replays)

        

    }

    private void Start()
    {
        playingState = false;
        menuManagerScript.ShowCompletedRideMenu();

        InitaliseRiders(); //creates the riders needed for this first ride (naturally will only ever be one)
        numberOfRiders = gameBoardScript.GetRiderCount(); //creates a reference for the rider count for this class
    }

    /* Update:
     * Description: This is the core method of the game whilst in play. It is called by unity every single frame. For most of the game there is no computation unless the player has entered a move.
     *              most of the time the update loop is just constantly checking for inputs
     * 
     *              To stop and start play (for pause menus and game over screens) I set a member variable called playingState
     *              The update functions observes this variable before deciding whether to run/skip the game logic this turn 
     * 
     *              If the game is playing and the current rider has completed their journey but the game is not over (other replay riders have yet to finsih theirs)
     *              the update loop will enter a state where the updates are skipped until a certain amount of time has passed. (timeDelay)
     */
    void Update()
    {
        if (playingState) //if we are currently not paused, continue into the game loop
        {
            if (isRiderDone[currentRider - 1] == RiderStatus.Complete) //if the current rider is complete (automatically replay the ReplayRiders)
            {
                time = time + 1f * Time.deltaTime;
                if (time >= timeDelay) //time Delay = the time between each automatic step of the replay riders
                {
                    time = 0f;
                    GameTickUpdate(); //Move rider replays is within this
                }
            }
            else //the current player has more moves to make so continue to check input 
            {
                InputCheck();
            }
        }
    }

    /* OnDestroy:
     * Description: This method is called when a level is concluded and the GameManager is destroyed
     *              It destroys any objects that are connected to the instance of a level
     */
    public void OnDestroy() 
    {
        Destroy(gameBoardInstance);
        DestroyRiders();
    }

    /* CreateBoardInstance:
     * Description: Creates the GameBoard and creates a reference of its GameBoardScript to use in this class
     */
    private void CreateBoardInstance()
    {
        gameBoardInstance = Instantiate(GameBoardPrefab, new Vector3(0, 0, 0), transform.rotation);
        gameBoardScript = gameBoardInstance.GetComponent<GameBoardScript>();
        numberOfRiders = gameBoardInstance.GetComponent<GameBoardScript>().GetRiderCount(); //Now that the board has been created based off of one of the text files (levels), we are able to get the total number of riders for this level
        routes = new List<Direction>[numberOfRiders];
        isRiderDone = new RiderStatus[numberOfRiders];
    }

    /* InitaliseRiders:
     * Description: This method fetches the tile array from the gameBoard. It goes through the tiles and spawns riders on the spawn tiles.
     *              It only does this if they are the current rider controlled by the player or a rider that already has a route recorded for itself.
     *              This method will be called as many times as there are riders in the level (after each ride is complete the riders are destroyed and re initialised
     */
    private void InitaliseRiders()
    {
        turnCount = 0; //When the riders are spawned the turnCount is reset
        GameObject[,] tileArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray(); //copying the Tile Array from the game board
        int boardWidth = gameBoardInstance.GetComponent<GameBoardScript>().GetBoardWidth();
        int boardHeight = gameBoardInstance.GetComponent<GameBoardScript>().GetBoardHeight();

        for (int i = 1; i <= numberOfRiders; i++) //resetting all the ride statuses
        {
            isRiderDone[i - 1] = RiderStatus.Waiting;
        }

        for (int i = 0; i < boardWidth; i++) //Going through the x axis of the tiles on the board 
        {
            for (int j = 0; j < boardHeight; j++) //Going through the y axis of the tiles on the board
            {
                if (tileArray[i, j].GetComponent<TileScript>().GetTileType() == TileType.Spawn) //if this tile is a spawn tile
                {
                    if (tileArray[i, j].GetComponent<TileScript>().GetRiderID() == currentRider) //if this is where the current rider needs to be spawned
                    {
                        currentRiderInstance = Instantiate(PlayerRiderPrefab, new Vector3(i, j, 0), transform.rotation); //create current rider 
                        currentRiderScript = currentRiderInstance.GetComponent<PlayerRiderScript>(); //create a reference for the script of the created rider 
                        currentRiderScript.SetLocation(i, j); //set the riders location to that of the tile it was spawned on
                        currentRiderScript.SetColour(Colours.ElementAt(tileArray[i, j].GetComponent<TileScript>().GetRiderID() - 1)); //set the colour of the rider based on it's RiderID (this id corresponds to a colour in the Colours list
                        isRiderDone[currentRider - 1] = RiderStatus.Riding; //set this riders status to riding
                        allRiders[currentRider - 1] = currentRiderInstance; //add this rider to the list of riders in the game space
                    }
                    else //Otherwise a replay rider should be spawned here instead
                    {
                        if (routes[tileArray[i, j].GetComponent<TileScript>().GetRiderID() - 1] != null) //if there is a saved route for this rider 
                        {
                            GameObject replayRider = Instantiate(ReplayRiderPrefab, new Vector3(i, j, 0), transform.rotation); //create replay rider 
                            replayRider.GetComponent<ReplayRiderScript>().SetLocation(i, j); //set the riders location to that of the tile it was spawned on
                            replayRider.GetComponent<ReplayRiderScript>().SetColour(Colours.ElementAt(tileArray[i, j].GetComponent<TileScript>().GetRiderID() - 1)); //set the colour of the rider based on it's RiderID (this id corresponds to a colour in the Colours list
                            isRiderDone[tileArray[i, j].GetComponent<TileScript>().GetRiderID() - 1] = RiderStatus.Riding; //set this riders status to riding
                            allRiders[tileArray[i, j].GetComponent<TileScript>().GetRiderID() - 1] = replayRider; //add this rider to the list of riders in the game space
                            replayRider.GetComponent<ReplayRiderScript>().InitialiseGhostRider(tileArray[i, j].GetComponent<TileScript>().GetRiderID() - 1);
                        }
                    }
                }
            }
        }
    }

    /* GameTickUpdate:
     * Description: This method is called each time the riders need to be moved. This could either be because the 
     *              player has inputted a move or the replay riders are being moved automatically
     *              It contains the computation that decides whether the new game state should results in (winning, losing, or nothing)
     */
    public void GameTickUpdate() 
    {
        GameObject[,] tileArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        MoveRiderReplays(); //everytime the player moves and it was a valid move, move the riders
        foreach (GameObject t in tileArray) //checks all the tiles on the board
        {
            if (t.GetComponent<TileScript>().GetTileType() != TileType.Wall) //ignores the wall tiles (nothing could be inside them)
            {
                if (t.GetComponent<TileScript>().GetObjectList().Count >= 2) //If there are more than one occupant in any tile on the board
                {
                    GameOver();
                    Debug.Log("x = " + t.transform.position.x + "| y = " + t.transform.position.y); //the offending tile 
                }
                //else if the tile is a finish tile, check if this tile is for the current rider, check if there's a rider on it 
                else if (t.GetComponent<TileScript>().GetTileType() == TileType.Finish && t.GetComponent<TileScript>().GetObjectList().Contains(currentRiderInstance) && t.GetComponent<TileScript>().GetRiderID() == currentRider) 
                {
                    isRiderDone[currentRider - 1] = RiderStatus.Complete;
                }
            }  
        }
        GameWinCheck();
    }

    /* GameWinCheck:
     * Description: This method is called each time the GameTickUpdate occurs (which is everytime and rider (current or replay) moves.
     *              It runs the game logic for when the rides are all complete. If the current ride is not the final ride it saves the route played in by the player
     *              If this is the final ride then the it calls GameWin as all riders have reached their destinations without colliding 
     */
    public void GameWinCheck()
    {

        if (!isRiderDone.Contains(RiderStatus.Riding)) //if there are no riders still riding
        {
            routes[currentRider - 1] = (currentRiderScript.GetRoute()); //save the players route into the routes array
            if (!routes.Contains(null)) //If this is the final rider
            {
                GameWin();
            }
            else //Another ride is still to complete so clear restart the level on the next rider
            {
                playingState = false;
                menuManagerScript.ShowCompletedRideMenu();
                //currentRider++;
                //StartRiders();
            }
        }
    }


    /* StartRiders:
     * Description: A method that clears the tile's object lists and re-initialises the riders 
     * 
     */
    public void StartRiders() 
    {
        gameBoardScript.ClearTileLists(); //wipes the object lists each tile holds
        DestroyRiders();
        InitaliseRiders(); //Initalises the riders again (this time with a new replay rider that uses the route saved above)
    }

    /* SelectedRider:
     * Description: A method called by the menu manager that sets the current rider to what the player has selected. It also
     *              sets the playing state to true and re-initialises the riders with the new currentRider.
     * 
     */
    public void SelectRider(int riderID) 
    {
        SetCurrentRider(riderID);
        routes[riderID - 1] = null;
        StartRiders();
        playingState = true;
    }

    /* InputCheck:
     * Description: This method is called on every frame whilst playingStatus = true
     *              It calls the updateRider method of the current rider (the one the player is in control of) and passes it the direction the player has entered
     */
    private void InputCheck()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow) == true)
        {
            currentRiderScript.UpdateRider(Direction.Right);
        }
        else
            if (Input.GetKeyDown(KeyCode.LeftArrow) == true)
        {
            currentRiderScript.UpdateRider(Direction.Left);
        }
        else
            if (Input.GetKeyDown(KeyCode.UpArrow) == true)
        {
            currentRiderScript.UpdateRider(Direction.Up);
        }
        else
            if (Input.GetKeyDown(KeyCode.DownArrow) == true)
        {
            currentRiderScript.UpdateRider(Direction.Down);
        }
    }


    /* MoveRiderReplays:
     * Description: This method is called to progress all the replay riders a step through their respective routes
     *              It's called whenever the play makes a valid move, and also at time intervals when the replay riders are in automatic mode (when the play has finished their route but the replays have furthere to go)
     */
    private void MoveRiderReplays()
    {
        for (int i = 0; i < numberOfRiders; i++)
        {
            if (routes[i] != null) //if this rider has a route and therefore is a replay rider
            {
                if (routes[i].Count > turnCount) //if the rider has more moves to execute
                {
                    allRiders[i].GetComponent<ReplayRiderScript>().MoveRider(routes[i].ElementAt(turnCount)); //move the rider in direction the route dictates for this turn count 
                    if (routes[i].Count == turnCount + 1) //if this is the last move
                    {
                        isRiderDone[i] = RiderStatus.Complete; //set this riders status to complete
                    }
                    else //if it's not the last move, update the Ghost pointer
                    {
                        allRiders[i].GetComponent<ReplayRiderScript>().UpdateGhostPointerRider(routes[i].ElementAt(turnCount+1)); //move to the pointer to its next position
                    }
                }
            }
        }
        turnCount++;
    }

    /* DestroyRiders:
     * Description: Destroys all the riders that are currently in the level
     *              This is called both when the player is ready to complete a new ride, and when the level is complete and the player can progress to the next
     */
    private void DestroyRiders()
    {
        foreach (GameObject rider in allRiders)
        {
            Destroy(rider); //Destroys the rider and calls the OnDestroy method within the Rider script
        }
    }

    /* GameOver:
     * Description: this method tells the menu manager that the game is over and haults the playing of the game 
     */
    public void GameOver()
    {
        playingState = false;
        menuManagerScript.ShowGameOverMenu();
        
    }
    /* GameWin:
     * Description: this method tells the menu manager that the game is won and haults the playing of the game 
     */
    public void GameWin() 
    {
        playingState = false;
        menuManagerScript.ShowGameWinMenu();
    }

    //Getters ----------------------
    public GameObject[] GetRiders()
    {
        return allRiders;
    }

    public GameObject GetCurrentRider()
    {
        return currentRiderInstance;
    }

    public GameObject GetGameBoard()
    {
        return gameBoardInstance;
    }

    public List<Color32> GetColours()
    {
        return Colours;
    }

    public bool GetPlayingState() 
    {
        return playingState;
    }

    public List<Direction>[] GetRoutes() 
    {
        return routes;
    }
    //Setters ----------------------
    public void SetPlayingState(bool playing) 
    {
        playingState = playing;
    }

    public void SetCurrentRider(int riderID) 
    {
        currentRider = riderID;
    }
}
