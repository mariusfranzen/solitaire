using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("clicked!");
        transform.parent.GetComponent<BoardScript>().NextCard();
    }
}
