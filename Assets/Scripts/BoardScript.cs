using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardScript : MonoBehaviour
{
    private List<(Enums.Suits, int)> _deck;
    private List<(Enums.Suits, int)> _shownDeck;
    private List<List<(Enums.Suits, int)>> _board = new();

    private bool _deckEmpty;
    private bool _shownDeckEmpty = true;

    private Card _topShownCard;
    private Card _bottomShownCard;

    void Start()
    {
        _topShownCard = transform.Find("topShownCard").GetComponent<Card>();
        _bottomShownCard = transform.Find("bottomShownCard").GetComponent<Card>();

        _deck = CreateShuffledDeck();
        _shownDeck = new();
        SetUpBoard();
    }

    public void RestartGame()
    {
        _board = new();
        _shownDeck = new();
        _deck = CreateShuffledDeck();
        SetUpBoard();
    }

    public void SetUpBoard()
    {
        const int width = 7;
        int count = 1;

        for (int i = 0; i < width; i++)
        {
            List<(Enums.Suits, int)> col = new();
            for (int j = 0; j < count; j++)
            {
                col.Add(_deck.First());
                _deck.Remove(_deck.First());
            }

            _board.Add(col);
            count++;
        }

        DeactivateAllCards();

        for (int i = 0; i < width; i++)
        {
            ActivateColumn(i, _board.ElementAt(i));
        }
    }

    public void NextCard()
    {
        if (_deckEmpty)
        {
            if (_shownDeckEmpty is false)
            {
                ResetDeck();
            }

            return;
        }

        (Enums.Suits, int) nextCard = _deck.ElementAt(_deck.Count - 1);
        _deck.RemoveAt(_deck.Count - 1);

        _deckEmpty = _deck.Count < 1;

        _shownDeck.Add(nextCard);
        (Enums.Suits, int) previousShownCard = (_topShownCard.Suit, _topShownCard.Value);
        _topShownCard.SetCardValue(nextCard.Item1, nextCard.Item2);
        _topShownCard.IsPlayCard = true;
        _topShownCard.InPlay = true;

        if (_shownDeck.Count > 1)
        {
            _bottomShownCard.SetCardValue(previousShownCard);
            _bottomShownCard.ActivateCard();
        }

        _shownDeckEmpty = _shownDeck.Count < 1;
    }

    public void ResetDeck()
    {
        _deck.Clear();
        _deck.AddRange(_shownDeck);
        _shownDeck.Clear();
        _topShownCard.DeactivateCard();
        _bottomShownCard.DeactivateCard();
    }

    public List<Transform> GetCardsInPlayInColumn(int col)
    {
        if (col < 0)
        {
            return new List<Transform>();
        }
        string colName = $"Column{col}";
        Transform column = transform.Find(colName);
        List<Transform> cards = new();

        for (int i = 0; i < 13; i++)
        {
            Transform card = column.GetChild(i);
            if (card.GetComponent<Card>().InPlay)
            {
                cards.Add(card);
            }
        }

        return cards;
    }

    public void PlayedTopShownCard()
    {
        _shownDeck.Remove((_topShownCard.Suit, _topShownCard.Value));
        _topShownCard.SetCardValue(_bottomShownCard.Suit, _bottomShownCard.Value);
        _topShownCard.ActivateCard();

        if (_shownDeck.Count > 1)
        {
            _bottomShownCard.SetCardValue(_shownDeck.ElementAt(_shownDeck.Count - 2));
        }
        else
        {
            _bottomShownCard.DeactivateCard();
        }
    }

    private static List<(Enums.Suits, int)> CreateShuffledDeck()
    {
        List<(Enums.Suits, int)> orderedDeck = new();
        List<(Enums.Suits, int)> shuffledDeck = new();

        for (int suit = 0; suit < 4; suit++)
        {
            for (int value = 1; value < 14; value++)
            {
                orderedDeck.Add(((Enums.Suits)suit, value));
            }
        }

        for (int i = 1; i < 53; i++)
        {
            int index = Random.Range(0, orderedDeck.Count - 1);
            shuffledDeck.Add(orderedDeck.ElementAt(index));
            orderedDeck.RemoveAt(index);
        }

        return shuffledDeck;
    }

    private void ActivateColumn(int columnIndex, IReadOnlyCollection<(Enums.Suits, int)> cards)
    {
        for (int cardIndex = 0; cardIndex < cards.Count; cardIndex++)
        {
            var card = transform.Find($"Column{columnIndex}").Find($"card{cardIndex}").GetComponent<Card>();
            card.SetCardValue(cards.ElementAt(cardIndex));
            if (cards.Count > cardIndex + 1)
            {
                card.HideCard();
            }
        }
    }

    private void DeactivateAllCards()
    {
        for (int col = 0; col < 7; col++)
        {
            for (int card = 0; card < 13; card++)
            {
                transform.Find($"Column{col}").Find($"card{card}").GetComponent<Card>().DeactivateCard();
            }
        }

        foreach (Card card in transform.Find("Collections").GetComponentsInChildren<Card>())
        {
            card.DeactivateCard();
        }

        _topShownCard.DeactivateCard();
        _bottomShownCard.DeactivateCard();
    }
}
