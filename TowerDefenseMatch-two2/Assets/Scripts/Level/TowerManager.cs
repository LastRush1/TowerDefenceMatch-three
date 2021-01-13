using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField]
    List<Tower> towers = new List<Tower>();
    [SerializeField]
    List<int> towersPlaceNum = new List<int>();
    [SerializeField]
    List<Tower> prefabs = new List<Tower>();
    int prefabNum = 0;

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
        int gridSum = gridPlace.Count;
        while (gridSum > 0)
        {
            gridSum--;
            towers.Add(Instantiate(prefabs[prefabNum],gameObject.transform.position,Quaternion.identity));
            //Исправить при реворке демки!!!
            towers[towers.Count - 1].transform.position = gridPlace[towers.Count - 1].transform.position;
            towersPlaceNum.Add(gridPlace[towers.Count - 1].NumberGrid);
            prefabNum++;
            if (prefabNum == prefabs.Count)
            {
                prefabNum = 0;
            }
        }
    }

    /// <summary>
    /// Слияние двух башен
    /// </summary>
    public void PotentialUnion(int firstTowerNum, int secondTowerNum)
    {
        Tower tower1 = null, tower2 = null;
        for (int i = 0; i < towers.Count; i++)
        {
            if (towersPlaceNum[i] == firstTowerNum)
            {
                tower1 = towers[i];
            }
            if (towersPlaceNum[i] == secondTowerNum)
            {
                tower2 = towers[i];
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
                    Union(tower1, tower2);
                }
            }
        }
        else
        {
            Debug.Log("Слияние невозможно!");
        }
    }

    private void Union(Tower tower1, Tower tower2)
    {
        Destroy(tower1.gameObject);
        tower2.LevelUp();
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
