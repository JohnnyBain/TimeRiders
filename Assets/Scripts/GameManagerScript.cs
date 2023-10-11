using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using TMPro.Examples;
using System.Diagnostics.CodeAnalysis;

public enum RiderStatus  //An enum that describes whether a rider is currently spawned, has not be spawned yet, or has be spawned and is complete
{
    Riding,
    Waiting,
    Complete
}
public class GameManagerScript : MonoBehaviour
{


    public GameObject MainCamera;
    public GameObject RiderPrefab;
    public GameObject GameBoardPrefab;
    public GameObject DelayPrefab;
    public GameOverScreen GameOverScreen;
    public GameWinScreen GameWinScreen;
    

    private GameObject currentRiderInstance;
    private GameObject gameBoardInstance;

    private GameBoardScript gameBoardScript;
    private RiderScript currentRiderScript;

    public List<Color32> Colours;
    private RiderStatus[] isRiderDone; // An array of booleans that describe whether a rider has completed there journey
    private GameObject[] AllRiders;
    private List<Direction>[] routes;
    private int numberOfRiders; //The number of routes/riders that a board has
    private int currentRider = 1; // determines which rider the player is currently in control of
    private int turnCount;
    private float time;
    public float timeDelay = 0.5f;
    private bool isAutoPlaying = false;


    // Start is called before the first frame update
    void Awake()
    {
        time = 0f;
        Debug.Log("GameManager Awake ----------");
        GameObject camera = Instantiate(MainCamera, new Vector3(4, (float)4.5, -10), transform.rotation);


        CreateBoardInstance();// creates a board (each time a new ride begins the board is recreated)
        AllRiders = new GameObject[numberOfRiders];

        InitaliseRiders();

        numberOfRiders = gameBoardScript.GetRiderCount();

        Debug.Log("rider count = " + numberOfRiders);

        //currentRiderScript = currentRiderInstance.GetComponent<RiderScript>();
        //currentRiderScript.SetLocation(1, 1);
        //currentRiderScript.SetColour(Colours.ElementAt(0));

        //allRiders = new List<GameObject>();

        //allRiders.Add(currentRiderInstance);

        routes = new List<Direction>[numberOfRiders]; //creating an array of lists to hold the routes of each rider

    }

    // Update is called once per frame
    void Update()
    {

        if (isRiderDone[currentRider - 1] == RiderStatus.Complete) //if the current rider is complete (automatically replay the ReplayRiders)
        {
            time = time + 1f * Time.deltaTime;
            Debug.Log(time);
            if (time >= timeDelay) //time Delay = the time between each automatic step of the replay riders
            {
                time = 0f;
                GameTickUpdate(); //Move rider replays is within this
            }
        }
        else
        {
            inputCheck();
        }
    }


    private void CreateBoardInstance()
    {
        gameBoardInstance = Instantiate(GameBoardPrefab, new Vector3(0, 0, 0), transform.rotation);
        gameBoardScript = gameBoardInstance.GetComponent<GameBoardScript>();
        numberOfRiders = gameBoardInstance.GetComponent<GameBoardScript>().GetRiderCount();
        routes = new List<Direction>[numberOfRiders];
    }



    private void InitaliseRiders()
    {
        turnCount = 0;
        GameObject[,] tileArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        int boardSize = gameBoardInstance.GetComponent<GameBoardScript>().GetBoardSize();

        Debug.Log("-------------------------- number of riders = " + numberOfRiders);
        isRiderDone = new RiderStatus[numberOfRiders];
        for (int i = 1; i <= numberOfRiders; i++)
        {
            isRiderDone[i - 1] = RiderStatus.Waiting;
        }

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (tileArray[i, j].GetComponent<TileScript>().GetTileType() == TileType.Spawn) //if this tile is a spawn tile
                {
                    if (tileArray[i, j].GetComponent<TileScript>().GetRiderID() == currentRider) //if this is where the current rider needs to be spawned
                    {
                        currentRiderInstance = Instantiate(RiderPrefab, new Vector3(i, j, 0), transform.rotation);
                        currentRiderScript = currentRiderInstance.GetComponent<RiderScript>();
                        currentRiderScript.SetLocation(i, j);
                        currentRiderScript.SetColour(Colours.ElementAt(tileArray[i, j].GetComponent<TileScript>().GetRiderID() - 1)); //rider ID starts at 1 but the colour list starts at 0
                        Debug.Log("current rider index = " + (currentRider - 1));
                        isRiderDone[currentRider - 1] = RiderStatus.Riding;
                        AllRiders[currentRider - 1] = currentRiderInstance;
                    }
                    else
                    {
                        if (routes[tileArray[i, j].GetComponent<TileScript>().GetRiderID() - 1] != null)
                        {
                            GameObject replayRider = Instantiate(RiderPrefab, new Vector3(i, j, 0), transform.rotation);
                            replayRider.GetComponent<RiderScript>().SetLocation(i, j);
                            replayRider.GetComponent<RiderScript>().SetColour(Colours.ElementAt(tileArray[i, j].GetComponent<TileScript>().GetRiderID() - 1));
                            isRiderDone[tileArray[i, j].GetComponent<TileScript>().GetRiderID() - 1] = RiderStatus.Riding;
                            AllRiders[tileArray[i, j].GetComponent<TileScript>().GetRiderID() - 1] = replayRider;
                        }
                    }
                }
            }
        }
    }

    private void DestroyRiders()
    {
        foreach (GameObject rider in AllRiders)
        {
            Destroy(rider);
        }
    }

    //Core level gameplay loop
    //
    //  IF (current rider complete && all replays are done)
    //      IF (completed riders == total riders)
    //          GameEnd
    //      else
    //          CurrentRider + 1;
    //          Restart Riders 
    //      
    //
    //

    public void GameTick()
    {
        Debug.Log("Current Rider = " + currentRider);
        Debug.Log("RiderStatuses = " + isRiderDone[0] + "," + isRiderDone[1] + "," + isRiderDone[2]);
        if (!isRiderDone.Contains(RiderStatus.Riding)) //if there are no riders still riding
        {
            routes[currentRider - 1] = (currentRiderScript.GetRoute());
            if (currentRider == numberOfRiders) //If this is the final rider
            {
                GameWin();
            }
            else
            {
                Debug.Log("Moving to next rider");
                currentRider++;
                gameBoardScript.clearTileLists();
                DestroyRiders();//garbage collect all the current riders
                InitaliseRiders();
            }
        }
    }
    public void GameTickUpdate() //A method that runs all the game computation that should be run on each turn the player makes (rider collision) 
    {
        Debug.Log("Game tick update");
        GameObject[,] tileArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        MoveRiderReplays(); //everytime the player moves and it was a valid move, move the riders
        foreach (GameObject t in tileArray)
        {

            if (t.GetComponent<TileScript>().GetTileType() == TileType.Finish && t.GetComponent<TileScript>().GetObjectList().Count == 1 && t.GetComponent<TileScript>().GetRiderID() == currentRider)
            {
                List<Direction> route = currentRiderInstance.GetComponent<RiderScript>().GetRoute();
                Debug.Log("current rider is complete");
                isRiderDone[currentRider - 1] = RiderStatus.Complete;
                time = time + 1f * Time.deltaTime;
                Debug.Log(time);

                isAutoPlaying = true;
                //GameWin();
            }
            else if (t.GetComponent<TileScript>().GetObjectList().Count >= 2)
            {
                Debug.Log("Collision");
                GameOver();
                Debug.Log("x = " + t.transform.position.x + "| y = " + t.transform.position.y);
            }
        }
        GameTick();
    }

    private void inputCheck()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow) == true)
        {
            currentRiderScript.SetDirection(Direction.Right);
            currentRiderScript.UpdateRider(Direction.Right);
        }
        else
            if (Input.GetKeyDown(KeyCode.LeftArrow) == true)
        {
            currentRiderScript.SetDirection(Direction.Left);
            currentRiderScript.UpdateRider(Direction.Left);
        }
        else
            if (Input.GetKeyDown(KeyCode.UpArrow) == true)
        {
            currentRiderScript.SetDirection(Direction.Up);
            currentRiderScript.UpdateRider(Direction.Up);
        }
        else
            if (Input.GetKeyDown(KeyCode.DownArrow) == true)
        {
            currentRiderScript.SetDirection(Direction.Down);
            currentRiderScript.UpdateRider(Direction.Down);
        }
    }


    //progresses all the riders by one step
    private void MoveRiderReplays()
    {
        for (int i = 0; i < numberOfRiders; i++)
        {
            if (routes[i] != null)
            {
                Debug.Log("Rider -" + i + " has a route");
                if (routes[i].Count > turnCount) //if the rider has finished
                {
                    AllRiders[i].GetComponent<RiderScript>().moveRider(routes[i].ElementAt(turnCount));
                    if (routes[i].Count == turnCount + 1) //if this is the last move, complete the ride 
                    {
                        isRiderDone[i] = RiderStatus.Complete;
                    }
                }
            }
        }
        turnCount++;
    }

    private IEnumerator Wait() 
    {
        yield return new WaitForSeconds(5);
    }
    private void AutoPlay() //This method is called when the current rider finishes its journey. It steps through the replay riders remaining steps at a set rate
    {

        if (isRiderDone.Contains(RiderStatus.Riding))
        {
            //Instantiate(DelayPrefab);
            for (int i = 0; i < numberOfRiders; i++)
            {
                if (isRiderDone[i] == RiderStatus.Riding)
                {
                    MoveRiderReplays();
                }
            }
        }
        else 
        {
            isAutoPlaying = false;
        }
    }
    public void GameOver() 
    {
        GameOverScreen.Setup();
    }

    public void GameWin() 
    {
        GameWinScreen.Setup();
    }

    public void Exit()
    {
        Application.Quit();
    }
    //public List<GameObject> GetRiders() 
    //{
    //    return allRiders;
    //}

    public GameObject[] GetRiders()
    {
        return AllRiders;
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
}
