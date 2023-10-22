using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class MoveRider
{
    GameObject GameManagerPrefab;
    GameObject MenuManagerPrefab;

    [SetUp]
    public void Setup() 
    {
        GameObject MenuManager = GameObject.Instantiate(MenuManagerPrefab);
        GameObject GameManager = GameObject.Instantiate(GameManagerPrefab);
    }
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator MoveUp()
    {
        var rider = new GameObject();
        rider.AddComponent<RiderScript>();

        

        //rider.GetComponent<RiderScript>().moveRider(Direction.None);

        
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
