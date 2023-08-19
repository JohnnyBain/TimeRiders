using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardScript : MonoBehaviour
{
    TileScript bts;


    public GameObject Tile;
    public int BoardSize;
    private GameObject[,] TileArray;

    // Start is called before the first frame update
    void Start()
    {
        InitialiseBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitialiseBoard()
    {
        Debug.Log("initialising board");
        TileArray = new GameObject[BoardSize, BoardSize];
        for (int i = 0; i < BoardSize; i++) 
        {
            for (int j = 0; j < BoardSize; j++) 
            {

                TileArray[i,j] = (GameObject)Instantiate(Tile, new Vector3(i, j, 0), Quaternion.identity);
                Debug.Log("creating tile");
            }
        }
    }
}
