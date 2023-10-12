using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Road,
    Wall,
    Spawn,
    Finish
}

public class TileScript : MonoBehaviour
{
    public GameObject RoadPrefab;
    public GameObject WallPrefab;
    public GameObject SpawnPrefab;
    public GameObject FinishPrefab;

    GameObject renderedTile;

    private int riderID = 0;
    private Color32 colour;
    private TileType Ttype;
    private List<GameObject> ObjectList;


    void Awake()
    {
        ObjectList = new List<GameObject>();

    }
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Tile Start ----------");
        if (Ttype == TileType.Road)
        {
            renderedTile = Instantiate(RoadPrefab, transform);

        }
        else if (Ttype == TileType.Wall)
        {
            renderedTile = Instantiate(WallPrefab, transform);
        }
        else if (Ttype == TileType.Spawn)
        {
            renderedTile = Instantiate(SpawnPrefab, transform);
            renderedTile.GetComponent<SpriteRenderer>().color = colour; //sets the colour of the sprite render
        }
        else if (Ttype == TileType.Finish)
        {
            renderedTile = Instantiate(FinishPrefab, transform);
            renderedTile.GetComponent<SpriteRenderer>().color = colour; //sets the colour of the sprite render
        }
    }

    public void SetTileType(TileType type)
    {
        Ttype = type;
    }

    public TileType GetTileType()
    {
        return Ttype;
    }
    public GameObject GetRenderedTile() //returns the reference of the object that physically represents this tile 
    {
        return renderedTile;
    }

    public void AddObject(GameObject obj)
    {
        ObjectList.Add(obj);
    }

    public void RemoveObject(GameObject obj) //removed the first instance of the object parameter in the list
    {
        ObjectList.Remove(obj);
    }

    public void ClearObjectList() 
    {
        ObjectList.Clear();
    }
    public void setColour(Color32 c)
    {
        colour = c;
    }
    public List<GameObject> GetObjectList()
    {
        return ObjectList;
    }

    public int GetRiderID() 
    {
        return riderID;
    }
    public void SetRiderID(int n) 
    {
        riderID = n;
    }
}
