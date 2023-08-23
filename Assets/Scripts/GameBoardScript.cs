using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class GameBoardScript : MonoBehaviour
{
    public GameObject RiderPrefab;
    public GameObject TilePrefab;
    
    GameManagerScript GMScript;
    private GameObject riderInstance;
    private int BoardSize; //why is this public?
    private GameObject[,] TileArray;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("GameBoard Awake ----------");
        GMScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        riderInstance = GMScript.GetMainRider();
        
        InitialiseBoard();
    }

    private void Start()
    {
        Debug.Log("Game Board Start --------------");
    }
    // Update is called once per frame
    void Update()
    {

    }

    
    /* 
     This method creates a tileArray of tiles based on the values in a text file (x = wall, 0 = road)
  
     */
    void InitialiseBoard()
    {
        string readFromFilePath = Application.streamingAssetsPath + "/TextFiles/" + "testText" + ".txt";
        List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList();
        BoardSize = fileLines.First().Replace(" ", string.Empty).Length;
        Debug.Log("Board Size = " + BoardSize);
        this.TileArray = new GameObject[BoardSize, BoardSize];

        Debug.Log("initialising board");
        
        
        for (int i = 0; i < BoardSize; i++) 
        {
            string line = fileLines.ElementAt(i);
            for (int j = 0; j < BoardSize; j++)
            {
                    GameObject obj;
                    obj = (GameObject)Instantiate(TilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                    switch (line[j])
                    {
                        case 'x':
                        obj.GetComponent<TileScript>().SetTileType(TileType.Wall);
                        Debug.Log("Wall");
                        break;

                        case '0':
                        obj.GetComponent<TileScript>().SetTileType(TileType.Road);
                        Debug.Log("Road");
                        break;
                    }
                    TileArray[i, j] = obj; 
                    Debug.Log("creating tile");
            }
            
        }
        
        TileArray[1,1].GetComponent<TileScript>().AddObject(GMScript.GetMainRider());
    }


    public void CollisionCheck() 
    {
        Debug.Log("Collision check");
        
        bool result = false;

        for (int i = 0; i < BoardSize; i++) 
        {
            for (int j = 0; j < BoardSize; j++)
            {
                if (TileArray[i,j].GetComponent<TileScript>().GetObjectList().Contains(riderInstance))
                {
                    if (TileArray[i,j].GetComponent<TileScript>().GetTileType() == TileType.Road)
                    {
                        Debug.Log("Road");
                    }
                    else
                    {
                        Debug.Log("Wall");
                    }
                }
            }
        }
        //return result;
        
    }
    public GameObject[,] GetTileArray() 
    {
        return TileArray;
    }
}
