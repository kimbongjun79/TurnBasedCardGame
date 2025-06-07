using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    public int startingHandSize = 6;
    public int maxHandSize = 12;
    public int currentHandSize;
    private HandManager handManager;
    private DrawPileManager drawPileManager;
    private bool startBattleRun = true;

    void Start()
    {
        Card[] cards = Resources.LoadAll<Card>("Cards");
        allCards.AddRange(cards);

    }

    void Awake()
    {
        if(drawPileManager == null)
        {
            drawPileManager = FindFirstObjectByType<DrawPileManager>();
        }
        if(handManager == null)
        {
            handManager = FindFirstObjectByType<HandManager>();
        }
    }

    void Update()
    {
        if(startBattleRun)
        {
            BattleSetup();
        }

        if(drawPileManager.drawPile.Count == 0)
        {
            drawPileManager.MakeDrawPile(allCards);
        }
    }

    public void BattleSetup()
    {
        handManager.BattleSetup(maxHandSize);
        drawPileManager.MakeDrawPile(allCards);
        drawPileManager.BattleSetup(startingHandSize, maxHandSize);
        startBattleRun = false;
    }

}
