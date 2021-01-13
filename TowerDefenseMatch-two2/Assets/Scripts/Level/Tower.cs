using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : TowerContent
{

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
