
using System.Collections.Generic;
using UnityEngine;


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
        //Shot();
        attackController.TowersData(towers, removeTowers);
        ClearRemoveTowers();
        return true;
    }

    public void Shot()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i].towerScript.Body.activeSelf)
            {
                towers[i].reload += Time.deltaTime;
                if (towers[i].reload >= towers[i].attackSpeed)
                {
                    towers[i].reload = 0;
                    towers[i].shot = true;
                    TowerTarget(i);
                }
            }
        }
    }

    void TowerTarget( int towerNum)
    {
        TargetPoint targetPoint = attackController.SetMainTarget();
        //TO DO: Добавить правила для проверки, чтобы переключать цели!!!
        if (targetPoint != null)
        {
            towers[towerNum].target = targetPoint;
        }
        else
        {
            //towers[towerNum].target = target;
        }

    }


    public class TowerInfo
    {
        public Tower towerScript;
        public GameObject towerObject;
        public Transform transform;
        public Transform Gun;
        public Transform turret;
        public TargetPoint target = null;
        public List<WarEntity> shells = new List<WarEntity>();
        public float attackSpeed;
        public float damage;
        public bool shot = false;

        public float reload = 0;
    }





    public void Info()
    {
        Debug.Log($"Кол-во башен {towers.Count}");
        for (int i = 0; i < towers.Count; i++)
        {
            Debug.Log($"Turret{i} {towers[i].turret.position}");
        }
    }

    public void TowerInitialization(Tower tower)
    {
        towers.Add(new TowerInfo
        {
            towerScript = tower,
            towerObject = tower.gameObject,
            transform = tower.transform,
            turret = tower.Turret,
            shot = false,
            damage = 20f,
            attackSpeed = 1f
        });
    }

    List<int> removeTowers = new List<int>();

    public List<int> RemoveTowers
    {
        get { return removeTowers; }
    }

    void ClearRemoveTowers()
    {
        removeTowers.Clear();
    }
    public void SetActiveTower(int i)
    {
        towers[i].towerScript.Body.SetActive(false);

        removeTowers.Add(i);

    }

}

