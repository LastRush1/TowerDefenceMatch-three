using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Entities;
using Unity.Collections;

public class AttackController : GameBehavior
{
    [SerializeField]
    WarFactory warFactory;

    List<TowerController.TowerInfo> towers = new List<TowerController.TowerInfo>();

    List<List<WarEntity>> shells = new List<List<WarEntity>>();

    [SerializeField]
    Shell prefabShell;

    //Главная цель всех башен(если они не бьют случайную цель)
    Transform targetEnemy;

    List<Transform> enemyPos = new List<Transform>();

    List<int> enemyGridPos = new List<int>();
    public override bool GameUpdate()
    {
        ShellCreater();

        if ((targetEnemy !=null)&(shells.Count > 0))
        {
            AttackSystem();
        }

        return false;
    }

    private void ShellCreater()
    {
        List<WarEntity> r = new List<WarEntity>();
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i].shot)
            {
                towers[i].shot = false;
                shells.Add(r);
                Shot(i);
            }
        }
    }

    private void Shot(int i)
    {
        var shell = warFactory.Get(prefabShell);
        shell.spawnPoint = towers[i].turret;
        shell.transform.position = shell.spawnPoint.position;
        shells[i].Add(shell);
    }

    void Start()
    {
        
    }

    public void EnemyData(List<Transform> enemyPos, List<int> enemyGridPos)
    {
        // TO DO: 
        //еще нужно знать когда умер враг, чтобы уничтожать снаряда
        //пока они полетят в другу цель!!!!
        this.enemyPos = enemyPos;
        this.enemyGridPos = enemyGridPos;
        SetMainTarget();
    }

    public void TowersData(List<TowerController.TowerInfo> towers)
    {
        this.towers = towers;
    }

    /// <summary>
    /// Определения более приоритетной цели(пока в бета версии) 
    /// </summary>
    private void SetMainTarget()
    {
        // TO DO: 
        //поумать как улучшить точность без потери производительности
        if (enemyGridPos.Count > 0)
        {
            targetEnemy = enemyPos[0];
            for (int i = 1; i < enemyGridPos.Count; i++)
            {
                if (enemyGridPos[i - 1] < enemyGridPos[i])
                {
                    targetEnemy = enemyPos[i];
                }
            }
            //Debug.Log($"Позиция врага {targetEnemy.position}");
        }

    }

    void AttackSystem()
    {
        int shellsCount = 0;
        for (int i = 0; i < shells.Count; i++)
        {
            for (int j = 0; j < shells[i].Count; j++)
            {
                shellsCount++;
            }
        }
        Debug.Log($"shellsCount {shellsCount}");
        NativeArray<float3> positionEnemyArray = new NativeArray<float3>(towers.Count, Allocator.TempJob);
        NativeArray<float3> positionTowerArray = new NativeArray<float3>(towers.Count, Allocator.TempJob);
        NativeArray<float3> positionShellArray = new NativeArray<float3>(shellsCount, Allocator.TempJob);
        NativeArray<float3> track = new NativeArray<float3>(towers.Count, Allocator.TempJob);
        //Ввод данных
        for (int i = 0; i < towers.Count; i++)
        {
            positionEnemyArray[i] = targetEnemy.position; //А если будут башни с рандомной атакой??
            positionTowerArray[i] = towers[i].turret.position;
        }


        //Ввод данных в Job system
        TrackOutOfTowerInEnemy ParallelJobTrack = new TrackOutOfTowerInEnemy
        {
            deltaTime = Time.deltaTime,
            positionEnemyArray = positionEnemyArray,
            positionTowerArray = positionTowerArray,
            trackArray = track

        };

        //Запуск Job System
        JobHandle jobHandleTrack = ParallelJobTrack.Schedule(towers.Count, 1);

        jobHandleTrack.Complete();

        NativeArray<float3> tracker = new NativeArray<float3>(shellsCount, Allocator.TempJob);

        int num = 0;
        for (int i = 0; i < shells.Count; i++)
        {
            for (int j = 0; j < shells[i].Count; j++)
            {
                positionShellArray[num] = shells[i][j].transform.position;
                tracker[num] = track[i];
                num++;
            }
        }

        //Ввод данных в Job system
        TowersAttack ParallelJob = new TowersAttack
        {
            deltaTime = Time.deltaTime,
            positionShellArray = positionShellArray,
            trackArray = tracker
        };



        //Запуск Job System
        JobHandle jobHandle = ParallelJob.Schedule(positionShellArray.Length, 1);

        jobHandle.Complete();
        //Вывод данных
        int number = 0;
        for (int i = 0; i < shells.Count; i++)
        {
            for (int j = 0; j < shells[i].Count; j++)
            {
                shells[i][j].transform.position = positionShellArray[number];
                number++;
            }
        }



        //Чистим память
        positionEnemyArray.Dispose();
        positionShellArray.Dispose();
        positionTowerArray.Dispose();
        track.Dispose();
        tracker.Dispose();
    }


}

[BurstCompile]
public struct TowersAttack : IJobParallelFor
{
    public NativeArray<float3> trackArray;
    public NativeArray<float3> positionShellArray;

    [ReadOnly] public float deltaTime;

    public void Execute(int index)
    {
        positionShellArray[index] += new float3(trackArray[index].x * deltaTime, trackArray[index].y * deltaTime, trackArray[index].z * deltaTime);
    }
}

[BurstCompile]
public struct TrackOutOfTowerInEnemy : IJobParallelFor
{
    public NativeArray<float3> positionEnemyArray;
    public NativeArray<float3> positionTowerArray;
    public NativeArray<float3> trackArray;

    [ReadOnly] public float deltaTime;

    public void Execute(int index)
    {
        float3 track;
        track.x = positionEnemyArray[index].x - positionTowerArray[index].x;
        track.y = positionEnemyArray[index].y - positionTowerArray[index].y;
        track.z = positionEnemyArray[index].z - positionTowerArray[index].z;
        trackArray[index] = track;
    }
}
