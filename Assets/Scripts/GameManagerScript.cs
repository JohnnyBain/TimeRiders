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

    private GameObject gameBoardInstance;



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

    }

    // Update is called once per frame
    void Update()
    {
        inputCheck();
    }

    private void inputCheck()
    {
        
        if (Input.GetKeyDown(KeyCode.RightArrow) == true)
        {
            GameObject[,] tArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
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
