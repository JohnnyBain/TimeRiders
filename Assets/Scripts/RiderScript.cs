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
    protected GameManagerScript gameManagerScript;
    TrailManagerScript trailManagerScript;

    [SerializeField] protected GameObject TrailManagerPrefab;

    protected GameObject trailManagerInstance;
    protected GameObject gameBoardInstance;

    
    private List<Direction> route = new List<Direction>{ };
    private Color32 colour; //Colour of the rider
    protected int xLocation;
    protected int yLocation;

    [SerializeField] protected int trailLength;

    /* Awake:
     * Description: This method is called upon the creation of a rider 
     */
    void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>(); //creates a reference to the Game Manager Script to use in this class
        gameBoardInstance = gameManagerScript.GetGameBoard();
    }

   /* Start: 
    * Description: This method is called on the first frame update after the riders creation
    *              The reason this code is not in awake is because after a rider is instantiated it's passed it's location through the SetLocation method
    */
    protected virtual void Start()
    {
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
    protected void OnDestroy()
    {
        Destroy(trailManagerInstance);
    }

    /* MoveRider
     * [direction] - the direction the rider should be moved in
     * 
     * Description: This method moves the rider if the direction given and the current location on the board mean it's a valid move
     */
    public virtual void MoveRider(Direction direction)
    {
        GameObject[,] TileArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray();
        
        route.Add(direction); //add the move to the current route
        switch (direction)
        {
            case Direction.Right:
                trailManagerScript.ManageTrail();
                transform.position = transform.position + ToVector3(direction); //change the current position based on what the ToVector3 function decided
                xLocation++; //adjust the xLocation member variable to contain the new location
                TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject); //adding the rider to the new tile
                TileArray[xLocation - 1, yLocation].GetComponent<TileScript>().RemoveObject(gameObject); //removing the rider from the last tile
                break;
            case Direction.Left: //dito ( -1 in the x axis)
                trailManagerScript.ManageTrail();
                transform.position = transform.position + ToVector3(direction);
                xLocation--;
                TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject);
                TileArray[xLocation + 1, yLocation].GetComponent<TileScript>().RemoveObject(gameObject);
                break;
            case Direction.Up: //dito ( +1 in the y axis)
                trailManagerScript.ManageTrail();
                transform.position = transform.position + ToVector3(direction);
                yLocation++;
                TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject);
                TileArray[xLocation, yLocation - 1].GetComponent<TileScript>().RemoveObject(gameObject);
                break;
            case Direction.Down: //dito ( -1 in the y axis)
                trailManagerScript.ManageTrail();
                transform.position = transform.position + ToVector3(direction);
                yLocation--;
                TileArray[xLocation, yLocation].GetComponent<TileScript>().AddObject(gameObject);
                TileArray[xLocation, yLocation + 1].GetComponent<TileScript>().RemoveObject(gameObject);
                break;
            case Direction.None:

                Debug.Log("Log: No direction");
                break;
            default:
                Debug.LogError("Error: Not a valid direction");
                break;
            
        }
    }

    /* ToVector3
     * in:
     * [direction] - the direction the rider is moving in
     * 
     * out:
     * [directionVector] - the vector to move the rider in
     * 
     * Description: This method creates a Vector3 to be added to the riders current transform based on what direction it is trying to go in
     */
    protected Vector3 ToVector3(Direction direction) 
    {
        Vector3 directionVector = new Vector3(0, 0, 0); ;
        switch (direction)
        {
            case Direction.Right:
                directionVector = new Vector3(1, 0, 0);
                break;
            case Direction.Left:
                directionVector = new Vector3(-1, 0, 0);
                break;
            case Direction.Up:
                directionVector = new Vector3(0, 1, 0);
                break;
            case Direction.Down:
                directionVector = new Vector3(0, -1, 0);
                break;
            case Direction.None:
                Debug.LogError("No Transform for Direction.None, returning an empty Vector3" );
                break;
            default:
                Debug.LogError("Cannot Transoform this Direction, returning an empty Vector3");
                break;
        }
        return directionVector;
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

    public TrailManagerScript GetTrailManagerScript() 
    {
        return trailManagerScript;
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

    /* SetColour:
     * [desiredColour]
     * Description: Sets the colour of the sprite component attached to this rider
     */
    public void SetColour(Color32 desiredColour) 
    {
        colour = desiredColour;
        GetComponent<SpriteRenderer>().color = desiredColour; 
    }

}
