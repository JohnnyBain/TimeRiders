using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class MoveRider
{
    GameBoardScript GameBoardScript;
    GameManagerScript gameManagerScript;
    MenuManagerScript menuManagerScript;

    [SetUp]
    public void Setup() 
    {

        GameObject MenuManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Objects/MenuManager.prefab");

        Debug.Log("Test Starting");
        
        MenuManagerScript menuManagerScript = GameObject.Instantiate(MenuManagerPrefab).GetComponent<MenuManagerScript>();

        menuManagerScript.LoadLevel(100);

        gameManagerScript = menuManagerScript.GetGameInstance().GetComponent<GameManagerScript>();
    }
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator MoveUp()
    {
        
        GameBoardScript gameBoardScript = gameManagerScript.GetGameBoard().GetComponent<GameBoardScript>();
        PlayerRiderScript playerRiderScript = gameManagerScript.GetCurrentRider().GetComponent<PlayerRiderScript>();

        //Debug.Log("Movement Test, starting X location = " + playerRiderScript.GetXLocation());
        Assert.AreEqual(playerRiderScript.GetXLocation(), 5);

        Assert.AreEqual(playerRiderScript.GetYLocation(), 8);



        //rider.GetComponent<RiderScript>().moveRider(Direction.None);


        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
