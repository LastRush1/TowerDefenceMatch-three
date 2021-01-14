using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TowerManager : MonoBehaviour
{

    List<Tower> towers = new List<Tower>();

    List<int> towersPlaceNum = new List<int>();

    [SerializeField]
    List<Tower> prefabs = new List<Tower>();

    int prefabNum = 0;

    

    List<GridPlace> gridPlace;

    private void Awake()
    {
        
    }

    void Start()
    {

    }

    void Update()
    {
        
    }


    public void SetTowers(List<GridPlace> gridPlace)
    {
        this.gridPlace = gridPlace;
        int gridSum = gridPlace.Count;

        /*
         //                 *******************Заполняет все поле башнями (для тестов)*********************
        while (gridSum > 0)
        {
            gridSum--;
            towers.Add(Instantiate(prefabs[prefabNum],gameObject.transform.position,Quaternion.identity));
            //Исправить при реворке демки!!!
            towers[towers.Count - 1].transform.position = gridPlace[towers.Count - 1].transform.position;
            towersPlaceNum.Add(gridPlace[towers.Count - 1].NumberGrid);
            banPlace[gridPlace[towers.Count - 1].NumberGrid] = true;
            prefabNum++;
            if (prefabNum == prefabs.Count)
            {
                prefabNum = 0;
            }
        } */
    }

    float Randomazer(float min, float max)
    {
        return Random.Range(min, max);
    }

    public void addTower()
    {
        List<int> clearPlace = new List<int>();
        bool set = false;
        for (int i = 0; i < gridPlace.Count; i++)
        {
            if (gridPlace[i].building == null)
            {
                clearPlace.Add(gridPlace[i].NumberGrid);
            }
        }
        if (clearPlace.Count > 0)
        {
            int random = (int)Randomazer(0, clearPlace.Count - 1);
            TowerInstantiate(clearPlace[random]);
        }
        else
        {
            Debug.Log("Нет места!");
        }
    }

    /// <summary>
    /// Слияние двух башен
    /// </summary>
    public void PotentialUnion(int firstTowerNum, int secondTowerNum)
    {
        if (gridPlace[secondTowerNum].building!=null)
        {
            Tower tower1 = gridPlace[firstTowerNum].building.GetComponent<Tower>(), tower2 = gridPlace[secondTowerNum].building.GetComponent<Tower>();
            if (tower1.GetTowerId == tower2.GetTowerId)
            {
                if (tower1 != tower2)
                {
                    if (tower1.GetTowerLevel == tower2.GetTowerLevel)
                    {
                        Union(tower1, tower2, firstTowerNum, secondTowerNum);
                    }
                }
            }
            else
            {
                Debug.Log("Слияние невозможно!");
            }
        }
        
    }

    void TowerInstantiate(int num)
    {
        if (gridPlace[num].building == null)
        {
            //Debug.Log("ПРивет");
            gridPlace[num].building = Instantiate(prefabs[(int)Randomazer(0, prefabs.Count - 1)], gameObject.transform.position, Quaternion.identity);
            towers.Add(gridPlace[num].building);
            gridPlace[num].building.transform.position = gridPlace[num].transform.position;
        }
    }

    void TowerDestroy(GridPlace tower)
    {
        Destroy(tower.building.gameObject);
        tower.building = null;
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i] == null)
            {
                towers.RemoveAt(i);
                break;
            }
        }
    }

    private void Union(Tower tower1, Tower tower2, int firstTowerNum, int secondTowerNum) // При слиянии нужно удалять из активных башен!!! а так же добавлять в активные башни новые
    {
        int level;
        Transform transformTower;

        level = tower2.GetTowerLevel;
        transformTower = tower2.transform;
        TowerDestroy(gridPlace[firstTowerNum]);
        TowerDestroy(gridPlace[secondTowerNum]);
        TowerInstantiate(gridPlace[secondTowerNum].NumberGrid);

        ///обдумать шаг


        for (int i = 0; i < level; i++)
        {
            towers[towers.Count-1].LevelUp();
        }
        Debug.Log("Типо слияние");
    }

    public Tower TryTakeTower(int gridNum)
    {
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i] == gridPlace[gridNum].building)
            {
                return towers[i];
            }  
        }
        return null;
    }
    
}
