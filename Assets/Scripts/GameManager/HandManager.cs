using System.Collections;
using System.Collections.Generic;
//using NUnit.Framework;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform handTransform;
    public float fanSpread = 7.5f;
    public float cardSpacing = 100f;
    public float verticalSpacing = 100f;
    public List<GameObject> cardsInHand = new List<GameObject>();
    public int maxHandSize = 12;

    void Start()
    {
       
    }

    public void AddCardToHand(Card cardData)
    {
        //Instantiate card
        if(cardsInHand.Count < maxHandSize)
        {
            GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
            cardsInHand.Add(newCard);

            //카드 데이터 instantiated
            newCard.GetComponent<CardDisplay>().cardData = cardData;
            newCard.GetComponent<CardDisplay>().UpdateCardDisplay();
        }
        UpdateHandVisuals();
    }

    void Update()
    {
        //UpdateHandVisuals();
    }

    public void BattleSetup(int setMaxHandSize)
    {
        maxHandSize = setMaxHandSize;
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;
        if(cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }
        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);


            float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));

            float nomalizedPosition = (2f * i / (cardCount - 1) - 1f);
            float verticalOffset = verticalSpacing * (1 - nomalizedPosition * nomalizedPosition);
            
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }
}
