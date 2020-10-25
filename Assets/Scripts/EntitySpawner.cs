using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    private Spawnpoint[] entitySpawnpoints;

    private GameObject spawnedEntity;
    private List<GameObject> entityList;
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
        if(enemySpawnerCount >= entitySpawnpoints.Length -1) //-1 so it doesn't spawn twice for some reason
        {
            GameManager.Instance.isEnemySpawned = true;
        }
    }

    public void SpawnEntity(GameObject entityPrefab, Transform transform)
    {
        spawnedEntity = Instantiate(entityPrefab, transform.position, Quaternion.identity);
        //entityList.Add(spawnedEntity);
        spawnedEntity = null;
    }

    public void RemoveEntities()
    {
        GameManager.Instance.isEnemySpawned = false;
        GameManager.Instance.isPlayerSpawned = false;

        //delete every object in list
        foreach(GameObject entity in entityList)
        {
            Destroy(entity);
        }
        entityList.Clear();
    }
}
