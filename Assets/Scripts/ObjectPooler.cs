using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObjectType{
    goombaEnemy=0,
    greenEnemy=1
}

[System.Serializable]
public class ObjectPoolItem
{
    public int amount;
    public GameObject prefab;
    public bool expandPool;
    public ObjectType type;
}

public class ExistingPoolItem
{
    public GameObject gameObject;
    public ObjectType type;

    public ExistingPoolItem(GameObject gameObject, ObjectType type)
    {
        this.gameObject = gameObject;
        this.type = type;
    }
}

public class ObjectPooler : MonoBehaviour
{
    public List<ObjectPoolItem> itemsToPool;
    public List<ExistingPoolItem> pooledObjects;
    private int idGenerator = 0;

    void Awake()
    {
        SharedInstance = this;
        pooledObjects = new List<ExistingPoolItem>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amount; i++)
            {
                GameObject pickup = (GameObject)Instantiate(item.prefab);
                pickup.SetActive(false);
                pickup.transform.parent = this.transform;
                //pickup.transform.GetChild(1).GetComponent<EnemyController>().id = idGenerator;
                ExistingPoolItem e = new ExistingPoolItem(pickup,item.type);
                pooledObjects.Add(e);
                idGenerator++;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static ObjectPooler SharedInstance;

    public GameObject GetPooledObject(ObjectType type)
    {
        for(int i = 0; i< pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].gameObject.activeInHierarchy && pooledObjects[i].type == type)
            {
                return pooledObjects[i].gameObject;
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.type == type)
            {
                GameObject pickup = (GameObject)Instantiate(item.prefab);
                pickup.SetActive(false);
                pickup.transform.parent = this.transform;
                pooledObjects.Add(new ExistingPoolItem(pickup, item.type));
                return pickup;
            }
        }
        return null;
    }
}
