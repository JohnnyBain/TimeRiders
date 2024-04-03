using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using XDiffGui;

public class ReplayRiderScript : RiderScript
{
    [SerializeField] GameObject GhostPointerPrefab;
    private GameObject GhostPointerInstance;
    

    protected override void Start()
    {
        base.Start();       
    }

    Vector2 GhostPointerPosition = new Vector2(0,0);
    public void SetPointerPosition(int xLocation, int yLocation) 
    {
        //GhostPointerPosition = Instantiate(GhostPointerPrefab)
        GhostPointerPosition.x = xLocation;
        GhostPointerPosition.y = yLocation;
    }

    public void InitialiseGhostRider(int RiderID) 
    {
        Vector3 debugVar = ToVector3(gameManagerScript.GetRoutes()[RiderID].ElementAt(0));
        GhostPointerInstance = Instantiate(GhostPointerPrefab, transform.position + debugVar, transform.rotation);
    }
    public void UpdateGhostPointerRider(Direction direction) 
    {
        GhostPointerInstance.transform.position = GhostPointerInstance.transform.position + ToVector3(direction);
    }

    private void OnDestroy()
    {
        Destroy(GhostPointerInstance);
        base.OnDestroy();
    }
}
