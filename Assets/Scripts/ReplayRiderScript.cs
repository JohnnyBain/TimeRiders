
using System.Linq;
using UnityEngine;


public class ReplayRiderScript : RiderScript
{
    [SerializeField] GameObject GhostPointerPrefab;
    private GameObject GhostPointerInstance;
    private Vector2 GhostPointerPosition = new Vector2(0, 0);

    protected override void Start()
    {
        base.Start();       
    }

    public void SetPointerPosition(int xLocation, int yLocation) 
    {
        //GhostPointerPosition = Instantiate(GhostPointerPrefab)
        GhostPointerPosition.x = xLocation;
        GhostPointerPosition.y = yLocation;
    }

    //creates the ghost pointer and spawns it at the first position in that replay riders route list by adding the first direction to the replay riders current (starting) pos
    public void InitialiseGhostRider(int RiderID) 
    {
        GhostPointerInstance = Instantiate(GhostPointerPrefab, transform.position, transform.rotation);
        UpdateGhostPointerRider(gameManagerScript.GetRoutes()[RiderID].ElementAt(0));
        GhostPointerInstance.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color;
    }
    //updates the ghost point by moving it in the direction the replay is going (one step ahead)
    public void UpdateGhostPointerRider(Direction direction) 
    {
        GhostPointerInstance.transform.position = GhostPointerInstance.transform.position + ToVector3(direction);
        //Rotation update (by knowing what direction we're going in we can change the rotation of the triangle)

        switch (direction) // No default required as all potential enum options are accounted for 
        {
            case Direction.Right:
                GhostPointerInstance.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case Direction.Left:
                GhostPointerInstance.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.Up:
                GhostPointerInstance.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.Down:
                GhostPointerInstance.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
        }

    }

    private new void OnDestroy()
    {
        Destroy(GhostPointerInstance);
        base.OnDestroy();
    }

}

