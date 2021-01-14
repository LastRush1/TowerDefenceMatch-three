using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
/// <summary>
/// Пул башен для уровня
/// </summary>
public class TowersPull : ScriptableObject
{
    [SerializeField]
    List<BasicInfoTower> towers = new List<BasicInfoTower>();

    public List<BasicInfoTower> Towers
    { 
        get { return towers; }
    }
}
