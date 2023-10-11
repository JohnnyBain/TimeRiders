using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class GameBoardScript : MonoBehaviour
{
    public GameObject RiderPrefab;
    public GameObject TilePrefab;
    

    GameManagerScript GMScript;
    private GameObject riderInstance;
    private int BoardSize;
    private int RiderCount = 0;
    private GameObject[,] TileArray;
    

    // Start is called before the first frame update
    void Awake()
    {

        Debug.Log("GameBoard Awake ----------");
        GMScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        riderInstance = GMScript.GetCurrentRider();
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
                GameObject tile;
                tile = (GameObject)Instantiate(TilePrefab, new Vector3(i, j, 0), Quaternion.identity);

                if (line[j] != 'x' && line[j] != '0')
                {
                    InitialiseSpecialTile(tile, line[j]);
                }
                else 
                {
                    switch (line[j])
                    {
                        case 'x':
                            tile.GetComponent<TileScript>().SetTileType(TileType.Wall);
                            //Debug.Log("Wall");
                            break;

                        case '0':
                            tile.GetComponent<TileScript>().SetTileType(TileType.Road);
                            //Debug.Log("Road");
                            break;
                    }
                }
                TileArray[i, j] = tile;
            }
        }
    }

    /*
     * [specialTile] - the object reference to the tile in question
     * [c] - the char that represents the tile in the tile board text file 
     */
    private void InitialiseSpecialTile(GameObject specialTile, char c) // A method to deal with the initalisation of special tiles that are not roads or walls
    {
        TileScript SpecialTileScript = specialTile.GetComponent<TileScript>();
        if (char.IsLower(c)) //if the char is lower case it is one of the riders spawns (a = 97 in ascii)
        {
            int ID = (int)c - 97; //a is now 0 

            SpecialTileScript.SetTileType(TileType.Spawn);
            SpecialTileScript.setColour(GMScript.GetColours().ElementAt(ID));
            specialTile.transform.localScale = new Vector3((float)0.5, (float)0.5,1);
            SpecialTileScript.SetRiderID(ID + 1); //connects this tile to the rider that needs to spawn at it (riders start at 1)
            //Debug.Log("Spawn");

        }
        else //if it is upper case it is one of the riders destinations (A = 65 in ascii)
        {
            RiderCount++;
            int ID = (int)c - 65; //a is now 0
            SpecialTileScript.SetTileType(TileType.Finish);
            SpecialTileScript.setColour(GMScript.GetColours().ElementAt(ID));
            SpecialTileScript.SetRiderID(ID + 1); //connects this tile to the rider that needs to finish at it (riders start at 1)
            //Debug.Log("Finish");

        }
        
       
    }
    
    public GameObject[,] GetTileArray() 
    {
        return TileArray;
    }

    public int GetRiderCount() 
    {
        return RiderCount;
    }

    //clears the lists that contain which objects are currently in that tile
    public void clearTileLists() 
    {
        foreach (GameObject tile in TileArray) 
        {
            tile.GetComponent<TileScript>().ClearObjectList();
        }
    }
    public int GetBoardSize() 
    {
        return BoardSize;
    }
}
