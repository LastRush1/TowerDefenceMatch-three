using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
/// <summary>
/// SO Для всех существующих башен в игре
/// </summary>
public class AllTowers : ScriptableObject
{
    [SerializeField]
    List<TowerAbstractSO> towers = new List<TowerAbstractSO>();
}
