using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pool : MonoBehaviour
{
    [SerializeField] private GameObject pooledPrefab = null;
    [SerializeField] private int numberToPool = 0;
    [SerializeField] private bool createOnDemand = false;

    private List<GameObject> pool = new List<GameObject>();
    private List<GameObject> inUseObjects = new List<GameObject>();

    private void Awake()
    {
        //create the initial pool of objects
        resetPool();
    }

    private void resetPool()
    {
        pool.Clear();
        inUseObjects.Clear();
        for (int i = 0; i < numberToPool; i++)
        {
            addToPool(Instantiate(pooledPrefab, transform));
        }
    }

    public GameObject getFromPool()
    {
        GameObject poolObject = null;
        if (pool.Count > 0)
        {
            poolObject = pool[pool.Count - 1];
            inUseObjects.Add(poolObject);
            pool.RemoveAt(pool.Count - 1);
        }
        return poolObject;
    }

    public GameObject getFromPool(Transform parent)
    {
        GameObject poolObject = getFromPool();
        if(poolObject != null)
        {
            poolObject.transform.SetParent(parent);
        }
        return poolObject;
    }

    public void returnToPool(GameObject pooledObject)
    {
        if(inUseObjects.Contains(pooledObject))
        {
            inUseObjects.Remove(pooledObject);
            addToPool(pooledObject);
        }
    }

    private void addToPool(GameObject pooledObject)
    {
        if(!pool.Contains(pooledObject))
        {
            pool.Add(pooledObject);
            pooledObject.transform.position = transform.position;
            pooledObject.SetActive(false);
        }
    }

}
