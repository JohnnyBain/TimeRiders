using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using System.Linq;

/* Description: This is a test to check that the correct behaviour is seen when
 *              a ride is complete. The game board should be reset and the 
 *              current rider in a new spawn location. The replay rider should have been
 *              spawned where the last riders route started.
 */

public class CompleteRideTest
{
    GameManagerScript gameManagerScript;
    MenuManagerScript menuManagerScript;
    PlayerRiderScript playerRiderScript;

    [OneTimeSetUp]
    public void OneTimeSetup()  //Setting up the test level
    {
        GameObject MenuManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Objects/MenuManager.prefab");
        menuManagerScript = GameObject.Instantiate(MenuManagerPrefab).GetComponent<MenuManagerScript>();
        
        menuManagerScript.LoadLevel(100); //Loading the test level
        gameManagerScript = menuManagerScript.GetGameInstance().GetComponent<GameManagerScript>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() //destroying the test objects after the test is complete
    {
        GameObject.Destroy(menuManagerScript.gameObject);
    }

    [UnityTest, Order(1)]
    public IEnumerator CheckLoad() //Checking the test level loaded the rider in the correct position
    {
        playerRiderScript = gameManagerScript.GetCurrentRider().GetComponent<PlayerRiderScript>();

        Assert.AreEqual(playerRiderScript.GetXLocation(), 5);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 8);

        yield return null; //yield to skip a frame.
    }

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

        //checking that the new rider has been spawned in the right position
        Assert.AreEqual(playerRiderScript.GetXLocation(), 6);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 5);

        //checking that the replay rider has been spawned where the previous ride started
        Assert.AreEqual(gameManagerScript.GetRiders().ElementAt(0).GetComponent<RiderScript>().GetXLocation(), 5);
        Assert.AreEqual(gameManagerScript.GetRiders().ElementAt(0).GetComponent<RiderScript>().GetYLocation(), 8);

        yield return null;
    }
}
