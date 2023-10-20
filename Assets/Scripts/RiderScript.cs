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

    private Color32 colour; //Colour of the rider
    private int xLocation;
    private int yLocation;

    [SerializeField] int trailLength;


    /* Awake:
     * Description: This method is called upon the creation of a rider 
     */
    void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>(); //creates a reference to the Game Manager Script to use in this class
    }


    /* Start: 
     * Description: This method is called on the first frame update after the riders creation
     *              The reason this code is not in awake is because after a rider is instantiated it's passed it's location through the SetLocation method
     */
    private void Start()
    {
        gameBoardInstance = gameManagerScript.GetGameBoard();
        gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray()[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject); //adds this rider to the tile that it was spawned at
        trailManagerInstance = Instantiate(TrailManagerPrefab, transform.position, transform.rotation);// create trail manager with this riders current location and rotation
        trailManagerScript = trailManagerInstance.GetComponent<TrailManagerScript>(); //creating a reference of the trail manager script attached to this rider
        trailManagerScript.setRiderScript(gameObject);
        trailManagerScript.SetColour(colour); //sets the colour of the trail sprites to the same colour as the rider
    }

    /* OnDestroy:
     * 
     * Description: This method destroys the trail manager associated with a rider when the rider is destroyed
     *              It is called when all the riders have finished and need to be reset
     */
    private void OnDestroy()
    {
        Destroy(trailManagerInstance);
    }

    /* MoveRider
     * direction - the direction the rider should be moved in
     * 
     * Description: This method moves the rider if the direction given and the current location on the board mean it's a valid move
     */
    public void MoveRider(Direction direction)
    {
        GameObject[,] TileArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        if (ValidMoveCheck(direction)) 
        {
            route.Add(direction); //add the move to the current route
            switch (direction)
            {
                case Direction.Right: 
                    trailManagerScript.ManageTrail();
                    transform.position = transform.position + (new Vector3(1, 0, 0)); //change the current position by +1 in the x axis
                    xLocation++; //adjust the xLocation member variable to contain the new location
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject); //adding the rider to the new tile
                    TileArray[xLocation - 1, yLocation].GetComponent<TileScript>().RemoveObject(gameObject); //removing the rider from the last tile
                    break;
                case Direction.Left: //dito ( -1 in the x axis)
                    trailManagerScript.ManageTrail();
                    transform.position = transform.position + (new Vector3(-1, 0, 0));
                    xLocation--;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject);
                    TileArray[xLocation + 1, yLocation].GetComponent<TileScript>().RemoveObject(gameObject);
                    break;
                case Direction.Up: //dito ( +1 in the y axis)
                    trailManagerScript.ManageTrail();
                    transform.position = transform.position + (new Vector3(0, 1, 0));
                    yLocation++;
                    TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject);
                    TileArray[xLocation, yLocation - 1].GetComponent<TileScript>().RemoveObject(gameObject);
                    break;
                case Direction.Down: //dito ( -1 in the y axis)
                    trailManagerScript.ManageTrail();
                    transform.position = transform.position + (new Vector3(0, -1, 0));
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
     * direction - the direction the rider should move in 
     * 
     * Description: This method checks if the direction is valid. If it is it moves the rider and then calls GameTickUpdate which runs the post move logic
     * 
     */
    public void UpdateRider(Direction direction)
    {
        GameObject[,] tArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        if (ValidMoveCheck(direction)) 
        {
            MoveRider(direction);
            gameManagerScript.GameTickUpdate();
        }
    }

    /* ValidMoveCheck:
     * direction - the direction the rider should move in 
     * 
     * Description: This method takes uses the x and why coordinates that the rider is currently at to check the tile type of the tile in the direction provided.
     *              If the type of that tile is contained within the ValidMoveTiles then true is returned as the move is allowed
     * 
     */
    bool ValidMoveCheck(Direction direction) 
    {
        bool isValid = false;
        GameObject[,] TileArray  = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray(); //loading the GameBoard Tile array locally
        switch (direction)
        {
            case Direction.Right:
                if (ValidMoveTiles.Contains(TileArray[xLocation + 1, yLocation].GetComponent<TileScript>().GetTileType())) //Checks if the tile type of the tile the rider is going to is in the valid tiles set
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
