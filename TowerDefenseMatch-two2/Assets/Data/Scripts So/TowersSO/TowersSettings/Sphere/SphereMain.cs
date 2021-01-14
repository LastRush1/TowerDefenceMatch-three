using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[CreateAssetMenu]
public class SphereMain : BasicInfoTower
{
    [SerializeField]
    List<Sphere> towers = new List<Sphere>();
    public List<Sphere> Towers
    {
        get { return towers; }
    }
}
