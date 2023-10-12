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
    GameManagerScript GameManagerScript;
    TrailManagerScript TrailManagerScript;

    [SerializeField] GameObject TrailManagerPrefab;

    GameObject trailManagerInstance;
    GameObject GameBoardInstance;

    private List<TileType> ValidMoveTiles = new List<TileType> { TileType.Road, TileType.Spawn, TileType.Finish };
    private List<Direction> route = new List<Direction>{ };

    private Direction previousDirection = Direction.None;
    private Direction direction = Direction.None;
    private Color32 colour; //Colour of the rider
    private int xLocation;
    private int yLocation;

    [SerializeField] int trailLength;
    

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Rider Awake ----------");
        GameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>(); //creates a reference to the Game Manager Script to use in this class
    }

    private void OnDestroy()
    {
        Debug.Log("RiderScript: onDestroy");
        Debug.Log("Destroying Trail");
        Destroy(trailManagerInstance);
    }
    private void Start()
    {
        GameBoardInstance = GameManagerScript.GetGameBoard();
        GameBoardInstance.GetComponent<GameBoardScript>().GetTileArray()[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject); //add this rider object to the tile array where it is spawned
        trailManagerInstance = Instantiate(TrailManagerPrefab, transform.position, transform.rotation);// create trail manager with this riders current location and rotation
        TrailManagerScript = trailManagerInstance.GetComponent<TrailManagerScript>();
        TrailManagerScript.setRiderScript(gameObject);
        TrailManagerScript.SetColour(colour); //sets the colour of the trail sprites to the same colour as the rider
    }

    public void moveRider(Direction direction)
    {
        GameObject[,] TileArray = GameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        if (ValidMoveCheck(direction)) 
        {
            route.Add(direction);
            switch (direction)
            {
                case Direction.Right:
                    TrailManagerScript.manageTrail(Direction.Right);
                    transform.position = transform.position + (new Vector3(1, 0, 0));
                    previousDirection = Direction.Right;
                    xLocation++;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject); //adding the rider to the new tile
                    TileArray[xLocation - 1, yLocation].GetComponent<TileScript>().RemoveObject(gameObject); //removing the rider from the last tile
                    break;
                case Direction.Left:
                    TrailManagerScript.manageTrail(Direction.Left);
                    transform.position = transform.position + (new Vector3(-1, 0, 0));
                    previousDirection = Direction.Left;
                    xLocation--;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject);
                    TileArray[xLocation + 1, yLocation].GetComponent<TileScript>().RemoveObject(gameObject);
                    break;
                case Direction.Up:
                    TrailManagerScript.manageTrail(Direction.Up);
                    transform.position = transform.position + (new Vector3(0, 1, 0));
                    previousDirection = Direction.Up;
                    yLocation++;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject);
                    TileArray[xLocation, yLocation - 1].GetComponent<TileScript>().RemoveObject(gameObject);
                    break;
                case Direction.Down:
                    TrailManagerScript.manageTrail(Direction.Down);
                    transform.position = transform.position + (new Vector3(0, -1, 0));
                    previousDirection = Direction.Down;
                    yLocation--;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject);
                    TileArray[xLocation, yLocation + 1].GetComponent<TileScript>().RemoveObject(gameObject);
                    break;
            }
        }
    }

    /* UpdateRider:
     * Direction - the direction the rider should move in 
     * 
     * Description: This method checks if the direction is valid. If it is it moves the rider and then calls GameTickUpdate which runs the post move logic
     * 
     */
    public void UpdateRider(Direction direction)
    {
        GameObject[,] tArray = GameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        if (ValidMoveCheck(direction)) 
        {
            moveRider(direction);
            GameManagerScript.GameTickUpdate();
        }
    }

    /* ValidMoveCheck:
     * Direction - the direction the rider should move in 
     * 
     * Description: This method takes uses the x and why coordinates that the rider is currently at to check the tile type of the tile in the direction provided.
     *               If the type of that tile is contained within the ValidMoveTiles then true is returned as the move is allowed
     * 
     */
    bool ValidMoveCheck(Direction direction) 
    {
        bool isValid = false;
        GameObject[,] TileArray  = GameBoardInstance.GetComponent<GameBoardScript>().GetTileArray(); //loading the GameBoard Tile array locally
        switch (direction) // No default required as all potential enum options are accounted for 
        {
            case Direction.Right:
                //if the tile to the right is a road isValid = true
                if (ValidMoveTiles.Contains(TileArray[xLocation + 1, yLocation].GetComponent<TileScript>().GetTileType())) //Chekcs if the tile type of the tile the rider is going to is in the valid tiles set
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

    //Getters ----------------------------
    public Direction GetDirection()
    {
        return direction;
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
    public List<Direction> GetRoute()
    {
        return route;
    }
    //Setters ----------------------------
    public void SetDirection(Direction D)
    {
        direction = D;
    }

    public void SetLocation(int x, int y) 
    {
        xLocation = x;
        yLocation = y;
    }


    public void SetColour(Color32 desiredColour) 
    {
        colour = desiredColour;
        GetComponent<SpriteRenderer>().color = desiredColour; //Sets the colour of the sprite component attached to this rider
    }

    

     

}
