using UnityEngine;

public class DeckScript : MonoBehaviour
{
    void OnMouseDown()
    {
        transform.parent.GetComponent<BoardScript>().NextCard();
    }

    public void HideStack()
    {
        transform.GetComponent<SpriteRenderer>().sprite = null;
    }

    public void ShowStack()
    {
        transform.GetComponent<SpriteRenderer>().sprite = transform.parent.GetComponent<BoardScript>().SelectedCardBack;
    }
}
