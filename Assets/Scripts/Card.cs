using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Utils;
using UnityEngine;

public class Card : MonoBehaviour
{
    public List<Sprite> CardSprites;
    public Enums.Suits Suit;
    public bool IsPlayCard = false;
    public int Value; // 1 - 13
    public bool TopShownCard = false;

    [NonSerialized] public bool InPlay = false;

    private Collider2D _collider;
    [SerializeField] private Vector3 _originalPosition;
    private BoardScript _mainBoardScript;

    [SerializeField] private bool _active = false;
    [SerializeField] private int _column = -1;
    [SerializeField] private bool _isDragging = false;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _originalPosition = transform.position;
        _mainBoardScript = transform.root.GetComponent<BoardScript>();
        if (IsPlayCard is false) return;
        if (TopShownCard is false)
        {
            _column = int.Parse(transform.parent.name.Last().ToString());
        }
    }

    void OnMouseDragStart()
    {
        //print(transform.name);
    }

    void OnMouseDrag()
    {
        if (InPlay is false)
        {
            return;
        }

        if (_isDragging is false)
        {
            OnMouseDragStart();
        }

        _isDragging = true;
        List<Transform> cardsToMove = _mainBoardScript.GetCardsInPlayInColumn(_column).Where(CardIsChild).ToList();

        MoveCardWithMouse(transform, 0);
        for (int i = 0; i < cardsToMove.Count; i++)
        {
            Transform card = cardsToMove.ElementAt(i);
            MoveCardWithMouse(card, i + 1);
        }
    }

    void OnMouseDragEnd()
    {
        // Current cards in column
        List<Transform> cards = new();
        List<Transform> cardsToMove = _mainBoardScript.GetCardsInPlayInColumn(_column).Where(CardIsChild).ToList();
        bool lastCardActive = true;
        int indexOfLastCard = 0;
        GameObject closestColumn = GetClosestColumn();

        for (int i = 0; i < 13; i++)
        {
            if (closestColumn is null)
            {
                return;
            }

            if (closestColumn.name.Contains("Collections") && cardsToMove.Count < 1)
            {
                PlaceCardInCollection();
                ResetPosition();
                return;
            }

            Transform card = closestColumn.transform.Find($"card{i}");
            if (lastCardActive)
            {
                cards.Add(card);
                indexOfLastCard = i;
            }

            if (card is null || card.GetComponent<Card>()._active is false)
            {
                lastCardActive = false;
            }
        }

        ResetPosition();
        foreach (Transform card in cardsToMove)
        {
            card.GetComponent<Card>().ResetPosition();
        }

        Card targetCard = cards.Count > 0 ? cards.ElementAt(cards.Count - 2).GetComponent<Card>() : closestColumn.transform.Find("card0").GetComponent<Card>();

        if (IsValidPosition(targetCard.Suit, targetCard.Value, cards.Count) is false)
        {
            return;
        }

        cards.Last().GetComponent<Card>().SetCardValue(Suit, Value);
        for (int i = 0; i < cardsToMove.Count; i++)
        {
            var card = cardsToMove.ElementAt(i).GetComponent<Card>();
            closestColumn.transform.GetChild(indexOfLastCard + i + 1).GetComponent<Card>()
                .SetCardValue(card.Suit, card.Value);
            card.DeactivateCard();
        }

        DeactivateCard();

        if (TopShownCard)
        {
            _mainBoardScript.PlayedTopShownCard();
        }

        int indexOfSibling = TopShownCard ? -1 : int.Parse(Regex.Replace(transform.name, @"[\D]", string.Empty)) - 1;
        if (transform.parent.Find($"card{indexOfSibling}") is null)
        {
            return;
        }

        transform.parent.Find($"card{indexOfSibling}").GetComponent<Card>().RevealCard();
    }

    void OnMouseUp()
    {
        if (_isDragging is false) return;
        _isDragging = false;
        OnMouseDragEnd();
    }

    /// <summary>
    /// Moves the card along with the mouse
    /// </summary>
    /// <param name="card">The transform of the card</param>
    /// <param name="index">The index of the card, with 0 being the card at the top of the selected stack</param>
    void MoveCardWithMouse(Transform card, int index)
    {
        Vector3 newPosition = card.position;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.x = mousePosition.x;
        newPosition.y = mousePosition.y + -0.45f * index;
        newPosition.z = -2 - 0.1f * index;
        card.position = newPosition;
    }

    GameObject GetClosestColumn()
    {
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        var results = new List<Collider2D>();
        Physics2D.OverlapCollider(_collider, filter, results);
        results.RemoveAll(c => c.transform.name.Contains("card"));

        if (results.Count < 1)
        {
            return null;
        }

        List<float> distances = results.Select(result => Physics2D.Distance(_collider, result).distance).ToList();
        return results.ElementAt(distances.IndexOfMin()).gameObject;
    }

    private bool CardIsChild(Transform card)
    {
        int cardIndex = int.Parse(Regex.Replace(card.name, @"[\D]", string.Empty));
        int thisCardIndex = int.Parse(Regex.Replace(transform.name, @"[\D]", string.Empty));

        return cardIndex > thisCardIndex;
    }

    private void PlaceCardInCollection()
    {
        Transform collections = TopShownCard ? transform.parent.Find("Collections") : transform.parent.parent.Find("Collections");

        int indexOfSibling = TopShownCard ? -1 : int.Parse(Regex.Replace(transform.name, @"[\D]", string.Empty)) - 1;
        Card collection;

        switch (Suit)
        {
            case Enums.Suits.Hearts:
                collection = collections.Find("heartsCollection").GetComponent<Card>();
                break;

            case Enums.Suits.Spades:
                collection = collections.Find("spadesCollection").GetComponent<Card>();
                break;

            case Enums.Suits.Diamonds:
                collection = collections.Find("diamondsCollection").GetComponent<Card>();
                break;

            case Enums.Suits.Clubs:
                collection = collections.Find("clubsCollection").GetComponent<Card>();
                break;

            default:
                return;
        }

        if (collection.Value != Value - 1 && Value != 1)
        {
            return;
        }

        collection.SetCardValue(Suit, Value);
        DeactivateCard();
        if (TopShownCard)
        {
            _mainBoardScript.PlayedTopShownCard();
        }
        transform.parent.Find($"card{indexOfSibling}")?.GetComponent<Card>().RevealCard();
    }

    private bool IsValidPosition(Enums.Suits suit, int value, int cardCount)
    {
        if (cardCount is 0)
        {
            return true;
        }

        bool blackIsValid = Suit is Enums.Suits.Diamonds or Enums.Suits.Hearts;
        bool targetIsBlack = suit is Enums.Suits.Clubs or Enums.Suits.Spades;

        return targetIsBlack == blackIsValid && value == Value + 1;
    }


    private void SetActive(bool active)
    {
        transform.gameObject.SetActive(active);
        _active = active;
        InPlay = active;
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
        InPlay = false;
    }

    /// <summary>
    /// Flips the card to reveal suit and value
    /// </summary>
    public void RevealCard()
    {
        SetCardValue(Suit, Value);
        InPlay = true;
    }

    /// <summary>
    /// Moves a card to its original position
    /// </summary>
    public void ResetPosition()
    {
        transform.position = _originalPosition;
    }
}
