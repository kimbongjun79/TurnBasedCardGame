using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyHandManager : MonoBehaviour
{
    public GameObject BackCardPrefab; // 카드 뒷면 프리팹
    public Transform EnemyHandTransform; // 적 핸드 위치 기준점
    public float fanSpread = 5f;
    public List<GameObject> cardsInEnemyHand = new List<GameObject>();



    void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            AddCardToHand();
        }
    }

    public void AddCardToHand()
    {
        GameObject newCard = Instantiate(BackCardPrefab, EnemyHandTransform.position, Quaternion.identity, EnemyHandTransform);
        cardsInEnemyHand.Add(newCard);

        UpdateEnemyHandVisual();
    }

    private void UpdateEnemyHandVisual()
    {
        int EnemyCardCount = cardsInEnemyHand.Count;
        for (int i = 0; i < EnemyCardCount; i++)
        {
            float rotationangle = (fanSpread * (i - (EnemyCardCount-1) / 2f));
            cardsInEnemyHand[i].transform.localRotation = Quaternion.Euler(0f,0f, rotationangle);
        }

    }
}
