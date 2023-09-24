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
    private int RiderCount;
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
                GameObject tile;
                tile = (GameObject)Instantiate(TilePrefab, new Vector3(i, j, 0), Quaternion.identity);

                if (line[j] != 'x' && line[j] != '0')
                {
                    if (char.IsLower(line[j])) //if the char is lower case it is one of the riders spawns
                    {
                        switch (line[j])
                        {
                            case 'a':
                                tile.GetComponent<TileScript>().SetTileType(TileType.Road);
                                Debug.Log("Finish");
                                break;
                        }
                    }
                    else //if it is upper case it is one of the riders destinations
                    {
                        Debug.Log(line[j]);
                        switch (line[j])
                        {
                            case 'A':
                                tile.GetComponent<TileScript>().SetTileType(TileType.Finish);
                                tile.GetComponent<TileScript>().setColour(GMScript.GetColours().ElementAt(0));  
                                Debug.Log("Finish");
                                break;
                        }
                    }
                    //InitialiseSpecialTile(tile, line[j]);
                }
                else 
                {
                    switch (line[j])
                    {
                        case 'x':
                            tile.GetComponent<TileScript>().SetTileType(TileType.Wall);
                            Debug.Log("Wall");
                            break;

                        case '0':
                            tile.GetComponent<TileScript>().SetTileType(TileType.Road);
                            Debug.Log("Road");
                            break;

                    }
                    
                }
                TileArray[i, j] = tile;
                Debug.Log("creating tile");
            }
            
        }
        
        TileArray[1,1].GetComponent<TileScript>().AddObject(GMScript.GetMainRider());
    }

    /*
     * [specialTile] - the object reference to the tile in question
     * [c] - the char that represents the tile in the tile board text file 
     */
    private void InitialiseSpecialTile(GameObject specialTile, char c) // A method to deal with the initalisation of special tiles that are not roads or walls
    {
        if (char.IsLower(c)) //if the char is lower case it is one of the riders spawns
        {
            switch (c)
            {
                case 'a':
                    specialTile.GetComponent<TileScript>().SetTileType(TileType.Road);
                    Debug.Log("Finish");
                    break;
            }
        }
        else //if it is upper case it is one of the riders destinations
        {
            switch (c)
            {
                case 'A':
                    specialTile.GetComponent<TileScript>().SetTileType(TileType.Finish);
                    Debug.Log("Finish");
                    break;
            }
        }
        
       
    }
    public void CollisionCheck() 
    {
        Debug.Log("Collision check");
        
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
