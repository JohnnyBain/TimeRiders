using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Road,
    Wall,
    Finish
}

public class TileScript : MonoBehaviour
{
    public GameObject Road;
    public GameObject Wall;
    public GameObject Finish;
    GameObject renderedTile;

    private TileType Ttype;
    private List<GameObject> ObjectList;


    void Awake()
    {
        ObjectList = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Tile Start ----------");
        if (Ttype == TileType.Road)
        {
            renderedTile = Instantiate(Road, transform);
        }
        else if (Ttype == TileType.Wall)
        {
            renderedTile = Instantiate(Wall, transform);
        }
        else if (Ttype == TileType.Finish)
        {
            renderedTile = Instantiate(Finish, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public List<GameObject> GetObjectList() 
    {
        return ObjectList;
    }
}
