using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RiderSelectMenuScript : MonoBehaviour
{
    [SerializeField] GameObject RiderSelectorPrefab;
    [SerializeField] GameObject GridPrefab;

    GameManagerScript gameManagerScript;
    RiderSelectorScript riderSelectorScript;

    GameObject[] riders;
    GameObject currentGrid;

    //Generates the correct RiderSelector menu for the current level
    public void GenerateMenu() 
    {
        Destroy(currentGrid);
        currentGrid = Instantiate(GridPrefab, gameObject.transform);
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        riders = gameManagerScript.GetRiders();
        List<Color32> colours = gameManagerScript.GetColours();

        for (int riderCount = 0; riderCount < riders.Count(); riderCount++)
        {
            GameObject riderSelector = Instantiate(RiderSelectorPrefab, gameObject.transform);
            riderSelector.transform.SetParent(currentGrid.transform, false);
            riderSelectorScript = riderSelector.GetComponent<RiderSelectorScript>();
            riderSelectorScript.setRiderId(riderCount + 1);
            riderSelectorScript.SetColour(colours.ElementAt(riderCount));
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
}
