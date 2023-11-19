using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    public void SetColour(Color32 c)
    {
        GetComponent<SpriteRenderer>().color = c;
    }
}
