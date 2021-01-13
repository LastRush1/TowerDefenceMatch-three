using System.Collections.Generic;
using UnityEngine;

public class BoardCreater : MonoBehaviour
{
    [SerializeField]
    GameObject board;

    [SerializeField]
    Vector2Int sizeBoard;

    [SerializeField]
    GameObject gridTileMap;

    [SerializeField]
    GameObject canvasTileMap;

    public Vector2Int SizeBoard
    {
        get { return sizeBoard; }
    }

    [SerializeField]
    GridPlace gridPlacePrefab;

    List<GridPlace> gridPlaceList = new List<GridPlace>();

    public List<GridPlace> GridPlaceList
    {
        get { return gridPlaceList; }
    }


    public GameObject Board
    {
        get { return board; }
    }

    void TileMapSettings()
    {
        if (gridTileMap == null)
        {
            gridTileMap = Instantiate(loadTileMap, canvasTileMap.transform);
        }
        else
        {
            if (sizeBoard.x % 2 == 0)
            {
                gridTileMap.transform.position = new Vector3(gridTileMap.transform.position.x + 0.5f, gridTileMap.transform.position.y, gridTileMap.transform.position.z);
            }
            if (sizeBoard.y % 2 == 0)
            {
                gridTileMap.transform.position = new Vector3(gridTileMap.transform.position.x, gridTileMap.transform.position.y, gridTileMap.transform.position.z + 0.5f);
            }
        }
    }

    void SliceBoard()
    {
        float x, y;
        int num = 0;

        //TileMapSettings();
        for (int i = 0; i < sizeBoard.y; i++)
        {
            if (sizeBoard.y % 2 == 0)
            {
                y = (-sizeBoard.y + 1f) / 2 + i;
            }
            else
            {
                y = -sizeBoard.y / 2 + i;
            }
            for (int j = 0; j < sizeBoard.x; j++)
            {
                if (sizeBoard.x % 2 == 0)
                {
                    x = (-sizeBoard.x + 1f) / 2 + j;
                }
                else
                {
                    x = -sizeBoard.x / 2 + j;
                }

                createGrid(x, y, num);
                num++;
            }
        }
    }

    
    public void DestoyAll()
    {
        for (int i = 0; i < gridPlaceList.Count; i++)
        {
            gridPlaceList[i].DestroyBuilding();
            gridPlaceList[i].DestroyRoad();
        }
    } 

    GameObject loadTileMap;
    public void LoadMap(bool LoadBoard)
    {
        if (LoadBoard)
        {
            board.transform.localScale = new Vector3(sizeBoard.x, sizeBoard.y, 1);
            Debug.Log($"Размер карты { sizeBoard.x} на { sizeBoard.y}");
            SliceBoard();


        }
        else
        {
            board.transform.localScale = new Vector3(sizeBoard.x, 1, sizeBoard.y);
            SliceBoard();
        }

    }
    /*
         public void LoadMap(bool LoadBoard, Vector2Int size, GameObject tileMap)
    {
        if (LoadBoard)
        {
            board.transform.localScale = new Vector3(size.x, 1, size.y);
            sizeBoard.x = size.x;
            sizeBoard.y = size.y;
            loadTileMap = tileMap;
            Debug.Log($"Размер карты { sizeBoard.x} на { sizeBoard.y}");
            SliceBoard();


        }
        else
        {
            board.transform.localScale = new Vector3(sizeBoard.x, 1, sizeBoard.y);
            SliceBoard();
        }

    }
     */


    void createGrid(float x, float y, int num)
    {
        gridPlaceList.Add(Instantiate(gridPlacePrefab, new Vector3(x, y, 0), Quaternion.identity));
        gridPlaceList[num].NumberGrid = num;
    } 

    public GridPlace TryGetGrid(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, 1))
        {
            int x = (int)(hit.point.x + sizeBoard.x * 0.5f);
            int y = (int)(hit.point.y + sizeBoard.y * 0.5f);
            if (x >= 0 && x < sizeBoard.x && y >= 0 && y < sizeBoard.y)
            {
                return gridPlaceList[x + y * sizeBoard.x];
            }
        }
        return null;
    }
}