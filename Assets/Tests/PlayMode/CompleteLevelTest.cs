using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;


/* Description: This is a test to check that the correct behaviour is seen when a level is completed.
 *              After all the rides in a level have been completed the game win screen should appear
 *              and the play state should turn to false.
 */
public class CompleteLevelTest
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
    public IEnumerator CompleteLevel()
    {
        //move the rider to their end point
        for (int i = 0; i < 5; i++)
        {
            playerRiderScript.UpdateRider(Direction.Right);
        }

        yield return null; //we need to yield here so that the new riders start methods have run before we access them

        //the playerRider has changed so we need the new script
        playerRiderScript = gameManagerScript.GetCurrentRider().GetComponent<PlayerRiderScript>();

        for (int i = 0; i < 5; i++)
        {
            playerRiderScript.UpdateRider(Direction.Right);
        }

        Assert.AreEqual(gameManagerScript.GetPlayingState(), false);
        Assert.AreEqual(menuManagerScript.GetUIController().transform.GetChild(3).GetComponent<GameWinScript>().isActiveAndEnabled, true);

        yield return null;
    }
}
