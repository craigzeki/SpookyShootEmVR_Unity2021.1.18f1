//Author: Craig Zeki
//Student ID: zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PooledObject
{
    public GameObject pooledPrefab = null;
    public int numberToPool = 0;
}

public class PoolerManager : MonoBehaviour
{
    [SerializeField] List<PooledObject> pooledObjects = new List<PooledObject>();

    private Dictionary<int, List<GameObject>> pooledDict = new Dictionary<int, List<GameObject>>();
    private Dictionary<int, List<GameObject>> inUseObjectsDict = new Dictionary<int, List<GameObject>>();

    private void Awake()
    {
        //create the initial pool of objects
        foreach (PooledObject pooledObject in pooledObjects)
        {
            
            resetPool(pooledObject);
        }
    }

    private void resetPool(PooledObject pooledObject)
    {
        int key = pooledObject.pooledPrefab.GetInstanceID();
        pooledDict[key].Clear();
        inUseObjectsDict[key].Clear();
        
        for (int i = 0; i < pooledObject.numberToPool; i++)
        {
            addToPool(key, Instantiate(pooledObject.pooledPrefab, transform));
        }
    }

    public GameObject getFromPool(GameObject prefab)
    {
        int key = prefab.GetInstanceID();
        GameObject poolObject = null;
        if (pooledDict[key].Count > 0)
        {
            poolObject = pooledDict[key][pooledDict[key].Count - 1];
            inUseObjectsDict[key].Add(poolObject);
            pooledDict[key].RemoveAt(pooledDict[key].Count - 1);
        }
        return poolObject;
    }

    public GameObject getFromPool(GameObject prefab, Transform parent)
    {
        GameObject poolObject = getFromPool(prefab);
        if (poolObject != null)
        {
            poolObject.transform.SetParent(parent);
        }
        return poolObject;
    }

    public void returnToPool(GameObject prefab, GameObject pooledObject)
    {
        int key = prefab.GetInstanceID();
        if (inUseObjectsDict[key].Contains(pooledObject))
        {
            inUseObjectsDict[key].Remove(pooledObject);
            addToPool(key, pooledObject);
        }
    }

    private void addToPool(int poolKey, GameObject pooledGameObject)
    {
        if (!pooledDict[poolKey].Contains(pooledGameObject))
        {
            pooledDict[poolKey].Add(pooledGameObject);
            pooledGameObject.transform.position = transform.position;
            pooledGameObject.SetActive(false);
        }
    }
}
