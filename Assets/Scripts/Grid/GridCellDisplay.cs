using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),typeof(GridCell))]
public class GridCellDisplay : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color highlightColor = Color.white;
    public Color posColor = Color.green;
    public Color negColor = Color.red;
    private Color originalColor;
    public GameObject[] backgrounds;
    private bool setBackground = false;
    public GridCell gridCell;


    void Update()
    {
        if (!setBackground) SetBackground();
    }
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridCell = GetComponent<GridCell>();
        originalColor = spriteRenderer.color;
    }
    //When the mouse enters the collider area
    void OnMouseEnter()
    {
        spriteRenderer.color = highlightColor;
        Debug.Log(gridCell.CellFull);
        if (!GameManager.Instance.PlayingCard)
        {
            spriteRenderer.color = highlightColor;
        }
        else if (gridCell.CellFull || gridCell.girdIndex.y > 1)
        {
            spriteRenderer.color = negColor;
        }
        else spriteRenderer.color = posColor;
    }
    //When the mouse exits the collider area
    void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }

    private void SetBackground()
    {
        if(gridCell.girdIndex.x % 2 != 0)
        {
            backgrounds[0].SetActive(true);
        }
        if(gridCell.girdIndex.y %2 != 0)
        {
            backgrounds[1].SetActive(true);
        }
        setBackground = true;
    }
}