using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public TMP_Text nameText;
    public Image[] typeImage;
    public Image displayImage;
    public TMP_Text descriptionText;
    public TMP_Text healthText;
    public TMP_Text damageText;

    private Color[] cardColor =
    {
        new Color(0.44f,0f,0f),
        new Color(0.42f,0.25f,0.08f),
        new Color(0.1f,0.2f,0.35f),
        new Color(0.54f,0.55f,0.39f),
        new Color(0.38f,0.51f,0.55f)
    };

    private Color[] typeColors =
    {
        Color.red,
        new Color(0.8f, 0.52f, 0.24f),
        Color.green,
        Color.blue,
        Color.magenta,
        Color.white
    };

    void Update()
    {
       // UpdateCardDisplay();
    }
    public void UpdateCardDisplay()
    {
        cardImage.color = typeColors[(int)cardData.cardType[0]];
        nameText.text = cardData.cardName;
        healthText.text = cardData.durability.ToString();
        damageText.text = cardData.damage.ToString();
        displayImage.sprite = cardData.cardSprite;

        
        for(int i = 0; i< typeImage.Length; i++)
        {
            if(i<cardData.cardType.Count)
            {
                typeImage[i].gameObject.SetActive(true);
                typeImage[i].color = typeColors[(int)cardData.cardType[i]];
            }
            else
            {
                typeImage[i].gameObject.SetActive(false);
            }
        }

        
    }

}
