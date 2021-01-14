using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
public class CubeMain : BasicInfoTower
{
    [SerializeField]
    List<Cube> towers = new List<Cube>();

    public List<Cube> Towers
    { 
        get { return towers; }
    }

}
