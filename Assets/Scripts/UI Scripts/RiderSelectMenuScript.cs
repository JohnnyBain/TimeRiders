using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public enum RiderSelectorMenuState  //An enum that describes whether a rider is currently spawned, has not be spawned yet, or has be spawned and is complete
{
    Riding,
    SelectRide
}
public class RiderSelectMenuScript : MonoBehaviour
{
    [SerializeField] GameObject RiderSelectorPrefab;
    [SerializeField] GameObject GridPrefab;

    GameManagerScript gameManagerScript;
    RiderSelectorScript riderSelectorScript;

    GameObject[] riders;
    GameObject currentGrid;
    List<GameObject> riderSelectors;

    //Generates the correct RiderSelector menu for the current level
    public void GenerateMenu() 
    {
        Destroy(currentGrid);
        currentGrid = Instantiate(GridPrefab, gameObject.transform);
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        riders = gameManagerScript.GetRiders();
        riderSelectors = new List<GameObject>();
        List<Color32> colours = gameManagerScript.GetColours();

        for (int riderCount = 0; riderCount < riders.Count(); riderCount++)
        {
            GameObject riderSelector = Instantiate(RiderSelectorPrefab, gameObject.transform);
            riderSelector.transform.SetParent(currentGrid.transform, false);
            riderSelectorScript = riderSelector.GetComponent<RiderSelectorScript>();
            riderSelectorScript.setRiderId(riderCount + 1);
            riderSelectorScript.SetColour(colours.ElementAt(riderCount));
            riderSelectors.Add(riderSelector);
        }
    }
    //sets this object (and by extension all its child rider selectors to active)
    public void SetActive() 
    {
        gameObject.SetActive(true);
    }

    //sets this object (and by extension all its child rider selectors to active)
    public void SetInactive()
    {
        gameObject.SetActive(false);
    }

    public void SetSelectorState(int riderId, bool isFull)
    {
        riderSelectors.ElementAt(riderId).GetComponent<RiderSelectorScript>().RideComplete(isFull);
    }

    public void SetRiderSelectorMenuState(RiderSelectorMenuState state) 
    {
        if (state == RiderSelectorMenuState.SelectRide)
        {
            foreach (GameObject riderSelector in riderSelectors)
            {
                var riderSelectorScript = riderSelector.GetComponent<RiderSelectorScript>();
                RiderSelectorStatus status = riderSelectorScript.GetSelectorStatus();
                if (status == RiderSelectorStatus.Waiting)
                {
                    riderSelectorScript.SetIsNudgeVisible(true);
                    riderSelectorScript.SetSelectorNudgeVisibility();
                }
            }
        }
        else 
        {
            foreach (GameObject riderSelector in riderSelectors)
            {
                var riderSelectorScript = riderSelector.GetComponent<RiderSelectorScript>();
                RiderSelectorStatus status = riderSelectorScript.GetSelectorStatus();
                riderSelectorScript.SetIsNudgeVisible(false);
                riderSelectorScript.SetSelectorNudgeVisibility();
            }
        }
        
    }
}
