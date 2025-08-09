using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    private int xLocation;
    private int yLocation;

    public void setLocation(int x, int y) 
    {
        xLocation = x;
        yLocation = y;
    }

    public int getXLocation() 
    {
        return xLocation;
    }

    private void OnDestroy() 
    {
        Debug.Log("Trail node destroy trace");
    }

    public int getYLocation()
    {
        return yLocation;
    }

    public void SetColour(Color32 c)
    {
        GetComponent<SpriteRenderer>().color = c;
    }
}
