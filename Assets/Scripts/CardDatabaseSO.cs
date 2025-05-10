using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "CardGame/CardDatabase", fileName = "CardDatabase")]
public class CardDatabaseSO : ScriptableObject
{
    public CardDataSO[] allCards;

    private Dictionary<string, CardDataSO> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<string, CardDataSO>();
        foreach (var cd in allCards)
            lookup[cd.cardName] = cd;
    }

    public CardDataSO GetByName(string name)
        => lookup != null && lookup.TryGetValue(name, out var cd) ? cd : null;
}
