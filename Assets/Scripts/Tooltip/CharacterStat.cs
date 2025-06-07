using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static Card;

public class CharacterStat : MonoBehaviour
{
    public Card cardData;
    public string cardName;
    public CardClass cardClass;
    public List<CardType> cardType;
    public CharacterType1 characterType1;
    public CharacterType2 characterType2;
    public CharacterType3 characterType3;
    public CharacterType4 characterType4;

    public int Durability;
    public int damage;
    public int CardScore;
    public int range;
    public AttackPattern attackPattern;
    public PriorityTarget priorityTarge;

    private bool statsSet = false;

    public void Initialize(Card data)
    {
        cardData = data;
        SetStartStats();
    }

    private void SetStartStats()
    {
        if (cardData == null) Debug.Log("null carddata");
        Debug.Log("cardName " + cardData.cardName);
        Debug.Log("cardName " + cardData.cardType);
        Debug.Log("cardName " + cardData.durability);
        cardName = cardData.cardName;
        cardType = cardData.cardType;
        Durability = cardData.durability;
        damage = cardData.damage;
        cardType = cardData.cardType;
        statsSet = true;
    }
}
