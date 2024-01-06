using System.Collections;
using System.Collections.Generic;
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
        Debug.Log(ToVector3(GetRoute().ElementAt(1)));
        GhostPointerInstance = Instantiate(GhostPointerPrefab, transform.position , transform.rotation);
    }

    Vector2 GhostPointerPosition = new Vector2(0,0);
    public void SetPointerPosition(int xLocation, int yLocation) 
    {
        //GhostPointerPosition = Instantiate(GhostPointerPrefab)
        GhostPointerPosition.x = xLocation;
        GhostPointerPosition.y = yLocation;
    }
}
