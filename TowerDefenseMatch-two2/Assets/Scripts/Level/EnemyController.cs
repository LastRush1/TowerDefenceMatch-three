using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Entities;
using Unity.Collections;

public class EnemyController : GameBehavior
{
    [SerializeField]
    EnemyFactory enemyFactory;

    List<EnemyInfo> enemies = new List<EnemyInfo>();

    List<Transform> enemyPos = new List<Transform>();

    List<int> enemyGridPos = new List<int>();

    public List<Transform> EnemyPos
    {
        get { return enemyPos; }
    }

    public List<int> EnemyGridPos
    {
        get { return enemyGridPos; }
    }

    List<Transform>  road = new List<Transform>();

    public class EnemyInfo
    {
        public GameObject enemyObject;
        public Enemy enemyScript;
        public Transform transform;
        public float moveY;
        public float health;
        public int gridPosition;
        public bool finish = false;
        public bool move = false;
    }


    public override bool GameUpdate()
    {
        MoveEnemies();
        EnemyData();
        return true;
    }

    /// <summary>
    /// Информация о местопоожении врага
    /// </summary>
    void EnemyData()
    {
        List<Transform> enemyPos = new List<Transform>();
        List<int> enemyGridPos = new List<int>();

        for (int i = 0; i < enemies.Count; i++)
        {
            enemyPos.Add(enemies[i].transform);

            enemyGridPos.Add(enemies[i].gridPosition);
        }

        this.enemyPos = enemyPos;
        this.enemyGridPos = enemyGridPos;
    }


    private void MoveEnemies()
    {
        MoveSystem();
    }

    /// <summary>
    /// Получаем данные о дороге
    /// </summary>
    /// <param name="road"></param>
    public void RoadSet(List<Road> road)
    {
        for (int i = 0; i < road.Count; i++)
        {
            this.road.Add(road[i].transform);
        }
    }


    //////////////////////////////////////////////////////////////////////////////Spawn//////////////////////////////////////////////////////////////////
    [SerializeField]
    int spawnSpeed = 2;

    Transform spawn;
    IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / spawnSpeed);
            Spawn();
        }
    }

    public void StartSpawn(Transform spawnPos)
    {
        spawn = spawnPos;
        StartCoroutine(Spawner());
    }

    private void Spawn()
    {
        Enemy enemy = enemyFactory.Get();
        enemies.Add(new EnemyInfo
        {
            enemyScript = enemy,
            enemyObject = enemy.gameObject,
            transform = enemy.gameObject.transform,
            gridPosition = 1,
            moveY = UnityEngine.Random.Range(1f, 2f),
            health = 100f,
            move = true,

            finish = false
        });
        enemy.transform.position = spawn.position;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void MoveSystem()
    {
        NativeArray<float3> positionEnemyArray = new NativeArray<float3>(enemies.Count, Allocator.TempJob);
        NativeArray<float> moveSpeedEnemyArray = new NativeArray<float>(enemies.Count, Allocator.TempJob);
        NativeArray<float3> trackTo = new NativeArray<float3>(enemies.Count, Allocator.TempJob);
        NativeArray<float3> trackFrom = new NativeArray<float3>(enemies.Count, Allocator.TempJob);
        NativeArray<int> gridPositionEnemyArray = new NativeArray<int>(enemies.Count, Allocator.TempJob);
        NativeArray<bool> moveEnemyArray = new NativeArray<bool>(enemies.Count, Allocator.TempJob);
        NativeArray<bool> fightEnemyArray = new NativeArray<bool>(enemies.Count, Allocator.TempJob);

        //Ввод данных
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].enemyObject.activeSelf)
            {
                positionEnemyArray[i] = enemies[i].transform.position;
                moveSpeedEnemyArray[i] = enemies[i].moveY;
                if (!enemies[i].finish)
                {
                    trackTo[i] = TrackCalculator(i); 
                    trackFrom[i] = trackTo[i] - 1;
                }

                gridPositionEnemyArray[i] = enemies[i].gridPosition;



                if (enemies[i].finish)
                {
                    positionEnemyArray[i] = new float3(0, 0, 0);
                    enemies[i].enemyObject.SetActive(false);
                    trackTo[i] = 1;
                    trackFrom[i] = 0;
                }
            }

        }
        //Ввод данных в Job system
        EnemyMove ParallelJob = new EnemyMove
        {
            deltaTime = Time.deltaTime,
            positionEnemyArray = positionEnemyArray,
            moveSpeedEnemyArray = moveSpeedEnemyArray,
            trackTo = trackTo,
            trackFrom = trackFrom,
            gridPositionEnemyArray = gridPositionEnemyArray
        };
        //Запуск Job System
        JobHandle jobHandle = ParallelJob.Schedule(enemies.Count, 1);
        jobHandle.Complete();
        //Вывод данных
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].enemyObject.activeSelf)
            {
                enemies[i].transform.position = positionEnemyArray[i];
                enemies[i].gridPosition = gridPositionEnemyArray[i];
            }
        }



        //Чистим память
        positionEnemyArray.Dispose();
        moveSpeedEnemyArray.Dispose();
        trackTo.Dispose();
        trackFrom.Dispose();
        gridPositionEnemyArray.Dispose();
        moveEnemyArray.Dispose();
        fightEnemyArray.Dispose();
    }

    Vector3 TrackCalculator(int num)
    {
        Vector3 trak;
        if (road.Count > 0)
        {
            if (enemies[num].gridPosition < road.Count)
            {
                trak = new Vector3(road[enemies[num].gridPosition].transform.position.x,road[enemies[num].gridPosition].transform.position.y,road[enemies[num].gridPosition].transform.position.z) ;
            }
            else
            {
                enemies[num].finish = true;
                trak = new Vector3(0, 0, 0);
            }
        }
        else
        {
            trak = new Vector3(0, 0, 0);
        }
        return trak;
    }


}


[BurstCompile]
public struct EnemyMove : IJobParallelFor
{
    public NativeArray<float3> positionEnemyArray;
    public NativeArray<float> moveSpeedEnemyArray;
    public NativeArray<float3> trackTo;
    public NativeArray<float3> trackFrom;
    public NativeArray<int> gridPositionEnemyArray;
    [ReadOnly] public float deltaTime;

    public void Execute(int index)
    {
        float x = (trackTo[index].x - trackFrom[index].x) * moveSpeedEnemyArray[index];
        float y = (trackTo[index].y - trackFrom[index].y) * moveSpeedEnemyArray[index];
        float z = (trackTo[index].z - trackFrom[index].z) * moveSpeedEnemyArray[index];

        bool changeX = false;
        bool changeY = false;
        bool changeZ = false;

        //////////////////////////////////////////////////////////////////////Система движения////////////////////////////////////////////////////////////////////////////////
        if (positionEnemyArray[index].x != trackTo[index].x)
        {
            if (math.abs(positionEnemyArray[index].x - trackTo[index].x) < math.abs(x * deltaTime))
            {
                changeX = true;
            }
            else
            {
                if (positionEnemyArray[index].x - trackTo[index].x > 0)
                {
                    positionEnemyArray[index] -= new float3(x * deltaTime, 0, 0);
                }
                else
                {
                    positionEnemyArray[index] += new float3(x * deltaTime, 0, 0);
                }
            }

        }
        if (positionEnemyArray[index].y != trackTo[index].y)
        {
            if (math.abs(positionEnemyArray[index].y - trackTo[index].y) < math.abs(y * deltaTime))
            {
                changeY = true;
            }
            else
            {
                if (positionEnemyArray[index].y - trackTo[index].y > 0)
                {
                    positionEnemyArray[index] -= new float3(0, y * deltaTime, 0);
                }
                else
                {
                    positionEnemyArray[index] += new float3(0, y * deltaTime, 0);
                }
            }
        }
        if (positionEnemyArray[index].z != trackTo[index].z)
        {
            if (math.abs(positionEnemyArray[index].z - trackTo[index].z) < math.abs(z * deltaTime))
            {
                changeZ = true;
            }
            else
            {
                if (positionEnemyArray[index].z - trackTo[index].z > 0)
                {
                    positionEnemyArray[index] -= new float3(0, 0, z * deltaTime);
                }
                else
                {
                    positionEnemyArray[index] += new float3(0, 0, z * deltaTime);
                }
            }
        }

        if (changeX)
        {
            positionEnemyArray[index] = new float3(trackTo[index].x, positionEnemyArray[index].y, positionEnemyArray[index].z);
        }
        if (changeY)
        {
            positionEnemyArray[index] = new float3(positionEnemyArray[index].x, trackTo[index].y, positionEnemyArray[index].z);
        }
        if (changeZ)
        {
            positionEnemyArray[index] = new float3(positionEnemyArray[index].x, positionEnemyArray[index].y, trackTo[index].z);
        }


        if ((positionEnemyArray[index].x - trackTo[index].x == 0)
           & (positionEnemyArray[index].y - trackTo[index].y == 0)
           & (positionEnemyArray[index].z - trackTo[index].z == 0))
        {
            gridPositionEnemyArray[index]++;
        }
    }
}