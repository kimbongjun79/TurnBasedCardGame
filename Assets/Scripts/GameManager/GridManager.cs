using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 4;
    public int height = 4;
    [SerializeField] Vector2 spacing = new Vector2 (2f, 1.5f); //그리드셀간의 간격

    public GameObject girdCellPrefab;
    public List<GameObject> gridObjects = new List<GameObject>();
    public GameObject[,] gridCells;


    void Start()
    {
        CreateGrid();
    }

    void CreateGrid() 
    {
        gridCells = new GameObject[width, height];
        Vector2 gridSize = new Vector2(width, height);
        Vector2 centerOffset = Vector2.Scale(gridSize - Vector2.one, spacing) / 2f;

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 gridPosition = new Vector2(x, y);
                Vector2 SpawnPosition = Vector2.Scale(gridPosition, spacing)- centerOffset;

                GameObject gridCell = Instantiate(girdCellPrefab, SpawnPosition, Quaternion.identity);
                gridCell.transform.SetParent(transform);
                gridCell.GetComponent<GridCell>().girdIndex = gridPosition;
                gridCells[x, y] = gridCell;
            }
        }
    
    }

    //그리드에 배치할 수 있는 객체면 true 반환, 아니면 false반환
    public bool AddObjectToGrid(GameObject obj, Vector2 gridPosition, Card cardData)
    {
        if (gridPosition.x >= 0 && gridPosition.x < width && gridPosition.y >= 0 && gridPosition.y < height)
        {
            GridCell cell = gridCells[(int)gridPosition.x, (int)gridPosition.y].GetComponent<GridCell>();
            if (cell.CellFull) return false;
            else
            {
                GameObject newObj = Instantiate(obj, cell.GetComponent<Transform>().position, Quaternion.identity);
                newObj.transform.SetParent(transform);
                gridObjects.Add(newObj);
                cell.objectInCell = newObj;
                cell.CellFull = true;

                CharacterStat stat = newObj.GetComponent<CharacterStat>();
                if(stat != null)
                {
                    stat.Initialize(cardData);
                }
                
                return true;
            }
               
        }
        else return false;
    }
}
