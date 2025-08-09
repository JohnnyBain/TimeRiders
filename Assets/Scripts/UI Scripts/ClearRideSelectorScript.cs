using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearRideSelectorScript : MonoBehaviour
{
    GameObject riderSelector;

    GameManagerScript gameManagerScript;
    RiderSelectorScript riderSelectorScript;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        riderSelector = gameObject.transform.parent.gameObject;
        riderSelectorScript = riderSelector.GetComponent<RiderSelectorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearRider() 
    {
        Debug.Log("Clearing rider - " + riderSelectorScript.getRiderId());
        gameManagerScript.RemoveRider(riderSelectorScript.getRiderId());
        riderSelectorScript.SetIsNudgeVisible(true);
        riderSelectorScript.RideComplete(false);
    }
}
