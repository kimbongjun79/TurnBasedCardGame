using UnityEngine;

[CreateAssetMenu(menuName = "CardGame/CardData", fileName = "NewCardData")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public Sprite illustration;
    public int attack, durability, cost;
    [TextArea] public string effectDescription;
    [TextArea] public string loreDescription;
}
