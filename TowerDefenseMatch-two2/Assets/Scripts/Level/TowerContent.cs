using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerContent : MonoBehaviour
{
    protected int level = 1;
    public int GetTowerLevel
    {
        get { return level; }
    }

    [SerializeField]
    protected int id = 0;

    public int GetTowerId
    {
        get { return id; }
    }
}
