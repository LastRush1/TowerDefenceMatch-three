using System.Collections.Generic;
using UnityEngine;

public class BoardCreater : MonoBehaviour
{
    [SerializeField]
    GameObject board = default;

    [SerializeField]
    Vector2Int sizeBoard = default;

    [SerializeField]
    GameObject gridTileMap = default;

    [SerializeField]
    GameObject canvasTileMap = default;

    [SerializeField]
    GridFactory gridFactory = default;

    [SerializeField]
    RoadFactory roadFactory = default;
    public Vector2Int SizeBoard
    {
        get { return sizeBoard; }
    }

    [SerializeField]
    GridPlace gridPlacePrefab = default;

    List<GridPlace> gridPlaceList = new List<GridPlace>();

    public List<GridPlace> GridPlaceList
    {
        get { return gridPlaceList; }
    }


    public GameObject Board
    {
        get { return board; }
    }


    List<Road> roads = new List<Road>();

    public Transform FirstRoad
    { 
        get { return roads[0].transform; }
    }

    public List<Road>  Road
    { 
        get { return roads; }
    }





    /// <summary>
    /// Для TileMap
    /// </summary>
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

    /// <summary>
    /// Создаем в нужном месте grid
    /// </summary>
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

    GameObject loadTileMap = default;
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
        LoadRoad();
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
        //gridPlaceList.Add(Instantiate(gridPlacePrefab, new Vector3(x, y, 0), Quaternion.identity));
        GridPlace grid = gridFactory.Get();
        grid.transform.position = new Vector3(x, y, 0);
        gridPlaceList.Add(grid);
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



    void LoadRoad()
    {
        int sum = 0;
        Road road;
        Debug.Log($"КОл-во гридов : {gridPlaceList.Count}");
        for (int i = 0; i < sizeBoard.x; i++)
        {
            road = roadFactory.Get();
            roads.Add(road);
            road.transform.position = new Vector2(gridPlaceList[sum].transform.position.x - 1, gridPlaceList[sum].transform.position.y);
            sum += sizeBoard.y;
            Debug.Log($"Сумма: {sum}");
        }
        road = roadFactory.Get();

        road.transform.position = new Vector2(roads[roads.Count-1].transform.position.x, roads[roads.Count - 1].transform.position.y + 1);
        roads.Add(road);
        sum = sizeBoard.x * (sizeBoard.y - 1);

        for (int j = 0; j < sizeBoard.y; j++)
        {
            road = roadFactory.Get();
            roads.Add(road);
            road.transform.position = new Vector2(gridPlaceList[sum].transform.position.x, gridPlaceList[sum].transform.position.y + 1);
            sum++;
            Debug.Log($"Сумма: {sum}");
        }
        road = roadFactory.Get();
        road.transform.position = new Vector2(roads[roads.Count - 1].transform.position.x + 1, roads[roads.Count - 1].transform.position.y);
        roads.Add(road);
        sum = sizeBoard.x * sizeBoard.y - 1;
        for (int i = 0; i < sizeBoard.x; i++)
        {
            road = roadFactory.Get();
            roads.Add(road);
            road.transform.position = new Vector2(gridPlaceList[sum].transform.position.x + 1, gridPlaceList[sum].transform.position.y);
            sum -= sizeBoard.y;
            Debug.Log($"Сумма: {sum}");
        }
    }


}