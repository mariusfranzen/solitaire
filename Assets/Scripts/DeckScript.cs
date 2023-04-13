using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite CardBack;

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
        transform.GetComponent<SpriteRenderer>().sprite = CardBack;
    }
}
