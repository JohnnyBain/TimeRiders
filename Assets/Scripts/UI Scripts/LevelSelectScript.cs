using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectScript : MonoBehaviour
{
    [SerializeField] int level;
    GameManagerScript GMScript;
    // Start is called before the first frame update
    void Awake()
    {
        GMScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel() 
    {
        gameObject.SetActive(false); //deactives the button, not the screen
        
        GMScript.SetPlayingState(true);
    }
    public void LevelOneButtonPress() 
    {
        
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
