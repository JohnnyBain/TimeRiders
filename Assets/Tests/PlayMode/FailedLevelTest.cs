using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

/* Description: This is a test to check that the correct behaviour is seen when the game
 *              end due to a collision. In this test the player rider is turned in a circle 
 *              so they collide with their own trail. After this the menu is checked and 
 *              if the game over menu is being displayed the test passes.
 */
public class FailedLevelTest
{
    GameManagerScript gameManagerScript;
    MenuManagerScript menuManagerScript;
    PlayerRiderScript playerRiderScript;

    [OneTimeSetUp]
    public void OneTimeSetup() //Setting up the test level
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

        yield return null;
    }

    [UnityTest, Order(2)]
    public IEnumerator FailedLevel()
    {
        //moving the rider in a circle so that they collide with their trail
        playerRiderScript.UpdateRider(Direction.Right);
        playerRiderScript.UpdateRider(Direction.Up);
        playerRiderScript.UpdateRider(Direction.Left);
        playerRiderScript.UpdateRider(Direction.Down);

        //checking that the game is not being played and that the GameOver menu is being displayed
        Assert.AreEqual(gameManagerScript.GetPlayingState(), false);
        Assert.AreEqual(menuManagerScript.GetUIController().transform.GetChild(2).GetComponent<GameOverScript>().isActiveAndEnabled, true);

        yield return null;
    }
}
