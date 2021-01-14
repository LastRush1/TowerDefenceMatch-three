using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAbstractSO : ScriptableObject
{
    [SerializeField]
    protected Tower prefab = default;

    public Tower Prefab
    {
        get { return prefab; }
    }

    [SerializeField]
    protected float attack = default;

    public float Attack
    {
        get { return attack; }
    }

    [SerializeField]
    protected int level = default;

    public int Level
    {
        get { return level; }
    }
}
