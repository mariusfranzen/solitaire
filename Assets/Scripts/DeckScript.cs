using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    void OnMouseDown()
    {
        transform.parent.GetComponent<BoardScript>().NextCard();
    }
}
