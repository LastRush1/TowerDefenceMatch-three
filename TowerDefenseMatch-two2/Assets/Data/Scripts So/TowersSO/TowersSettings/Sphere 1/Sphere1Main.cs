using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[CreateAssetMenu]
public class Sphere1Main : BasicInfoTower
{
    [SerializeField]
    List<Sphere1> towers = new List<Sphere1>();
    public List<Sphere1> Towers
    {
        get { return towers; }
    }
}
