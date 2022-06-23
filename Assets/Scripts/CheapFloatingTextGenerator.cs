using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheapFloatingTextGenerator : MonoBehaviour
{
    public GameObject floatingText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GenerateText()
    {
        Instantiate(floatingText,this.transform.position, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
