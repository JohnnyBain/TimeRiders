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

    GameObject trailManagerInstance;
    GameObject GameBoardInstance;

    public GameObject TrailManagerPrefab;

    List<TileType> ValidMoveTiles = new List<TileType> { TileType.Road, TileType.Spawn, TileType.Finish };
    private List<Direction> route = new List<Direction>{ };
    private Direction direction = Direction.None;
    private Direction previousDirection;
    private Color32 colour;
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
        GameBoardInstance = GMScript.GetGameBoard();
        GameBoardInstance.GetComponent<GameBoardScript>().GetTileArray()[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject); //add this rider object to the tile array where it is spawned
        trailManagerInstance = Instantiate(TrailManagerPrefab, transform.position, transform.rotation);
        TMScript = trailManagerInstance.GetComponent<TrailManagerScript>();
        TMScript.SetColour(colour); //sets the colour of the trail sprites to the same colour as the rider
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void moveRider(Direction direction)
    {
        GameObject[,] TileArray = GameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        if (ValidMoveCheck(direction)) 
        {
            route.Add(direction);
            switch (direction)
            {
                case Direction.Right:
                    TMScript.manageTrail(Direction.Right);
                    transform.position = transform.position + (new Vector3(1, 0, 0) * stepSize);
                    previousDirection = Direction.Right;
                    xLocation++;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(GMScript.GetCurrentRider()); //adding the rider to the new tile
                    TileArray[xLocation - 1, yLocation].GetComponent<TileScript>().RemoveObject(GMScript.GetCurrentRider()); //removing the rider from the last tile
                    break;
                case Direction.Left:
                    TMScript.manageTrail(Direction.Left);
                    transform.position = transform.position + (new Vector3(-1, 0, 0) * stepSize);
                    previousDirection = Direction.Left;
                    xLocation--;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(GMScript.GetCurrentRider());
                    TileArray[xLocation + 1, yLocation].GetComponent<TileScript>().RemoveObject(GMScript.GetCurrentRider());
                    break;
                case Direction.Up:
                    TMScript.manageTrail(Direction.Up);
                    transform.position = transform.position + (new Vector3(0, 1, 0) * stepSize);
                    previousDirection = Direction.Up;
                    yLocation++;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(GMScript.GetCurrentRider());
                    TileArray[xLocation, yLocation - 1].GetComponent<TileScript>().RemoveObject(GMScript.GetCurrentRider());
                    break;
                case Direction.Down:
                    TMScript.manageTrail(Direction.Down);
                    transform.position = transform.position + (new Vector3(0, -1, 0) * stepSize);
                    previousDirection = Direction.Down;
                    yLocation--;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(GMScript.GetCurrentRider());
                    TileArray[xLocation, yLocation + 1].GetComponent<TileScript>().RemoveObject(GMScript.GetCurrentRider());
                    break;
            }
            
        }
       
    }

    public void UpdateRider(Direction direction)
    {
        GameObject[,] tArray = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<GameBoardScript>().GetTileArray();
        moveRider(direction);
        GMScript.GameTickUpdate();
    }

    bool ValidMoveCheck(Direction direction) 
    {
        bool isValid = false;
        GameObject[,] TileArray  = GameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        switch (direction)
        {
            case Direction.Right:
                //if the tile to the right is a road isValid = true
                if (ValidMoveTiles.Contains(TileArray[xLocation + 1, yLocation].GetComponent<TileScript>().GetTileType())) 
                {
                    isValid = true;
                }
                break;
            case Direction.Left:
                if (ValidMoveTiles.Contains(TileArray[xLocation - 1, yLocation].GetComponent<TileScript>().GetTileType()))
                {
                    isValid = true;
                }
                break;
            case Direction.Up:
                if (ValidMoveTiles.Contains(TileArray[xLocation, yLocation + 1].GetComponent<TileScript>().GetTileType()))
                {
                    isValid = true;
                }
                break;
            case Direction.Down:
                if (ValidMoveTiles.Contains(TileArray[xLocation, yLocation - 1].GetComponent<TileScript>().GetTileType()))
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

    public void SetColour(Color32 c) 
    {
        colour = c;
        GetComponent<SpriteRenderer>().color = c; //sets the colour of the rider sprite
    }

    public List<Direction> GetRoute() 
    {
        return route;
    }

}
