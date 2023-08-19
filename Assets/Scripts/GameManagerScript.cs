using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    public GameObject MainCamera;
    public GameObject Rider;
    public GameObject GameBoard;


    RiderScript riderScript;
    // Start is called before the first frame update
    void Start()
    {
        GameObject camera = Instantiate(MainCamera, new Vector3(0,0,-10), transform.rotation) as GameObject;
        GameObject rider = Instantiate(Rider, new Vector3(0, 0, 0), transform.rotation) as GameObject;
        GameObject gameBoard = Instantiate(GameBoard, new Vector3(0, 0, 0), transform.rotation) as GameObject;
        
        
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

    
}
