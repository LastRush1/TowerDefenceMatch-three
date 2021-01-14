using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
/// <summary>
/// Пул башен для уровня
/// </summary>
public class LevelTowersPull : ScriptableObject
{
    [SerializeField]
    List<TowerAbstractSO> towers = new List<TowerAbstractSO>();
}
