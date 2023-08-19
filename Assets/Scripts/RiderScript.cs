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



    private Direction direction;
    private Direction previousDirection;
    public int stepSize;


    // Start is called before the first frame update
    void Start()
    {
        direction = Direction.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void moveRider(Direction direction)
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

    public void UpdateRider(Direction direction)
    {
        moveRider(direction);
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
