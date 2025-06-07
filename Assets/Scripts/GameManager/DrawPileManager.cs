using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrawPileManager : MonoBehaviour
{
    public List<Card> drawPile = new List<Card>();
    
    public int startingHandSize = 6;
    private int currentIndex = 0;
    public int maxHandSize;
    public int currentHandSize;
    private HandManager handManager;

    public TextMeshProUGUI drawPileCounter;

    void Start()
    {
        handManager = FindFirstObjectByType<HandManager>();
    }

    void Update()
    {
        if (handManager != null)
        {
            currentHandSize = handManager.cardsInHand.Count;
        }
    }
    
    public void MakeDrawPile(List<Card> cardsToAdd)
    {
        
        drawPile.AddRange(cardsToAdd);
        Utility.Shuffle(drawPile);
        UpdateDrawPileCount();
    }

    public void BattleSetup(int numberofCardsToDraw, int setMaxHandSize)
    {
        maxHandSize = setMaxHandSize;
        for(int i = 0; i < numberofCardsToDraw; i++)
        {
            DrawCard(handManager);
        }
    }

    public void DrawCard(HandManager handManager)
    {
        if (drawPile.Count == 0)
        {

            return;
        }
        if (currentHandSize < maxHandSize)
        {
            Card nextCard = drawPile[currentIndex];
            handManager.AddCardToHand(nextCard);
            drawPile.RemoveAt(currentIndex);
            UpdateDrawPileCount();
            if (drawPile.Count > 0) currentIndex %= drawPile.Count;

        }
    }


    private void UpdateDrawPileCount()
    {
        drawPileCounter.text = drawPile.Count.ToString();
    }

    
}
