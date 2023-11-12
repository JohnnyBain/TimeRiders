using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class WallCollisionTest
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

        menuManagerScript.LoadLevel(100);
        gameManagerScript = menuManagerScript.GetGameInstance().GetComponent<GameManagerScript>();
        playerRiderScript = gameManagerScript.GetCurrentRider().GetComponent<PlayerRiderScript>();
    }

    [UnityTest]
    public IEnumerator WallCollideTest()
    {
        playerRiderScript.UpdateRider(Direction.Down);
        playerRiderScript.UpdateRider(Direction.Down);
        playerRiderScript.UpdateRider(Direction.Down);
        Assert.AreEqual(playerRiderScript.GetXLocation(), 5);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 6); //there is a wall below (5,6) so the last MoveRider call should have not changed the riders position

        // Use yield to skip a frame.
        yield return null;
    }
}
