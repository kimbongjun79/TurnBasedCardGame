using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card" , menuName = "Card")]
public class Card : ScriptableObject
{
    //CardElements 
    public string cardName;
    public CardClass cardClass;
    public List<CardType> cardType;
    public Sprite cardSprite;
    public string description;
    public int durability;
    public int damage;
    public GameObject prefab;
    public int range;
    public AttackPattern attackpattern;
    public PriorityTarget prioirtyTarget;
    public int CardScore;
/*  
 *  public CharacterType1 characterType1;
    public CharacterType2 characterType2;
    public CharacterType3 characterType3;
    public CharacterType4 characterType4;
*/
    

    public enum CardClass
    {
        Bronze,
        Silver,
        Gold
    }


    public enum CardType
    {
        Unit,
        Tower,
        Action
    }
    public enum CharacterType1
    {
        SteamPunk,
        CyberPunk
    }
    public enum CharacterType2
    {
        Inanimate,
        Virtual
    }

    public enum CharacterType3
    {
        Machine,
        Concept
    }

    public enum CharacterType4
    {
        Infantry,
        Pronunciation,
        Weapon,
        Politics,
        Tactic
    }

    public enum AttackPattern
    {
        Single,
        Multitarget,
        Cross,
        Column,
        Row,
        TwoByTwo,
        FourByFour
    }

    public enum PriorityTarget
    {
        Close,
        Far,
        LeastCurrentHealth,
        MostCurrentHealth,
        MostMaxHealth,
        MostDamage
    }

    public enum SpellType
    { 
        Buff,
        Debuff
    }

    public enum AttributeTarget
    {
        health,
        damage,
        range,
        attackPattern,
        damageType,
        cardType,
        priorityTarget
    }


}