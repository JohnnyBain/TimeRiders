using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class MovementValidation
{

    GameBoardScript gameBoardScript;
    GameManagerScript gameManagerScript;
    MenuManagerScript menuManagerScript;
    PlayerRiderScript playerRiderScript;

    

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest, Order(1)]
    public IEnumerator CheckLoad()
    {
        Debug.Log("Test Setup");
        GameObject MenuManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Objects/MenuManager.prefab");
        menuManagerScript = GameObject.Instantiate(MenuManagerPrefab).GetComponent<MenuManagerScript>();

        menuManagerScript.LoadLevel(100);

        gameManagerScript = menuManagerScript.GetGameInstance().GetComponent<GameManagerScript>();
        
        
        Debug.Log("Check Load test");
        playerRiderScript = gameManagerScript.GetCurrentRider().GetComponent<PlayerRiderScript>();

        Assert.AreEqual(playerRiderScript.GetXLocation(), 5);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 8);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest, Order(2) ]
    public IEnumerator MoveUp()
    {
        //gameBoardScript = gameManagerScript.GetGameBoard().GetComponent<GameBoardScript>();
        //PlayerRiderScript playerRiderScript = gameManagerScript.GetCurrentRider().GetComponent<PlayerRiderScript>();

        Debug.Log("Move Up test");
        playerRiderScript.MoveRider(Direction.Up);
        Assert.AreEqual(playerRiderScript.GetXLocation(), 5);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 9);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest, Order(3)]
    public IEnumerator MoveDown()
    {
        //gameBoardScript = gameManagerScript.GetGameBoard().GetComponent<GameBoardScript>();
        //PlayerRiderScript playerRiderScript = gameManagerScript.GetCurrentRider().GetComponent<PlayerRiderScript>();

        playerRiderScript.MoveRider(Direction.Down);
        Assert.AreEqual(playerRiderScript.GetXLocation(), 5);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 8);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest, Order(4)]
    public IEnumerator MoveRight()
    {

        //gameBoardScript = gameManagerScript.GetGameBoard().GetComponent<GameBoardScript>();
        //PlayerRiderScript playerRiderScript = gameManagerScript.GetCurrentRider().GetComponent<PlayerRiderScript>();

        playerRiderScript.MoveRider(Direction.Right);
        Assert.AreEqual(playerRiderScript.GetXLocation(), 6);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 8);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

    [UnityTest, Order(4)]
    public IEnumerator MoveLeft()
    {
        //gameBoardScript = gameManagerScript.GetGameBoard().GetComponent<GameBoardScript>();
        //PlayerRiderScript playerRiderScript = gameManagerScript.GetCurrentRider().GetComponent<PlayerRiderScript>();

        playerRiderScript.MoveRider(Direction.Left);
        Assert.AreEqual(playerRiderScript.GetXLocation(), 5);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 8);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
