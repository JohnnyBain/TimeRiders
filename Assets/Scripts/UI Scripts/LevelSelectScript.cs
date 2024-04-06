using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectScript : MonoBehaviour
{
    [SerializeField] int level;
    MenuManagerScript MMScript;
    // Start is called before the first frame update
    void Awake()
    {
        MMScript = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManagerScript>();
    }

    public void LoadLevel() 
    {
        MMScript.LoadLevel(level);
        //gameObject.SetActive(false); //deactives the button, not the screen
    }

    public void BackButtonPress() 
    {
        gameObject.SetActive(false);
        gameObject.transform.parent.GetChild(0).GetComponent<MainMenuScript>().SetActive();
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
