﻿using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
public class Cube1Main : BasicInfoTower
{
    [SerializeField]
    List<Cube1> towers = new List<Cube1>();
    public List<Cube1> Towers
    {
        get { return towers; }
    }
}
