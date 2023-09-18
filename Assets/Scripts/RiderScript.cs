using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction
{
    None,
    Right,
    Left,
    Up,
    Down
}
public class RiderScript : MonoBehaviour
{
    GameManagerScript GMScript;
    TrailManagerScript TMScript;
    GameObject GameBoard;
    private Direction direction = Direction.None;
    private Direction previousDirection;
    public int stepSize;
    public int trailLength = 5;
    int xLocation;
    int yLocation;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Rider Awake ----------");
        GMScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    private void Start()
    {
        GameBoard = GMScript.GetGameBoard();
        TMScript = gameObject.GetComponent<TrailManagerScript>(); //gameObject gets the object this script is attached to
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void moveRider(Direction direction)
    {
        GameObject[,] TileArray = GameBoard.GetComponent<GameBoardScript>().GetTileArray();
        if (ValidMoveCheck(direction)) 
        {
            switch (direction)
            {
                case Direction.Right:
                    TMScript.manageTrail(Direction.Right);
                    transform.position = transform.position + (new Vector3(1, 0, 0) * stepSize);
                    previousDirection = Direction.Right;
                    xLocation++;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(GMScript.GetMainRider()); //adding the rider to the new tile
                    TileArray[xLocation - 1, yLocation].GetComponent<TileScript>().RemoveObject(GMScript.GetMainRider()); //removing the rider from the last tile
                    break;
                case Direction.Left:
                    TMScript.manageTrail(Direction.Left);
                    transform.position = transform.position + (new Vector3(-1, 0, 0) * stepSize);
                    previousDirection = Direction.Left;
                    xLocation--;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(GMScript.GetMainRider());
                    TileArray[xLocation + 1, yLocation].GetComponent<TileScript>().RemoveObject(GMScript.GetMainRider());
                    break;
                case Direction.Up:
                    TMScript.manageTrail(Direction.Up);
                    transform.position = transform.position + (new Vector3(0, 1, 0) * stepSize);
                    previousDirection = Direction.Up;
                    yLocation++;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(GMScript.GetMainRider());
                    TileArray[xLocation, yLocation - 1].GetComponent<TileScript>().RemoveObject(GMScript.GetMainRider());
                    break;
                case Direction.Down:
                    TMScript.manageTrail(Direction.Down);
                    transform.position = transform.position + (new Vector3(0, -1, 0) * stepSize);
                    previousDirection = Direction.Down;
                    yLocation--;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(GMScript.GetMainRider());
                    TileArray[xLocation, yLocation + 1].GetComponent<TileScript>().RemoveObject(GMScript.GetMainRider());
                    break;
            }
            GMScript.GameTickUpdate();
        }
       
    }

    public void UpdateRider(Direction direction)
    {
        GameObject[,] tArray = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<GameBoardScript>().GetTileArray();
        Debug.Log("wawa");
        moveRider(direction);
        GMScript.GetGameBoard().GetComponent<GameBoardScript>().CollisionCheck();

    }

    bool ValidMoveCheck(Direction direction) 
    {
        bool isValid = false;
        GameObject[,] TileArray  =GameBoard.GetComponent<GameBoardScript>().GetTileArray();
        switch (direction)
        {
            case Direction.Right:
                //if the tile to the right is a road isValid = true
                if (TileArray[xLocation + 1, yLocation].GetComponent<TileScript>().GetTileType() == TileType.Road || TileArray[xLocation + 1, yLocation].GetComponent<TileScript>().GetTileType() == TileType.Finish) 
                {
                    isValid = true;
                }
                break;
            case Direction.Left:
                if (TileArray[xLocation - 1, yLocation].GetComponent<TileScript>().GetTileType() == TileType.Road || TileArray[xLocation - 1, yLocation].GetComponent<TileScript>().GetTileType() == TileType.Finish)
                {
                    isValid = true;
                }
                break;
            case Direction.Up:
                if (TileArray[xLocation, yLocation + 1].GetComponent<TileScript>().GetTileType() == TileType.Road || TileArray[xLocation , yLocation + 1].GetComponent<TileScript>().GetTileType() == TileType.Finish)
                {
                    isValid = true;
                }
                break;
            case Direction.Down:
                if (TileArray[xLocation, yLocation - 1].GetComponent<TileScript>().GetTileType() == TileType.Road || TileArray[xLocation, yLocation - 1].GetComponent<TileScript>().GetTileType() == TileType.Finish)
                {
                    isValid = true;
                }
                break;
        }





        return isValid;
    }

    public Direction GetDirection()
    {
        return direction;
    }

    public void SetDirection(Direction D)
    {
        direction = D;
    }

    public void SetLocation(int x, int y) 
    {
        xLocation = x;
        yLocation = y;
    }

    public int GetXLocation() 
    {
        return xLocation;
    }

    public int GetYLocation()
    {
        return yLocation;
    }
    public int GetTrailLength() 
    {
        return trailLength;
    }

}
