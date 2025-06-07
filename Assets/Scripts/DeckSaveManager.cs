using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// 덱 데이터를 JSON으로 저장/로드하는 유틸리티
/// </summary>
public static class DeckSaveManager
{
    [Serializable]
    private class DeckData
    {
        public List<string> cardIDs;
    }

    private static string FilePath => Path.Combine(Application.persistentDataPath, "player_deck.json");

    /// <summary>
    /// 현재 덱을 파일에 저장합니다.
    /// </summary>
    public static void SaveDeck(List<CardDataSO> deck)
{
    var data = new DeckData { cardIDs = deck.Select(c => c.cardName).ToList() };
    string json = JsonUtility.ToJson(data, true);
    File.WriteAllText(FilePath, json);
    Debug.Log($"Deck saved to {FilePath}");
}

/// <summary>
/// 파일에서 덱을 로드하여 카드 ID 목록을 반환합니다.
/// </summary>
public static List<string> LoadDeckIDs()
{
    if (!File.Exists(FilePath))
        return new List<string>();

    try
    {
        string json = File.ReadAllText(FilePath);
        var data = JsonUtility.FromJson<DeckData>(json);
        return data.cardIDs ?? new List<string>();
    }
    catch (Exception e)
    {
        Debug.LogError($"Failed to load deck: {e}");
        return new List<string>();
    }
}
}
