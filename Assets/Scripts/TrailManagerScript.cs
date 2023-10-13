using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailManagerScript : MonoBehaviour
{
    [SerializeField] GameObject Trail;
    [SerializeField] GameObject GameBoard;

    private GameManagerScript gameManagerScript;
    private RiderScript riderScript;

    private List<GameObject> riderTrail = new List<GameObject>();
    private GameObject[,] tileArray;
    private int targetTrailLength;
    private Color32 colour;


    /* Awake:
     * This method is called the moment a trail manager is created
     * It creates a reference to the tile array so that it can add and remove trail objects from the tiles as the trail moves
     */
    void Awake()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        tileArray = gameManagerScript.GetGameBoard().GetComponent<GameBoardScript>().GetTileArray(); //creates a reference for the TileArray in the GameBoard class
    }

    /* OnDestroy:
     * A method that destroys all the trail objects (nodes) that make up the rider trail when the trail manager is destroyed
     */
    private void OnDestroy()
    {
        foreach (GameObject trailNode in riderTrail) 
        {
            Destroy(trailNode);
        }
    }

    /* ManageTrail:
     * This method is called from RiderScript every time that rider makes a move.
     * It checks whether the trail has reached it's max length and either moves the existing trail pieces or adds a new one accordingly
     * Crucially the ManageTrail function is called right before the rider is moved
     */
    public void ManageTrail()
    {
        if (riderTrail.Count < targetTrailLength)
        {
            AddTrail();
        }
        else
        {
            MoveTrail();
        }
    }



    /* AddTrail:
     * Creates a new trail where the rider just was. The trail node is added to the tile array
     */
    private void AddTrail()
    {
        GameObject trail = Instantiate(Trail, new Vector3(riderScript.GetXLocation(), riderScript.GetYLocation(), 0), transform.rotation); //creates a trail node where the rider currently is (before it has moved to the new location)
        trail.GetComponent<SpriteRenderer>().color = colour; //sets the trail nodes colour to the same as the trail manager's
        riderTrail.Insert(0, trail); //add it to the start of the trail list
        tileArray[riderScript.GetXLocation(), riderScript.GetYLocation()].GetComponent<TileScript>().AddObject(trail); //adds the trail node object to the list of objects for the tile it is currently on
    }

    /* MoveTrail:
     * Moves the trail pieces shifting them all up by one. The first piece is moved to where the rider was and the rest shuffle up.
     */
    private void MoveTrail()
    {
        Vector3 temp = riderTrail[0].transform.position; //storing the location of the first piece of trail
        tileArray[(int)riderTrail[0].transform.position.x, (int)riderTrail[0].transform.position.y].GetComponent<TileScript>().RemoveObject(riderTrail[0]);//removing the trail node from it's old tile's object list
        riderTrail[0].transform.position = new Vector3(riderScript.GetXLocation(), riderScript.GetYLocation(),0); //moving the trail node to the riders location (before the rider has moved
        tileArray[riderScript.GetXLocation(), riderScript.GetYLocation()].GetComponent<TileScript>().AddObject(riderTrail[0]);//adding the trail to the new tile's object list

        for (int i = 1; i < riderTrail.Count; i++) //shift every other piece of trail up to the next spot 
        {
            Vector3 temp2 = riderTrail[i].transform.position; //storing the position a trail piece
            tileArray[(int)riderTrail[i].transform.position.x, (int)riderTrail[i].transform.position.y].GetComponent<TileScript>().RemoveObject(riderTrail[i]);//removing the trail node from it's old tile's object list
            riderTrail[i].transform.position = temp;//moving the trail node to it's predecessors locations 
            tileArray[(int)riderTrail[i].transform.position.x, (int)riderTrail[i].transform.position.y].GetComponent<TileScript>().AddObject(riderTrail[i]);//adding the trail to the new tile's object list
            temp = temp2; //converting the temps for the next cycle
        }
    }

    /* PrintTrail:
     * Prints the [x,y] location of each trail node currently in the Riders trail 
     */
    private void PrintTrail()
    {
        Debug.Log("printing trail locations");
        foreach (GameObject trailNode in riderTrail)
        {
            Debug.Log("Trail Location - [" + trailNode.transform.position.x + "][" + trailNode.transform.position.y + "]");
        }
    }

    /* SetColour:
     * Sets the colour of the Trail manager (this colour is then passed on to the trail nodes when they are spawned in
     * Called by RiderScript right after this TrailManager is created
     */
    public void SetColour(Color32 c) 
    {
        colour = c;
    }

    /* SetRiderScript:
     * rider - The Rider GameObject from which this trail manager was created and should be connected to
     * 
     * Sets the RiderScript member variable to a RiderScript
     * It's called in RiderScript right after the creation of the trail manager to assign a rider to the trail manager just created
     */
    public void setRiderScript(GameObject rider)
    {
        riderScript = rider.GetComponent<RiderScript>();
        targetTrailLength = riderScript.GetTrailLength(); //retrieves the desired length of the trail from the rider
    }
}
