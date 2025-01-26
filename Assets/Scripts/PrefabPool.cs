using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class PrefabPool : MonoBehaviour
{
	public GameObject pooledPrefab;

	// Collection checks will throw errors if we try to release an item that is already in the pool.
	public bool collectionChecks = true;
	public int maxPoolSize = 10;

	private IObjectPool<GameObject> objectPool;

	public IObjectPool<GameObject> Pool
	{
		get
		{
			if (objectPool == null)
			{
				objectPool = new ObjectPool<GameObject>(
					OnCreatePooledItem, 
					OnTakeFromPool, 
					OnReturnedToPool, 
					OnDestroyPoolObject, 
					collectionChecks, 
					10, maxPoolSize);
			}

			return objectPool;
		}
	}

	private GameObject OnCreatePooledItem()
    {
        GameObject spawnedPrefab = GameObject.Instantiate(pooledPrefab);
		spawnedPrefab.transform.SetParent(this.transform, false);
		return spawnedPrefab;
    }
 	
	private void OnTakeFromPool(GameObject gameObject)
    {
		gameObject.SetActive(true);
    }

    private void OnReturnedToPool(GameObject gameObject)
    {
		gameObject.transform.SetParent(this.transform);
        gameObject.SetActive(false);
    }

	private void OnDestroyPoolObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

	void OnGUI()
	{
		GUILayout.Label("Pool size: " + Pool.CountInactive);
	}
}