using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Entities;
using Unity.Collections;

public class TowerController : GameBehavior
{


    List<TowerInfo> towers = new List<TowerInfo>();

    AttackController attackController;

    void Awake()
    {
        attackController = GetComponent<AttackController>();
    }

    void Start()
    {
    }

    public override bool GameUpdate()
    {
        Shot();
        attackController.TowersData(towers);
        return true;
    }

    private void Shot()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            towers[i].reload += Time.deltaTime;
            if (towers[i].reload >= towers[i].attackSpeed)
            {
                towers[i].reload = 0;
                towers[i].shot = true;
            }
        }
    }

    public class TowerInfo
    {
        public Tower towerScript;
        public GameObject towerObject;
        public Transform transform;
        public Transform Gun;
        public Transform target;
        public Transform turret;
        public float attackSpeed;
        public bool shot;
        public float reload = 0;
    }

    public void TowerInitialization(Tower tower)
    {
        towers.Add(new TowerInfo
        {
            towerScript = tower,
            towerObject = tower.gameObject,
            transform = tower.transform,
            turret = tower.Turret,
            attackSpeed = 1f
        });
    }

    public void SetActiveTower(int i)
    {
        towers.RemoveAt(i);
    }

}

