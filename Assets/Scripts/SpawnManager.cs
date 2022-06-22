using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    public GameConstants gameConstants;
    public float minSpawnXPos = 9f;
    public float maxSpawnXPos = 13f;
    public float manualSurfaceOverride = 0f;
    void Start()
    {
        for (int j = 0; j < gameConstants.maxEnemySpawn; j++)
            spawnFromPooler(ObjectType.goombaEnemy);

    }

    void startSpawn(Scene scene, LoadSceneMode mode)
    {
        for (int j = 0; j < gameConstants.maxEnemySpawn; j++)
            spawnFromPooler(ObjectType.goombaEnemy);
    }


    void spawnFromPooler(ObjectType i)
    {
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(i);

        if (item != null)
        {
            //set position
            item.transform.localScale = new Vector3(1, 1, 1);
            item.transform.position = new Vector3(Random.Range(minSpawnXPos, maxSpawnXPos), gameConstants.groundSurface+manualSurfaceOverride + item.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().bounds.extents.y, 0);
            item.SetActive(true);
        }
        else
        {
            Debug.Log("Not enough items in the pool!");
        }
    }

    public void spawnNewEnemy()
    {
        ObjectType i = ObjectType.goombaEnemy;
        spawnFromPooler(i);
    }
}
