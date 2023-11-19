using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class CompleteLevelTest
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

        for (int i = 0; i < 5; i++)
        {
            playerRiderScript.UpdateRider(Direction.Right);
        }

        //Assert.AreEqual(menuManagerScript.transform.GetChild(3).GetComponent<GameWinScript>().isActiveAndEnabled,true);


        //UIcontroller.transform.GetChild(3).GetComponent<GameWinScript>().SetActive();


        yield return null;
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.

    }
}
