using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using System.Linq;
using System.Runtime.ExceptionServices;

public class TrailTest
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
    public IEnumerator OneSegment()
    {
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().Count, 0);

        playerRiderScript.UpdateRider(Direction.Right);

        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().Count, 1);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.x, 5);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.y, 8);


        yield return null;

    }

    [UnityTest, Order(3)]
    public IEnumerator TwoSegment()
    {
        playerRiderScript.UpdateRider(Direction.Up);

        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().Count, 2);

        //first segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.x, 6);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.y, 8);

        //Debug.Log("Second segment X = " + playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(1).transform.position.x);

        //second segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(1).transform.position.x, 5);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(1).transform.position.y, 8);

        yield return null;
    }

    [UnityTest, Order(4)]
    public IEnumerator ThreeSegment()
    {
        playerRiderScript.UpdateRider(Direction.Left);

        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().Count, 3);

        //first segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.x, 6);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(0).transform.position.y, 9);

        //second segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(1).transform.position.x, 6);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(1).transform.position.y, 8);

        //Debug.Log("Second segment X = " + playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(1).transform.position.x);

        //third segment
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(2).transform.position.x, 5);
        Assert.AreEqual(playerRiderScript.GetTrailManagerScript().GetTrail().ElementAt(2).transform.position.y, 8);

        

        yield return null;
    }

    [UnityTest, Order(5)]
    public IEnumerator FullTrail()
    {
        playerRiderScript.UpdateRider(Direction.Up);
        playerRiderScript.UpdateRider(Direction.Right);
        playerRiderScript.UpdateRider(Direction.Right);
        playerRiderScript.UpdateRider(Direction.Right);


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
