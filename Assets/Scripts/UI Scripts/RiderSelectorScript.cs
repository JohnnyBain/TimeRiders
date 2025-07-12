using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RiderSelectorScript : MonoBehaviour
{
    MenuManagerScript mainMenuScript;
    GameManagerScript gameManagerScript;
    
    private int riderId;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuScript = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManagerScript>();
        gameManagerScript = mainMenuScript.GetGameInstance().GetComponent<GameManagerScript>();
        gameObject.GetComponentInChildren<TMP_Text>().text = riderId.ToString();
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

    public void SelectRider()
    {
        mainMenuScript.SelectRider(riderId);
    }
}
