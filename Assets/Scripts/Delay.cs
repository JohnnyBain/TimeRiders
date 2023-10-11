using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnDelay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnDelay() 
    {
        yield return new WaitForSeconds(2f);    
        Destroy(this); 
    }
}
