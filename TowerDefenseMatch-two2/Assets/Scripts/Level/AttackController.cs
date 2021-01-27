using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using System;

public class AttackController : GameBehavior
{
    [SerializeField]
    WarFactory warFactory;

    List<TowerController.TowerInfo> towers = new List<TowerController.TowerInfo>();




    [SerializeField]
    Shell prefabShell;

    //Главная цель всех башен(если они не бьют случайную цель)
    Transform targetEnemy;

    List<Transform> enemyPos = new List<Transform>();

    List<int> enemyGridPos = new List<int>();

    List<TargetPoint> enemytargetPoints = new List<TargetPoint>();
    public override bool GameUpdate()
    {
        ShellHits();
        ShellCreater();
        shellsDestroyer();
        AttackSystem();

        return false;
    }

    private void ShellHits()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            for (int j = 0; j < towers[i].shells.Count; j++)
            {
                if (!towers[i].shells[j].Model.activeSelf)
                {
                    //damage
                }
            }
        }
    }

    private void ShellCreater()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i].shot)
            {
                towers[i].shot = false;
                Shot(i);
            }
        }
    }



    private void Shot(int i)
    {
        var shell = warFactory.Get(prefabShell);
        shell.spawnPoint = towers[i].towerScript.Turret;
        shell.transform.position = shell.spawnPoint.position;
        shell.SetDamage(towers[i].damage);
        towers[i].shells.Add(shell);
    }


    public void EnemyData(List<Transform> enemyPos, List<int> enemyGridPos, List<TargetPoint> enemytargetPoints)
    {
        // TO DO: 
        //еще нужно знать когда умер враг, чтобы уничтожать снаряда
        //пока они полетят в другу цель!!!!
        this.enemyPos = enemyPos;
        this.enemyGridPos = enemyGridPos;
        this.enemytargetPoints = enemytargetPoints;

    }

    public void TowersData(List<TowerController.TowerInfo> towers, List<int> removeTowers)
    {
        this.towers = towers;
        this.removeTowers = removeTowers;
    }

    List<int> removeTowers = new List<int>();


    /// <summary>
    /// Определения более приоритетной цели(пока в бета версии) 
    /// </summary>
    public TargetPoint SetMainTarget()
    {
        // TO DO: 
        //поумать как улучшить точность без потери производительности

        if (enemyGridPos.Count > 0)
        {
            int maxGrid = 0;
            int num = 0;

            for (int i = 0; i < enemyGridPos.Count; i++)
            {
                if (maxGrid < enemyGridPos[i])
                {
                    maxGrid = enemyGridPos[i];
                    num = i;
                }
            }
            return enemytargetPoints[num];
        }
        return null;
    }

    /// <summary>
    /// Удаляет неактивные снаряды(переписать, чтобы их можно снова использовтаь а не удалять!)
    /// </summary>
    void shellsDestroyer()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            if (!towers[i].towerScript.Body.activeSelf)
            {
                if (towers[i].shells.Count > 0)
                {
                    for (int j = 0; j < towers[i].shells.Count; j++)
                    {
                        Destroy(towers[i].shells[j].gameObject);
                    }
                    towers[i].shells.Clear();
                }
                
            }
        }
    }

    void AttackSystem()
    {
        int shellsCount = 0;
        for (int i = 0; i < towers.Count; i++)
        {
            for (int j = 0; j < towers[i].shells.Count; j++)
            {
                if (towers[i].towerScript.Body.activeSelf)
                {
                    shellsCount++;
                }   
            }
        }

        if (shellsCount != 0)
        {
            NativeArray<float3> positionEnemyArray = new NativeArray<float3>(towers.Count, Allocator.TempJob);
            NativeArray<float3> positionTowerArray = new NativeArray<float3>(towers.Count, Allocator.TempJob);
            NativeArray<float3> positionShellArray = new NativeArray<float3>(shellsCount, Allocator.TempJob);
            NativeArray<float3> track = new NativeArray<float3>(towers.Count, Allocator.TempJob);
            //Ввод данных
            for (int i = 0; i < towers.Count; i++)
            {
                //Debug.Log($"towers[{i}].turret.position {towers[i].turret.position}");
                if (towers[i].target != null)
                {
                    positionEnemyArray[i] = towers[i].target.Position; //А если будут башни с рандомной атакой??
                }
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
            for (int i = 0; i < towers.Count; i++)
            {
                for (int j = 0; j < towers[i].shells.Count; j++)
                {
                    positionShellArray[num] = towers[i].shells[j].transform.position;
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
            for (int i = 0; i < towers.Count; i++)
            {
                for (int j = 0; j < towers[i].shells.Count; j++)
                {
                    towers[i].shells[j].transform.position = positionShellArray[number];
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


}

[BurstCompile]
public struct TowersAttack : IJobParallelFor
{
    public NativeArray<float3> trackArray;
    public NativeArray<float3> positionShellArray;

    [ReadOnly] public float deltaTime;



    public void Execute(int index)
    {
        int speed = 3;     //Пока ускорил в 3 раза, если хочешь чтобы это можно настривтаь было пили переменную
        positionShellArray[index] += speed * new float3(trackArray[index].x * deltaTime, trackArray[index].y * deltaTime, trackArray[index].z * deltaTime);
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
