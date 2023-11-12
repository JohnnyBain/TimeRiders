using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class RiderMovement
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

    [UnityTest, Order(2)]
    public IEnumerator MoveUp()
    {
        
        playerRiderScript.UpdateRider(Direction.Up);

        Assert.AreEqual(playerRiderScript.GetXLocation(), 5);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 9);

        yield return null;
    }

    [UnityTest, Order(3)]
    public IEnumerator MoveDown()
    {
        
        playerRiderScript.UpdateRider(Direction.Down);

        Assert.AreEqual(playerRiderScript.GetXLocation(), 5);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 8);

        yield return null;
    }

    [UnityTest, Order(4)]
    public IEnumerator MoveRight()
    {
        
        playerRiderScript.UpdateRider(Direction.Right);

        Assert.AreEqual(playerRiderScript.GetXLocation(), 6);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 8);

        yield return null;
    }

    [UnityTest, Order(5)]
    public IEnumerator MoveLeft()
    {
        
        playerRiderScript.UpdateRider(Direction.Left);

        Assert.AreEqual(playerRiderScript.GetXLocation(), 5);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 8);

        yield return null;
    }
}
