using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInfoTower : ScriptableObject
{
    [SerializeField]
    protected int id = default;

    public int Id
    {
        get { return id; }
    }

    [SerializeField]
    protected string name = default;
    public string Name
    {
        get { return name; }
    }

    [SerializeField]
    protected Sprite icon = default;

    public Sprite Icon
    {
        get { return icon; }
    }
}
