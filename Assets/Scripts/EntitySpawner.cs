using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    private Spawnpoint[] entitySpawnpoints;
    private int enemySpawnerCount;


    private void Update()
    {
        if(GameManager.Instance.currentGameState == GameState.Playing)
        {
            SpawnEntities();
        }
    }


    public void SpawnEntities()
    {
        entitySpawnpoints = GetComponentsInChildren<Spawnpoint>();

        for (int i = 0; i < entitySpawnpoints.Length; i++)
        {
            //instantiating entity prefab based on spawnpoint type
            if(entitySpawnpoints[i].spawnType == SpawnType.Player)
            {
                if(GameManager.Instance.isPlayerSpawned == false)
                {
                    GameManager.Instance.isPlayerSpawned = true;
                    SpawnEntity(GameManager.Instance.playerPrefab, entitySpawnpoints[i].transform);
                }
            }

            if(entitySpawnpoints[i].spawnType == SpawnType.Enemy)
            {
                enemySpawnerCount++;
                if(GameManager.Instance.isEnemySpawned == false)
                {
                    SpawnEntity(GameManager.Instance.enemyPrefab, entitySpawnpoints[i].transform);
                }
            }
        }

        //limiting enemy spawns so it wont spawn millions LOL
        if(enemySpawnerCount >= entitySpawnpoints.Length -1)
        {
            GameManager.Instance.isEnemySpawned = true;
        }
    }

    public void SpawnEntity(GameObject entityPrefab, Transform transform)
    {
        Instantiate(entityPrefab, transform.position, Quaternion.identity);
    }

}
