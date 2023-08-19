using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum tileType 
    {
        wall,
        road
    }
public class Tile
{
    tileType Type;

    Tile(tileType type) 
    {
        Type = type;
    }
}
