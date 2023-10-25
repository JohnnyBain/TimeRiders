using System.Collections.Generic;
using UnityEngine;

public class PlayerRiderScript : RiderScript
{
    private List<TileType> ValidMoveTiles = new List<TileType> { TileType.Road, TileType.Spawn, TileType.Finish }; //a list of valid tile types that a player can move onto
    
    

    /* UpdateRider:
    * [direction] - The direction the rider should move in 
    * 
    * Description: This method checks if the direction is valid. If it is it moves the rider and then calls GameTickUpdate which runs the post move logic
    *              Unlike ReplayRiders, when the Player rider moves the whole game takes a step forward (hence GameTickUpdate)
    */
    public void UpdateRider(Direction direction) 
    {
        if (ValidMoveCheck(direction))
        {
            MoveRider(direction);
            gameManagerScript.GameTickUpdate();
        }
    }


    /* ValidMoveCheck:
     * [direction] - The direction the rider should move in 
     * 
     * Description: This method takes uses the x and why coordinates that the rider is currently at to check the tile type of the tile in the direction provided.
     *              If the type of that tile is contained within the ValidMoveTiles then true is returned as the move is allowed
     * 
     */
    bool ValidMoveCheck(Direction direction)
    {
        bool isValid = false;
        GameObject[,] TileArray = gameBoardInstance.GetComponent<GameBoardScript>().GetTileArray(); //loading the GameBoard Tile array locally
        switch (direction) // No default required as all potential enum options are accounted for 
        {
            case Direction.Right:
                //if the tile to the right is a road isValid = true
                if (ValidMoveTiles.Contains(TileArray[xLocation + 1, yLocation].GetComponent<TileScript>().GetTileType())) //Chekcs if the tile type of the tile the rider is going to is in the valid tiles set
                {
                    isValid = true;
                }
                break;
            case Direction.Left:
                if (ValidMoveTiles.Contains(TileArray[xLocation - 1, yLocation].GetComponent<TileScript>().GetTileType()))
                {
                    isValid = true;
                }
                break;
            case Direction.Up:
                if (ValidMoveTiles.Contains(TileArray[xLocation, yLocation + 1].GetComponent<TileScript>().GetTileType()))
                {
                    isValid = true;
                }
                break;
            case Direction.Down:
                if (ValidMoveTiles.Contains(TileArray[xLocation, yLocation - 1].GetComponent<TileScript>().GetTileType()))
                {
                    isValid = true;
                }
                break;
        }
        return isValid;
    }
    
}
