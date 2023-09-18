using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    private List<GameObject> allRiders;
    private GameObject rider; 

    public GameObject MainCamera;
    public GameObject RiderPrefab;
    public GameObject GameBoardPrefab;
    public GameOverScreen GameOverScreen;
    public GameWinScreen GameWinScreen;

    private GameObject gameBoardInstance;

    //git push test

    RiderScript riderScript;
    // Start is called before the first frame update
    void Awake ()
    {
        Debug.Log("GameManager Awake ----------");
        GameObject camera = Instantiate(MainCamera, new Vector3(0,0,-10), transform.rotation);

        allRiders = new List<GameObject>();
        rider = Instantiate(RiderPrefab, new Vector3(1, 1, 0), transform.rotation);
        
        allRiders.Add(rider);
        Debug.Log(" ----------");
        gameBoardInstance = Instantiate(GameBoardPrefab, new Vector3(0, 0, 0), transform.rotation);
        //Fetching all the relevant scripts that we are going to be using 
        riderScript = rider.GetComponent<RiderScript>();
        riderScript.SetLocation(1,1);

        
        
    }

    // Update is called once per frame
    void Update()
    {
        inputCheck();
    }

    public void GameTickUpdate() //A method that runs all the game computation that should be run on each turn the player makes (rider collision) 
    {
        Debug.Log("Game tick update");
        GameObject[,] tileArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        foreach (GameObject t in tileArray)
        {
            if (t.GetComponent<TileScript>().GetTileType() == TileType.Finish && t.GetComponent<TileScript>().GetObjectList().Count == 1)
            {
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

    public GameObject GetMainRider() 
    {
        return rider;
    }

    public GameObject GetGameBoard() 
    {
        return gameBoardInstance;
    }
}
