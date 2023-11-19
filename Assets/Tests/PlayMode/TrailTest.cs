using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using System.Linq;


/* Description: This is a test to check that the rider trail is functioning correctly.
 *              It checks that as a rider moves the length of the trail is as expected
 *              and the trail pieces are in the right locations on the game board.
 */
public class TrailTest
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
    public IEnumerator OneSegment()
    {
        //checking that the trail length is 0 before the player has moved 
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().Count, 0);

        playerRiderScript.UpdateRider(Direction.Right);

        //checking that the trail lenght is 1 after the player has moved
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().Count, 1);

        //checking that the trail segment is in the location that the player previously was
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.x, 5);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.y, 8);

        yield return null;
    }

    [UnityTest, Order(3)]
    public IEnumerator TwoSegment()
    {
        playerRiderScript.UpdateRider(Direction.Up);

        //checking that the trail lenght is 2 after the player has moved
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().Count, 2);

        //first segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.x, 6);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.y, 8);

        //second segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(1).transform.position.x, 5);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(1).transform.position.y, 8);

        yield return null;
    }

    [UnityTest, Order(4)]
    public IEnumerator ThreeSegment()
    {
        playerRiderScript.UpdateRider(Direction.Left);

        //checking that the trail lenght is 3 after the player has moved
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().Count, 3);

        //first segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.x, 6);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.y, 9);

        //second segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(1).transform.position.x, 6);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(1).transform.position.y, 8);

        //third segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(2).transform.position.x, 5);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(2).transform.position.y, 8);

        

        yield return null;
    }

    [UnityTest, Order(5)]
    public IEnumerator FullTrail()
    {
        //moving the player far enough that the trail starts dissapearing 
        playerRiderScript.UpdateRider(Direction.Up);
        playerRiderScript.UpdateRider(Direction.Right);
        playerRiderScript.UpdateRider(Direction.Right);
        playerRiderScript.UpdateRider(Direction.Right);

        //checking that the trail lenght is 6 after the player has moved
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().Count, 6);

        //first segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.x, 7);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.y, 10);

        //second segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(1).transform.position.x, 6);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(1).transform.position.y, 10);

        //third segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(2).transform.position.x, 5);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(2).transform.position.y, 10);

        //fourth segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(3).transform.position.x, 5);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(3).transform.position.y, 9);

        //fith segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(4).transform.position.x, 6);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(4).transform.position.y, 9);

        //sixth segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(5).transform.position.x, 6);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(5).transform.position.y, 8);

        yield return null;
    }
}
