using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[CreateAssetMenu]
public class Cube2Main : BasicInfoTower
{
    [SerializeField]
    List<Cube2> towers = new List<Cube2>();
    public List<Cube2> Towers
    {
        get { return towers; }
    }
}
