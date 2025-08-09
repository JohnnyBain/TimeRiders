using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum RiderSelectorStatus  //An enum that describes whether a rider is currently spawned, has not be spawned yet, or has be spawned and is complete
{
    Riding,
    Waiting,
    Complete
}

public class RiderSelectorScript : MonoBehaviour
{
    MenuManagerScript mainMenuScript;
    GameManagerScript gameManagerScript;

    GameObject RiderSelectorNudge;
    
    private int riderId;
    private bool isNudgeVisible;
    private RiderSelectorStatus selectorStatus = RiderSelectorStatus.Waiting;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuScript = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManagerScript>();
        gameManagerScript = mainMenuScript.GetGameInstance().GetComponent<GameManagerScript>();
        //gameObject.GetComponentInChildren<TMP_Text>().text = riderId.ToString(); //sets a number on the button
    }

    //Set the selector's button colour (with a slightly darkened colour for the buttons interaction states
    public void SetColour(Color32 desiredColour)
    {
        Color32 darkenedColour = new Color32((byte)Mathf.Max(desiredColour.r - 10,0), (byte)Mathf.Max(desiredColour.g - 10,0), (byte)Mathf.Max(desiredColour.b - 10,0), desiredColour.a);

        var desiredcolours = GetComponent<Button>().colors;
        desiredcolours.normalColor = desiredColour;
        desiredcolours.highlightedColor = darkenedColour;
        desiredcolours.pressedColor = darkenedColour;
        desiredcolours.selectedColor = darkenedColour;
        gameObject.GetComponent<Button>().colors = desiredcolours;
    }

    public void setRiderId(int riderID) 
    {
        riderId = riderID;
    }

    public int getRiderId() 
    {
        return riderId;
    }
    public bool GetIsNudgeVisible() 
    {
        return isNudgeVisible;
    }

    public void SetIsNudgeVisible(bool isVisible)
    {
         isNudgeVisible = isVisible;
    }
    public RiderSelectorStatus GetSelectorStatus() 
    {
        return selectorStatus;
    }
    public void SelectRider()
    {
        mainMenuScript.SelectRider(riderId);
        selectorStatus = RiderSelectorStatus.Riding;
        gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<RiderSelectMenuScript>().SetRiderSelectorMenuState(RiderSelectorMenuState.Riding);
        gameObject.transform.GetChild(1).GetComponent<ClearRideSelectorScript>().gameObject.SetActive(false);
    }

    public void RideComplete(bool isComplete) 
    {
        if (isComplete)
        {
            gameObject.GetComponent<Image>().fillCenter = true;
            selectorStatus = RiderSelectorStatus.Complete;
            gameObject.transform.GetChild(1).GetComponent<ClearRideSelectorScript>().gameObject.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<Image>().fillCenter = false;
            selectorStatus = RiderSelectorStatus.Waiting;
            gameObject.transform.GetChild(1).GetComponent<ClearRideSelectorScript>().gameObject.SetActive(false);
        }
    }

    public void SetSelectorNudgeVisibility() 
    {
        if (isNudgeVisible)
        {
            gameObject.transform.Find("RideSelectorNudge").gameObject.SetActive(true);
        }
        else 
        {
            gameObject.transform.Find("RideSelectorNudge").gameObject.SetActive(false);
        }
    }
}
