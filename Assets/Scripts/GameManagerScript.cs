using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManagerScript : MonoBehaviour
{
   

    public GameObject MainCamera;
    public GameObject RiderPrefab;
    public GameObject GameBoardPrefab;
    public GameOverScreen GameOverScreen;
    public GameWinScreen GameWinScreen;

    private GameObject currentRiderInstance;
    private GameObject gameBoardInstance;

    private GameBoardScript gameBoardScript;
    private RiderScript riderScript;

    public List<Color32> Colours;
    private List<GameObject> allRiders;
    private List<Direction>[] routes;
    private int numbetOfRiders;
    private int currentRider = 3; // determines which rider the player is currently in control of

    

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

    



    // Start is called before the first frame update
    void Awake ()
    {
        Debug.Log("GameManager Awake ----------");
        GameObject camera = Instantiate(MainCamera, new Vector3(4,(float)4.5,-10), transform.rotation);

        CreateBoardInstance();// creates a board (each time a new ride begins the board is recreated)

        InitaliseRiders();

        numbetOfRiders = gameBoardScript.GetRiderCount();

        Debug.Log("rider count = " + numbetOfRiders);

        riderScript = currentRiderInstance.GetComponent<RiderScript>();
        //riderScript.SetLocation(1, 1);
        //riderScript.SetColour(Colours.ElementAt(0));

        allRiders = new List<GameObject>();
        allRiders.Add(currentRiderInstance);
        routes = new List<Direction>[numbetOfRiders]; //creating an array of lists to hold the routes of each rider

    }

    // Update is called once per frame
    void Update()
    {
        inputCheck();
    }


    private void CreateBoardInstance() 
    {
        gameBoardInstance = Instantiate(GameBoardPrefab, new Vector3(0, 0, 0), transform.rotation);
        gameBoardScript = gameBoardInstance.GetComponent<GameBoardScript>();
    }

    //after I've created the board I need to spawn the riders, this should look something like the following 
    //
    //  for each tile 
    //      if tile is a spawn tile
    //          if tile ID == current rider
    //              Spawn a playable rider
    //          else
    //              Spawn a replayRider(ID)
    //
    //
    private void InitaliseRiders()
    {
        GameObject[,] tileArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        int boardSize = gameBoardInstance.GetComponent<GameBoardScript>().GetBoardSize();
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++) 
            {
                if (tileArray[i, j].GetComponent<TileScript>().GetTileType() == TileType.Spawn) //if this tile is a spawn tile
                {
                    if (tileArray[i, j].GetComponent<TileScript>().GetRiderID() == currentRider) //if this is where the current rider needs to be spawned
                    {
                        currentRiderInstance = Instantiate(RiderPrefab, new Vector3(i, j, 0), transform.rotation);
                        currentRiderInstance.GetComponent<RiderScript>().SetLocation(i, j);
                        //Debug.Log("Rider colour = " + ();
                        currentRiderInstance.GetComponent<RiderScript>().SetColour(Colours.ElementAt(tileArray[i, j].GetComponent<TileScript>().GetRiderID() - 1)); //rider ID starts at 1 but the colour list starts at 0
                    }
                    else 
                    {
                        //spawn the correct replay rider
                    }
                }
            
            }
        }
    }

    public void GameTickUpdate() //A method that runs all the game computation that should be run on each turn the player makes (rider collision) 
    {
        Debug.Log("Game tick update");
        GameObject[,] tileArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        foreach (GameObject t in tileArray)
        {
            if (t.GetComponent<TileScript>().GetTileType() == TileType.Finish && t.GetComponent<TileScript>().GetObjectList().Count == 1 && t.GetComponent<TileScript>().GetRiderID() == currentRider)
            {
                Debug.Log("Route = ");
                List<Direction> route = currentRiderInstance.GetComponent<RiderScript>().GetRoute();
                GameWin();
            }
            else if (t.GetComponent<TileScript>().GetObjectList().Count >= 2)
            {
                //Debug.Log("Collision");
                GameOver();
            }
        }
    }

    private void inputCheck()
    {
        
        if (Input.GetKeyDown(KeyCode.RightArrow) == true)
        {
            riderScript.SetDirection(Direction.Right);
            riderScript.UpdateRider(Direction.Right);
        }
        else
            if (Input.GetKeyDown(KeyCode.LeftArrow) == true)
        {
            riderScript.SetDirection(Direction.Left);
            riderScript.UpdateRider(Direction.Left);
        }
        else
            if (Input.GetKeyDown(KeyCode.UpArrow) == true)
        {
            riderScript.SetDirection(Direction.Up);
            riderScript.UpdateRider(Direction.Up);
        }
        else
            if (Input.GetKeyDown(KeyCode.DownArrow) == true)
        {
            riderScript.SetDirection(Direction.Down);
            riderScript.UpdateRider(Direction.Down);
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
    public List<GameObject> GetRiders() 
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
}
