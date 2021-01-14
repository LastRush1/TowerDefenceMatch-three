using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    EnemyFactory enemyFactory;

    List<Enemy> enemies = new List<Enemy>();

    private void Update()
    {
        MoveEnemies();
    }

    private void MoveEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.position = new Vector2(enemies[i].transform.position.x, enemies[i].transform.position.y + 0.02f);
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
        enemies.Add(enemy);
        enemy.transform.position = spawn.position;
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
