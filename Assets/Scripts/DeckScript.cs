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
        transform.parent.GetComponent<BoardScript>().NextCard();
    }
}
