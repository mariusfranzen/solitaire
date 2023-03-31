using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public List<Sprite> CardSprites;
    public Enums.Suits Suit;
    public int Value; // 1 - 13

    void Start()
    {
    }

    void Update()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }


    public void SetCardValue(Enums.Suits suit, int value)
    {
        Suit = suit;
        Value = value;
        transform.GetComponent<SpriteRenderer>().sprite = CardSprites.ElementAt((int)suit * 13 + value - 1);
    }

    public void SetCardValue((Enums.Suits, int) card)
    {
        Suit = card.Item1;
        Value = card.Item2;
        transform.GetComponent<SpriteRenderer>().sprite = CardSprites.ElementAt((int)Suit * 13 + Value - 1);
    }

    /// <summary>
    /// Hides the card completely
    /// </summary>
    public void DeactivateCard()
    {
        transform.GetComponent<SpriteRenderer>().sprite = null;
    }

    /// <summary>
    /// Puts back the card completely
    /// </summary>
    public void ActivateCard()
    {
        SetCardValue(Suit, Value);
    }

    /// <summary>
    /// Flips the card with the backside up
    /// </summary>
    public void HideCard()
    {
        transform.GetComponent<SpriteRenderer>().sprite = CardSprites.Find(s => s.name.Equals("card_back"));
    }

    /// <summary>
    /// Flips the card to reveal suit and value
    /// </summary>
    public void RevealCard()
    {
        SetCardValue(Suit, Value);
    }
}
