using UnityEngine;

//해상도 변경에 따른 ui 위치조정
public class UIposition : MonoBehaviour
{
    public RectTransform objectToPosition;

    public int widthDivider = 2;
    public int heightDivider = 2;
    public float widthMultiplier = 1f;
    public float heightMultiplier = 1f;

    public bool updatePosition = false;

    void Start()
    {
        SetUIObjectPosition();
    }

    void Update()
    {
        if(updatePosition)
        {
            SetUIObjectPosition();
        }
    }

    public void SetUIObjectPosition()
    {
        if(objectToPosition != null && widthDivider != 0 && heightDivider != 0)
        {
            float anchorX = widthMultiplier / widthDivider;
            float anchorY = heightMultiplier / heightDivider;

            objectToPosition.anchorMin = new Vector2(anchorX, anchorY);
            objectToPosition.anchorMax = new Vector2(anchorX, anchorY);
            objectToPosition.pivot = new Vector2(0.5f, 0.5f);

            objectToPosition.anchoredPosition = Vector2.zero;
        }
    }
}
