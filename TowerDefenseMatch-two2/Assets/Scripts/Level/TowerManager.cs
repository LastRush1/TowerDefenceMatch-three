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

    List<int> clearPlace = new List<int>();

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
        for (int i = 0; i < gridSum; i++)
        {
            clearPlace.Add(gridPlace[i].NumberGrid);
        }

        /*
         //                 *******************Заполняет все поле бащнями (для тестов)*********************
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
        bool set = false;
        if (clearPlace.Count > 0)
        {
            int random = (int)Randomazer(0, clearPlace.Count - 1);
            TowerInstantiate(random);
            /*
            towers.Add(Instantiate(prefabs[(int)Randomazer(0, prefabs.Count - 1)], gameObject.transform.position, Quaternion.identity));
            towers[towers.Count - 1].transform.position = gridPlace[clearPlace[random]].transform.position;
            towersPlaceNum.Add(gridPlace[clearPlace[random]].NumberGrid);
            clearPlace.RemoveAt(random);*/
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
        Tower tower1 = null, tower2 = null;
        int gridPlaceNum1 = 0;
        int gridPlaceNum2 = 0;
        for (int i = 0; i < towers.Count; i++)
        {
            if (towersPlaceNum[i] == firstTowerNum)
            {
                tower1 = towers[i];
                gridPlaceNum1 = towersPlaceNum[i];
            }
            if (towersPlaceNum[i] == secondTowerNum)
            {
                tower2 = towers[i];
                gridPlaceNum2 = towersPlaceNum[i];
            }
            if ((tower1 != null)&(tower2 != null))
            {
                break;
            }
        }
        if (tower1.GetTowerId == tower2.GetTowerId)
        {
            if (tower1 != tower2)
            {
                if (tower1.GetTowerLevel == tower2.GetTowerLevel)
                {
                    clearPlace.Add(gridPlaceNum1);
                    Union(tower1, tower2, gridPlaceNum2);
                }
            }
        }
        else
        {
            Debug.Log("Слияние невозможно!");
        }
    }

    void TowerInstantiate(int num)
    {
        towers.Add(Instantiate(prefabs[(int)Randomazer(0, prefabs.Count - 1)], gameObject.transform.position, Quaternion.identity));
        towers[towers.Count - 1].transform.position = gridPlace[clearPlace[num]].transform.position;
        towersPlaceNum.Add(gridPlace[clearPlace[num]].NumberGrid);
        clearPlace.RemoveAt(num);
    }

    void TowerDestroy(Tower tower)
    {
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i] == tower)
            {
                towers.RemoveAt(i);
                towersPlaceNum.RemoveAt(i);
                Destroy(tower.gameObject);
                break;
            }
        }
    }

    private void Union(Tower tower1, Tower tower2, int gridPlaceNum2) // При слиянии нужно удалять из активных башен!!! а так же добавлять в активные башни новые
    {
        int level;
        Transform transformTower;
        TowerDestroy(tower1);
        level = tower2.GetTowerLevel;
        transformTower = tower2.transform;
        TowerDestroy(tower2);


        int random = 0;
        random = UnityEngine.Random.Range(0, prefabs.Count-1);
        //TowerInstantiate(gridPlaceNum2);
        towers[gridPlaceNum2] = Instantiate(prefabs[random], gameObject.transform.position, Quaternion.identity);
        towers[towers.Count - 1].transform.position = new Vector2(transformTower.position.x, transformTower.position.y);
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
            if (towersPlaceNum[i] == gridNum)
            {
                return towers[i];
            }  
        }
        return null;
    }
    
}
