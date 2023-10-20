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
    GameManagerScript gameManagerScript;
    TrailManagerScript trailManagerScript;

    [SerializeField] GameObject TrailManagerPrefab;

    GameObject trailManagerInstance;
    GameObject gameBoardInstance;

    private List<TileType> ValidMoveTiles = new List<TileType> { TileType.Road, TileType.Spawn, TileType.Finish };
    private List<Direction> route = new List<Direction>{ };

    private Direction previousDirection = Direction.None;
    private Color32 colour; //Colour of the rider
    private int xLocation;
    private int yLocation;

    [SerializeField] int trailLength;
    

    // Start is called before the first frame update
    void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>(); //creates a reference to the Game Manager Script to use in this class
        gameBoardInstance = gameManagerScript.GetGameBoard();
    }

   
    private void Start()
    {
        gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray()[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject); //adds this rider to the tile that it was spawned at
        trailManagerInstance = Instantiate(TrailManagerPrefab, transform.position, transform.rotation);// create trail manager with this riders current location and rotation
        trailManagerScript = trailManagerInstance.GetComponent<TrailManagerScript>(); //creating a reference of the trail manager script attached to this rider
        trailManagerScript.setRiderScript(gameObject);
        trailManagerScript.SetColour(colour); //sets the colour of the trail sprites to the same colour as the rider
    }
    private void OnDestroy()
    {
        Destroy(trailManagerInstance);
    }

    public void moveRider(Direction direction)
    {
        GameObject[,] TileArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        if (ValidMoveCheck(direction)) 
        {
            route.Add(direction);
            switch (direction)
            {
                case Direction.Right:
                    trailManagerScript.ManageTrail();
                    transform.position = transform.position + (new Vector3(1, 0, 0));
                    previousDirection = Direction.Right;
                    xLocation++;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject); //adding the rider to the new tile
                    TileArray[xLocation - 1, yLocation].GetComponent<TileScript>().RemoveObject(gameObject); //removing the rider from the last tile
                    break;
                case Direction.Left:
                    trailManagerScript.ManageTrail();
                    transform.position = transform.position + (new Vector3(-1, 0, 0));
                    previousDirection = Direction.Left;
                    xLocation--;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject);
                    TileArray[xLocation + 1, yLocation].GetComponent<TileScript>().RemoveObject(gameObject);
                    break;
                case Direction.Up:
                    trailManagerScript.ManageTrail();
                    transform.position = transform.position + (new Vector3(0, 1, 0));
                    previousDirection = Direction.Up;
                    yLocation++;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject);
                    TileArray[xLocation, yLocation - 1].GetComponent<TileScript>().RemoveObject(gameObject);
                    break;
                case Direction.Down:
                    trailManagerScript.ManageTrail();
                    transform.position = transform.position + (new Vector3(0, -1, 0));
                    previousDirection = Direction.Down;
                    yLocation--;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject);
                    TileArray[xLocation, yLocation + 1].GetComponent<TileScript>().RemoveObject(gameObject);
                    break;
                case Direction.None:

                    Debug.Log("Log: No direction");
                    break;
                default:
                    Debug.LogError("Error: Direction is not recognised as a valid");
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
        GameObject[,] tArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        if (ValidMoveCheck(direction)) 
        {
            moveRider(direction);
            gameManagerScript.GameTickUpdate();
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
        GameObject[,] TileArray  = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray(); //loading the GameBoard Tile array locally
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
