using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailManagerScript : MonoBehaviour
{
    public GameObject Trail;
    public GameObject GameBoard;

    private GameManagerScript GMScript;
    private RiderScript RiderScript;

    public List<GameObject> RiderTrail = new List<GameObject>();
    private GameObject[,] TileArray;

    private int targetTrailLength;
    private Color32 colour;


    
    
    List<Vector3> TrailVectorLocations;


    

    // Start is called before the first frame update
    void Start()
    {
        GMScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        //RiderScript = GMScript.GetCurrentRider().GetComponent<RiderScript>();
        //targetTrailLength = RiderScript.GetTrailLength();
        TileArray = GMScript.GetGameBoard().GetComponent<GameBoardScript>().GetTileArray();
    }

    private void OnDestroy()
    {
        Debug.Log("Trail Manager: OnDestroy");
        foreach (GameObject trail in RiderTrail) 
        {
            Debug.Log("Destroying trail piece");
            Destroy(trail);
        }
    }
    public void manageTrail(Direction direction)
    {
        if (RiderTrail.Count < targetTrailLength)
        {
            Debug.Log("Trail count = " + RiderTrail.Count);
            addTrail();
        }
        else
        {
            moveTrail();
        }
        //printTrail();



    }



    // addTrail is called to add an entity to the end of the line 
    private void addTrail()
    {
        GameObject trail = Instantiate(Trail, new Vector3(RiderScript.GetXLocation(), (int)RiderScript.GetYLocation(), 0), transform.rotation); //create trail piece
        trail.GetComponent<SpriteRenderer>().color = colour;
        RiderTrail.Insert(0, trail); //add it to the start of the list
        TileArray[(int)RiderScript.GetXLocation(), (int)RiderScript.GetYLocation()].GetComponent<TileScript>().AddObject(trail);
    }

    private void moveTrail()
    {
        //move the first trail piece to the rider (saving a temp location)
        Vector3 temp = RiderTrail[0].transform.position; //storing the location of the first piece of trail
        TileArray[(int)RiderTrail[0].transform.position.x, (int)RiderTrail[0].transform.position.y].GetComponent<TileScript>().RemoveObject(RiderTrail[0]);//removing the trail from the old tile (the tile it is currently on but will soon be off)
        RiderTrail[0].transform.position = new Vector3((int)RiderScript.GetXLocation(), (int)RiderScript.GetYLocation(),0); //move the trail to the rider
        TileArray[(int)RiderScript.GetXLocation(), (int)RiderScript.GetYLocation()].GetComponent<TileScript>().AddObject(RiderTrail[0]);//adding the trail to the Tile

        for (int i = 1; i < RiderTrail.Count; i++) //shift everyother piece of trail up to the next spot 
        {
            Vector3 temp2 = RiderTrail[i].transform.position;
            TileArray[(int)RiderTrail[i].transform.position.x, (int)RiderTrail[i].transform.position.y].GetComponent<TileScript>().RemoveObject(RiderTrail[i]);//removing the trail from the old tile (the tile it is currently on but will soon be off)
            RiderTrail[i].transform.position = temp;
            TileArray[(int)RiderTrail[i].transform.position.x, (int)RiderTrail[i].transform.position.y].GetComponent<TileScript>().AddObject(RiderTrail[i]);//adding the trail to the Tile
            temp = temp2;
        }
    }
    private void printTrail()
    {
        Debug.Log("printing trail locations");
        foreach (GameObject trail in RiderTrail)
        {
            Debug.Log("Trail Location - [" + trail.transform.position.x + "][" + trail.transform.position.y + "]");
        }
    }

    public void SetColour(Color32 c) 
    {
        colour = c;
    }

    public void setRiderScript(GameObject rider)
    {
        RiderScript = rider.GetComponent<RiderScript>();
        targetTrailLength = RiderScript.GetTrailLength();

    }
}
