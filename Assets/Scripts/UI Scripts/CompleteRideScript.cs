using log4net.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteRideScript : MonoBehaviour
{
    [SerializeField] int riderID;

    MenuManagerScript mainMenuScript;
    GameManagerScript gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuScript = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManagerScript>();
        gameManagerScript = mainMenuScript.GetGameInstance().GetComponent<GameManagerScript>();
    }

    public void SelectRider()
    {
        mainMenuScript.SelectRider(riderID);
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }
}
