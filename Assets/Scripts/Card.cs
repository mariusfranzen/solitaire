using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    public List<Sprite> CardSprites;
    public Enums.Suits Suit;
    public int Value; // 1 - 13

    private Collider2D _collider;
    private Vector3 _originalPosition;

    private bool _active = false;
    private bool _inPlay = false;

    private bool _isDragging = false;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _originalPosition = transform.position;
    }

    void OnMouseDrag()
    {
        if (_inPlay is false)
        {
            return;
        }

        _isDragging = true;
        Vector3 newPosition = transform.position;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.x = mousePosition.x;
        newPosition.y = mousePosition.y;
        newPosition.z = -2;
        transform.position = newPosition;
    }

    void OnMouseUp()
    {
        if (_isDragging)
        {
            _isDragging = false;
            OnMouseDragEnd();
        }
    }

    GameObject GetClosestColumn()
    {
        var filter = new ContactFilter2D().NoFilter();
        var results = new List<Collider2D>();
        Physics2D.OverlapCollider(_collider, filter, results);
        results.RemoveAll(c => c.transform.name.Contains("card"));

        if (results.Count < 1)
        {
            return null;
        }

        var distances = results.Select(result => Physics2D.Distance(_collider, result).distance).ToList();
        return results.ElementAt(distances.IndexOfMin()).gameObject;
    }

    void OnMouseDragEnd()
    {
        // Current cards in column
        List<Transform> cards = new();
        bool lastCardActive = true;
        for (int i = 0; i < 13; i++)
        {
            var closestColumn = GetClosestColumn();
            if (closestColumn is null)
            {
                return;
            }

            if (closestColumn.name.Contains("Collections"))
            {
                if (PlaceCardInCollection())
                {
                    return;
                }
            }

            var card = closestColumn.transform.Find($"card{i}");
            if (lastCardActive)
            {
                cards.Add(card);
            }

            if (card.GetComponent<Card>()._active is false)
            {
                lastCardActive = false;
            }
        }

        transform.position = _originalPosition;
        var targetCard = cards.ElementAt(cards.Count - 2).GetComponent<Card>();

        if (IsValidPosition(targetCard.Suit, targetCard.Value))
        {
            cards.Last().GetComponent<Card>().SetCardValue(Suit, Value);
            DeactivateCard();
            int indexOfSibling = int.Parse(Regex.Replace(transform.name, @"[\D]", string.Empty)) - 1;
            transform.parent.Find($"card{indexOfSibling}").GetComponent<Card>().RevealCard();
        }
    }

    private bool PlaceCardInCollection()
    {
        var collections = transform.parent.parent.Find("Collections");
        int indexOfSibling = int.Parse(Regex.Replace(transform.name, @"[\D]", string.Empty)) - 1;
        Card collection;

        switch (Suit)
        {
            case Enums.Suits.Hearts:
                collection = collections.Find("heartsCollection").GetComponent<Card>();
                if (collection.Value != Value - 1 && Value != 1) return false;
                collection.SetCardValue(Suit, Value);
                DeactivateCard();
                transform.parent.Find($"card{indexOfSibling}").GetComponent<Card>().RevealCard();
                return true;

            case Enums.Suits.Spades:
                collection = collections.Find("spadesCollection").GetComponent<Card>();
                if (collection.Value != Value - 1 && Value != 1) return false;
                collection.SetCardValue(Suit, Value);
                DeactivateCard();
                transform.parent.Find($"card{indexOfSibling}").GetComponent<Card>().RevealCard();
                return true;

            case Enums.Suits.Diamonds:
                collection = collections.Find("diamondsCollection").GetComponent<Card>();
                if (collection.Value != Value - 1 && Value != 1) return false;
                collection.SetCardValue(Suit, Value);
                DeactivateCard();
                transform.parent.Find($"card{indexOfSibling}").GetComponent<Card>().RevealCard();
                return true;

            case Enums.Suits.Clubs:
                collection = collections.Find("clubsCollection").GetComponent<Card>();
                if (collection.Value != Value - 1 && Value != 1) return false;
                collection.SetCardValue(Suit, Value);
                DeactivateCard();
                transform.parent.Find($"card{indexOfSibling}").GetComponent<Card>().RevealCard();
                return true;

            default:
                return false;
        }
    }

    private bool IsValidPosition(Enums.Suits suit, int value)
    {
        bool blackIsValid = Suit is Enums.Suits.Diamonds or Enums.Suits.Hearts;
        bool targetIsBlack = suit is Enums.Suits.Clubs or Enums.Suits.Clubs;
        print($"Held card: {Suit} {Value} - Target card: {suit} {value}");
        print($"blackIsValid: {blackIsValid} - targetIsBlack: {targetIsBlack}");
        print($"value: {Value} - target value: {value}");
        print($"return {targetIsBlack == blackIsValid && value == Value + 1}");

        return targetIsBlack == blackIsValid && value == Value + 1;
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
