using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using System.Linq;

public class CompleteRideTest
{
    GameManagerScript gameManagerScript;
    MenuManagerScript menuManagerScript;
    PlayerRiderScript playerRiderScript;

    [OneTimeSetUp]
    public void OneTimeSetup() 
    {

        Debug.Log("Test Setup");
        GameObject MenuManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Objects/MenuManager.prefab");
        menuManagerScript = GameObject.Instantiate(MenuManagerPrefab).GetComponent<MenuManagerScript>();
        menuManagerScript.LoadLevel(100); //Loading the test level
       
        gameManagerScript = menuManagerScript.GetGameInstance().GetComponent<GameManagerScript>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() 
    {
        GameObject.Destroy(menuManagerScript.gameObject);
    }

    [UnityTest, Order(1)]
    public IEnumerator CheckLoad()
    {
        Debug.Log("Check Load test");
        playerRiderScript = gameManagerScript.GetCurrentRider().GetComponent<PlayerRiderScript>();

        Assert.AreEqual(playerRiderScript.GetXLocation(), 5);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 8);

        yield return null; //yield to skip a frame.
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest, Order(2)]
    public IEnumerator CompleteRide()
    {
        
        //move the rider to their end point
        for (int i = 0; i < 5; i++) 
        {
            playerRiderScript.UpdateRider(Direction.Right);
        }

        //the playerRider has changed so we need the new script
        playerRiderScript = gameManagerScript.GetCurrentRider().GetComponent<PlayerRiderScript>();

        Assert.AreEqual(playerRiderScript.GetXLocation(), 6);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 5);

        Assert.AreEqual(gameManagerScript.GetRiders().ElementAt(0).GetComponent<RiderScript>().GetXLocation(), 5);
        Assert.AreEqual(gameManagerScript.GetRiders().ElementAt(0).GetComponent<RiderScript>().GetYLocation(), 8);

        yield return null;
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.

    }
}
