using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction
{
    None,
    Right,
    Left,
    Up,
    Down
}
public class RiderScript : MonoBehaviour
{
    GameManagerScript GMScript;
    private Direction direction = Direction.None;
    private Direction previousDirection;
    public int stepSize;


    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Rider Awake ----------");
        GMScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void moveRider(Direction direction)
    {
        if (ValidMoveCheck()) 
        {
            switch (direction)
            {
                case Direction.Right:
                    transform.position = transform.position + (new Vector3(1, 0, 0) * stepSize);
                    previousDirection = Direction.Right;
                    break;
                case Direction.Left:
                    transform.position = transform.position + (new Vector3(-1, 0, 0) * stepSize);
                    previousDirection = Direction.Left;
                    break;
                case Direction.Up:
                    transform.position = transform.position + (new Vector3(0, 1, 0) * stepSize);
                    previousDirection = Direction.Up;
                    break;
                case Direction.Down:
                    transform.position = transform.position + (new Vector3(0, -1, 0) * stepSize);
                    previousDirection = Direction.Down;
                    break;
            }
        }
       
    }

    public void UpdateRider(Direction direction)
    {
        GameObject[,] tArray = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<GameBoardScript>().GetTileArray();
        Debug.Log("wawa");
        moveRider(direction);
        GMScript.GetGameBoard().GetComponent<GameBoardScript>().CollisionCheck();
    }

    bool ValidMoveCheck() 
    {
        bool isValid = true;
        





        return isValid;
    }

    public Direction GetDirection()
    {
        return direction;
    }

    public void SetDirection(Direction D)
    {
        direction = D;
    }
}
