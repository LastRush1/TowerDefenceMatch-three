using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{

    int level = 1;
    public int GetTowerLevel
    {
        get { return level; }
    }

    [SerializeField]
    int id = 0;

    public int GetTowerId
    {
        get { return id; }
    }

    //int towerNum;

    [SerializeField]
    Text levelNumText;

    void Start()
    {
        changeLevelText(level);
    }


    void Update()
    {
        
    }

    /// <summary>
    /// Вывод текста
    /// </summary>
    /// <param name="level"></param>
    public void changeLevelText(int level)
    {
        levelNumText.text = level.ToString();
    }

    public void LevelUp()
    {
        level++;
        changeLevelText(level);
    }
}
