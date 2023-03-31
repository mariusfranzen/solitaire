using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    public List<Sprite> CardSprites;
    public Enums.Suits Suit;
    public int Value; // 1 - 13

    private bool _active = false;
    private bool _inPlay = false;

    void Start()
    {
    }

    void Update()
    {
    }

    void OnMouseDrag()
    {
        if (_inPlay is false)
        {
            return;
        }
        Debug.Log("OnMouseDrag");
        Vector3 newPosition = transform.position;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.x = mousePosition.x;
        newPosition.y = mousePosition.y;
        transform.position = newPosition;
    }

    private void SetActive(bool active)
    {
        transform.gameObject.SetActive(active);
        _active = active;
        _inPlay = active;
        transform.GetComponent<BoxCollider2D>().isTrigger = active;
    }

    public void SetCardValue(Enums.Suits suit, int value)
    {
        Suit = suit;
        Value = value;
        transform.GetComponent<SpriteRenderer>().sprite = CardSprites.ElementAt((int)suit * 13 + value - 1);
        SetActive(true);
    }

    public void SetCardValue((Enums.Suits, int) card)
    {
        Suit = card.Item1;
        Value = card.Item2;
        transform.GetComponent<SpriteRenderer>().sprite = CardSprites.ElementAt((int)Suit * 13 + Value - 1);
        SetActive(true);
    }

    /// <summary>
    /// Hides the card completely
    /// </summary>
    public void DeactivateCard()
    {
        transform.GetComponent<SpriteRenderer>().sprite = null;
        SetActive(false);
    }

    /// <summary>
    /// Puts back the card completely
    /// </summary>
    public void ActivateCard()
    {
        SetCardValue(Suit, Value);
        SetActive(true);
    }

    /// <summary>
    /// Flips the card with the backside up
    /// </summary>
    public void HideCard()
    {
        transform.GetComponent<SpriteRenderer>().sprite = CardSprites.Find(s => s.name.Equals("card_back"));
        _inPlay = false;
    }

    /// <summary>
    /// Flips the card to reveal suit and value
    /// </summary>
    public void RevealCard()
    {
        SetCardValue(Suit, Value);
        _inPlay = true;
    }
}
