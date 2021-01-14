using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTowers : MonoBehaviour
{
    List<int> idTowers = new List<int>();

    //[SerializeField]
    List<Tower> towers = new List<Tower>();

    public List<Tower> Towers
    {
        get { return towers; }
    }


    [SerializeField]
    TowersPull pull;

    [SerializeField]
    AllTowers allTowers;

    private void Awake()
    {
        LoadingTowers();
    }

   

    void Start()
    {
        
    }

    /// <summary>
    /// Загрузка префабов башен на уровень
    /// </summary>
    private void LoadingTowers()
    {
        for (int i = 0; i < pull.Towers.Count; i++)
        {
            idTowers.Add(pull.Towers[i].Id);
        }


        //Ищем совпадения id со всеми существующими башнями
        for (int j = 0; j < allTowers.Towers.Count; j++)
        {
            for (int i = 0; i < idTowers.Count; i++)
            {
                if (idTowers[i] == j)
                {
                    towers.Add(allTowers.Towers[j].Prefab);
                    break;
                }
            }
        }

        
    }

}
