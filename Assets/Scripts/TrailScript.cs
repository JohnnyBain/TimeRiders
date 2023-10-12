using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    private int XLocation;
    private int YLocation;
  
    public int GetXLocation() 
    {
        return XLocation;
    }
    public int GetYLocation()
    {
        return YLocation;
    }

    public void SetColour(Color32 c)
    {
        GetComponent<SpriteRenderer>().color = c;
    }
}
