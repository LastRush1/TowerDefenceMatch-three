using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TowerManager : MonoBehaviour
{
    List<Tower> towers = new List<Tower>();

    [SerializeField]
    List<Tower> prefabs = new List<Tower>();   

    List<GridPlace> gridPlace;

    LoadTowers loadTowers;

    [SerializeField]
    TowerFactory towerFactory;

    TowerController towerController;

    public TowerController SetTowerController
    {
        set { towerController = value; }
    }

    private void Awake()
    {
        loadTowers = GetComponent<LoadTowers>();
        prefabs = loadTowers.Towers;
    }

    void Start()
    {
        List<Tower> tower = new List<Tower>();

        /*
        List<BasicInfoTower> towers = lvlTowerPull.Towers;
        for (int i = 0; i < towers.Count; i++)
        {
            prefabs = towers[i]
        }*/

    }


    /// <summary>
    /// Передает знание о GridPlace листе 
    /// </summary>
    /// <param name="gridPlace"></param>
    public void SetTowers(List<GridPlace> gridPlace)
    {
        this.gridPlace = gridPlace;
        int gridSum = gridPlace.Count;
    }

    /// <summary>
    /// Удобный рандомайзер
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    float Randomazer(float min, float max)
    {
        return Random.Range(min, max);
    }

    /// <summary>
    /// Проверка свободных слотов под башню и ее добавление
    /// </summary>
    public void addTower()
    {
        List<int> clearPlace = new List<int>();
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
    /// Проверка возможности слияния двух башен
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
                        Union(tower2.GetTowerLevel, firstTowerNum, secondTowerNum);
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
            Tower tower = towerFactory.Get(prefabs[(int)Randomazer(0, prefabs.Count - 1)]);
            tower.transform.position = gridPlace[num].gameObject.transform.position;
            gridPlace[num].building = tower;
            towers.Add(gridPlace[num].building);
            towerController.TowerInitialization(tower);
        } 
    }



    /// <summary>
    /// Удалить башню(переписать и сдеать более экономично, т.е. отключать и снова использовать)
    /// </summary>
    /// <param name="tower"></param>
    void TowerDestroy(GridPlace tower)
    {
        Destroy(tower.building.gameObject);
        tower.building = null;
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i] == null)
            {
                towers.RemoveAt(i);
                towerController.SetActiveTower(i);
                break;
            }
        }
    }

    /// <summary>
    /// Слияние башен
    /// </summary>
    /// <param name="tower1"></param>
    /// <param name="tower2"></param>
    /// <param name="firstTowerNum"></param>
    /// <param name="secondTowerNum"></param>
    private void Union(int levelTower2, int firstTowerNum, int secondTowerNum) // При слиянии нужно удалять из активных башен!!! а так же добавлять в активные башни новые
    {
        TowerDestroy(gridPlace[firstTowerNum]);
        TowerDestroy(gridPlace[secondTowerNum]);
        TowerInstantiate(gridPlace[secondTowerNum].NumberGrid);


        for (int i = 0; i < levelTower2; i++)
        {
            towers[towers.Count-1].LevelUp();
        }
        Debug.Log("Типо слияние");
    }

    /// <summary>
    /// Попытка выделить башню
    /// </summary>
    /// <param name="gridNum"></param>
    /// <returns></returns>
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
