using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class GameBoardScript : MonoBehaviour
{
    [SerializeField] GameObject RiderPrefab;
    [SerializeField] GameObject TilePrefab;
    
    GameManagerScript gameManagerScript;
    MenuManagerScript menuManagerScript;

    private int BoardWidth;
    private int BoardHeight;
    private int RiderCount = 0;
    private GameObject[,] TileArray;


    /* Awake:
     * Description: This method is run immediately after an object with this script attached is initalised
     *              It creates references to the MenuManager and GameManager scripts to be used in this class
     *              From the Menu manager we get what level we're supposed to be loading 
     */
    void Awake()
    {
        menuManagerScript = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManagerScript>();
        int level = menuManagerScript.GetCurrentLevel(); //fetches the level it should be loading from the MenuManager

        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();

        InitialiseBoard(level);
    }


    /* OnDestroy:
     * Description: Called when the gameBoardScript is destroyed. Destroys all the objects this script has created (all the tiles in the array)
     * 
     */
    private void OnDestroy()
    {
        foreach (GameObject t in TileArray)
        {
            Destroy(t);
        }
    }

    /* InitaliaseBoard:
     * Description: This method instantiates tile objects to fill the TileArray
     *              It uses the boardsize counted in from the file to define the dimensions of the array
     */
    void InitialiseBoard(int levelToLoad)
    {
        try //error catching if the file doesn't load correctly
        {
            string readFromFilePath = Application.streamingAssetsPath + "/TextFiles/" + "Level " + levelToLoad.ToString() + ".txt"; //loads the contents of the level file into a string
            List<string> fileLines = File.ReadAllLines(readFromFilePath).ToList(); //loads each line from the file into a string within a list 
            BoardHeight = fileLines.First().Length; //the board is always square so the length of the first string in the list will also be the length of the list
            
            BoardWidth = fileLines.Count();
            Debug.Log("BoardHeight = " + BoardHeight);
            Debug.Log("BoardWidth = " + BoardWidth);
            TileArray = new GameObject[BoardWidth, BoardHeight]; //initialising the Tile board using the board size 
            menuManagerScript.ResetCamera(((float)(BoardWidth))/2,((float)(BoardHeight))/2);
            for (int i = 0; i < BoardWidth; i++) //traversing each line loaded from the file
            {
                string line = fileLines.ElementAt(i);
                for (int j = 0; j < BoardHeight; j++) //traversing each char within that line
                {
                    GameObject tile = Instantiate(TilePrefab, new Vector3(i, j, 0), Quaternion.identity); //creating the tile at the corresponding game space (i = x, j = y)

                    if (line[j] != 'x' && line[j] != '0') //if the tile to be loaded here is not a road(0) or wall(x) tile
                    {
                        InitialiseSpecialTile(tile, line[j]);
                    }
                    else
                    {
                        switch (line[j]) //assigning a tile type to the newly created tile based on what the corresponding location was in the file
                        {
                            case 'x':
                                tile.GetComponent<TileScript>().SetTileType(TileType.Wall);
                                break;

                            case '0':
                                tile.GetComponent<TileScript>().SetTileType(TileType.Road);
                                break;
                            default:
                                Debug.LogError("Error: tile type unrecognised");
                                break;
                        }
                    }
                    TileArray[i, j] = tile; //saving the new tile to the tileArray 
                }
            }
        }
        catch (Exception e) //if the file cannot be loaded
        {
            Debug.LogException(e, this);
        }
        
    }



    /* InitialiseSpecelTile:
     * [specialTile] - The object reference to the tile being created
     * [c] - The char from the corresponding file location that dictates what the tile will end up ass
     * 
     * Description: This method creates the tiles that are no roads or walls
     *              Each rider is connected to two locations on the board, a spawn location (denoted with a lower case letter) 
     *              and a finish location (denoted with an uppercase of the same letter)
     */
    private void InitialiseSpecialTile(GameObject specialTile, char c)
    {
        TileScript SpecialTileScript = specialTile.GetComponent<TileScript>();
        if (char.IsLower(c)) //if the char is lower case it is a spawn location
        {
            int ID = (int)c - 97; //a is now 0 (a = 97 in ascii) // TODO 97 to (int) 'a'

            SpecialTileScript.SetTileType(TileType.Spawn);
            SpecialTileScript.setColour(gameManagerScript.GetColours().ElementAt(ID)); //special tiles share the same colour as the rider that they are connected to 
            specialTile.transform.localScale = new Vector3((float)0.5, (float)0.5,1);
            SpecialTileScript.SetRiderID(ID + 1); //connects this tile to the rider that needs to spawn at it (riders start at 1)

        }
        else //if the char is upper case it is a finish location 
        {
            RiderCount++; //for every finish location on a level there needs to be another rider
            int ID = (int)c - 65; //a is now 0 (A = 65 in ascii) // TODO 97 to (int) 'A'
            SpecialTileScript.SetTileType(TileType.Finish);
            SpecialTileScript.setColour(gameManagerScript.GetColours().ElementAt(ID)); //special tiles share the same colour as the rider that they are connected to
            SpecialTileScript.SetRiderID(ID + 1); //connects this tile to the rider that needs to finish at it (riders start at 1)
        }
    }

    /* ClearTileLists:
     * Description: Clears the lists that contain which objects are currently in that tile
     */
    public void ClearTileLists()
    {
        foreach (GameObject tile in TileArray)
        {
            tile.GetComponent<TileScript>().ClearObjectList();
        }
    }

    //Getters ------------------------
    public GameObject[,] GetTileArray() 
    {
        return TileArray;
    }

    public int GetRiderCount() 
    {
        return RiderCount;
    }

    public int GetBoardWidth() 
    {
        return BoardWidth;
    }

    public int GetBoardHeight() 
    {
        return BoardHeight;
    }
}
