using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

/* Description: This is a test to check that the player cannot move into a space on the map where there is a wall.
 *              The updateRider call to move into the wall should be rejected and the players position should 
 *              remain the same.
 */
public class WallCollisionTest
{
    GameManagerScript gameManagerScript;
    MenuManagerScript menuManagerScript;
    PlayerRiderScript playerRiderScript;

    [OneTimeSetUp]
    public void OneTimeSetup() //Setting up the test level
    {
        GameObject MenuManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Objects/MenuManager.prefab");
        menuManagerScript = GameObject.Instantiate(MenuManagerPrefab).GetComponent<MenuManagerScript>();

        menuManagerScript.LoadLevel(100);
        gameManagerScript = menuManagerScript.GetGameInstance().GetComponent<GameManagerScript>();
        playerRiderScript = gameManagerScript.GetCurrentRider().GetComponent<PlayerRiderScript>();
    }

    [UnityTest]
    public IEnumerator WallCollideTest()
    {
        //moving the player down to where there is a wall below them 
        playerRiderScript.UpdateRider(Direction.Down);
        playerRiderScript.UpdateRider(Direction.Down);

        //trying to move the player into the space where there's a wall
        playerRiderScript.UpdateRider(Direction.Down);

        //there is a wall below (5,6) so the last MoveRider call should have not changed the riders position
        Assert.AreEqual(playerRiderScript.GetXLocation(), 5);
        Assert.AreEqual(playerRiderScript.GetYLocation(), 6);

        // Use yield to skip a frame.
        yield return null;
    }
}
