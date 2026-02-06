using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{    
    [Header("Pool Configuration")]
    [SerializeField] private List<PoolItem> pools = new List<PoolItem>();

    [Header("Default Settings")]
    [SerializeField] private Transform defaultParent;
   
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary;
    
    private Dictionary<GameObject, PoolItem> poolConfigDictionary;    

    private void Awake()
    {
        InitializePools();
    }
    
    private void InitializePools()
    {
        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
        poolConfigDictionary = new Dictionary<GameObject, PoolItem>();       

        foreach (PoolItem poolItem in pools)
        {
            if (poolItem.prefab == null)
            {
                Debug.LogWarning("Pool has null prefab. Skipping.");
                continue;
            }
           
            if (poolItem.tempParent == null)
            {
                poolItem.tempParent = defaultParent != null ? defaultParent : transform;
            }
            
            if (poolDictionary.ContainsKey(poolItem.prefab))
            {
                Debug.LogWarning($"Duplicate pool for prefab: {poolItem.prefab.name}. Using first occurrence.");
                continue;
            }

            // Initialize dictionaries for this prefab
            poolDictionary.Add(poolItem.prefab, new Queue<GameObject>());
            poolConfigDictionary.Add(poolItem.prefab, poolItem);            

            // Pre-instantiate initial objects
            for (int i = 0; i < poolItem.initialSize; i++)
            {
                CreateNewObject(poolItem.prefab);
            }

            Debug.Log($"Initialized pool for {poolItem.prefab.name}: {poolItem.initialSize} objects created");
        }
    }

    
    // Spawns an object from the pool    
    public GameObject Spawn(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("Cannot spawn null prefab");
            return null;
        }
        
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning($"No pool found for {prefab.name}.");
            return null;
        }

        GameObject obj = null;
        Queue<GameObject> availableQueue = poolDictionary[prefab];
        PoolItem poolConfig = poolConfigDictionary[prefab];
       
        if (availableQueue.Count > 0)
        {
            obj = availableQueue.Dequeue();
            
            if (obj == null)
            {
                // Object was destroyed externally, create new one
                obj = CreateNewObject(prefab);
            }
        }
        else
        {
            // Pool is empty
            if (poolConfig.autoExpand)
            {               
                obj = CreateNewObject(prefab);
            }
            else
            {                
                Debug.LogWarning($"Pool for {prefab.name}");
                return null;
            }
        }
        
        if (obj != null)
        {
            obj.SetActive(true);
        }

        return obj;
    }                

   
    // Spawns an object and returns a specific component
    public T Spawn<T>(GameObject prefab) where T : Component
    {
        GameObject obj = Spawn(prefab);
        if (obj != null)
        {
            return obj.GetComponent<T>();
        }
        return null;
    }
        

    /// <summary>
    /// Returns an object to the pool
    /// </summary>
    /// <param name="prefab">The prefab type this object belongs to</param>
    /// <param name="obj">Object to despawn</param>
    /// <returns>True if successfully despawned</returns>
    public bool Despawn(GameObject prefab, GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("Attempted to despawn null object");
            return false;
        }

        if (prefab == null)
        {
            Debug.LogError("Prefab reference is null");
            return false;
        }
       
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning($"No pool found for prefab: {prefab.name}. Destroying object instead.");
            Destroy(obj);
            return false;
        }
        
        PoolItem poolConfig = poolConfigDictionary[prefab];

        // Reset object
        obj.SetActive(false);
        obj.transform.SetParent(poolConfig.tempParent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        // Return to available pool
        poolDictionary[prefab].Enqueue(obj);

        return true;
    }

   
    // Despawns object after a delay   
    public void DespawnDelayed(GameObject prefab, GameObject obj, float delay)
    {
        if (obj != null)
        {
            StartCoroutine(DespawnDelayedCoroutine(prefab, obj, delay));
        }
    }

    private System.Collections.IEnumerator DespawnDelayedCoroutine(GameObject prefab, GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Despawn(prefab, obj);
    }

    /// <summary>
    /// Despawns all objects of a specific type by collecting all active instances
    /// Note: This searches through all children of the pool's tempParent
    /// </summary>
    public void DespawnAll(GameObject prefab)
    {
        if (prefab == null || !poolDictionary.ContainsKey(prefab))
        {
            return;
        }

        PoolItem poolConfig = poolConfigDictionary[prefab];

        // Find all active instances under the temp parent
        List<GameObject> objectsToDespawn = new List<GameObject>();
        foreach (Transform child in poolConfig.tempParent)
        {
            if (child.gameObject.activeSelf)
            {
                objectsToDespawn.Add(child.gameObject);
            }
        }

        foreach (GameObject obj in objectsToDespawn)
        {
            Despawn(prefab, obj);
        }
    }

    // <summary>
    // Creates a new object for the pool   
    private GameObject CreateNewObject(GameObject prefab)
    {
        PoolItem poolConfig = poolConfigDictionary[prefab];        
        GameObject obj = Instantiate(prefab, poolConfig.tempParent);
        obj.name = $"{prefab.name}";
        obj.SetActive(false);

        poolDictionary[prefab].Enqueue(obj);        

        return obj;
    }
           
    public void ClearAllPools()
    {
        foreach (var kvp in poolDictionary)
        {
            GameObject prefab = kvp.Key;
            ClearPool(prefab);
        }

        poolDictionary.Clear();
        poolConfigDictionary.Clear();        
    }

    private void ClearPool(GameObject prefab)
    {
        // Destroy available objects in queue
        Queue<GameObject> availableQueue = poolDictionary[prefab];
        while (availableQueue.Count > 0)
        {
            GameObject obj = availableQueue.Dequeue();
            if (obj != null)
            {
                Destroy(obj);
            }
        }

        // Destroy any remaining objects under temp parent
        PoolItem poolConfig = poolConfigDictionary[prefab];
        List<Transform> childrenToDestroy = new List<Transform>();
        if(poolConfig.tempParent != null)
        {
            foreach (Transform child in poolConfig.tempParent)
            {
                if (child != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }                
    }

    private void OnDestroy()
    {
        ClearAllPools();
    }
}

[System.Serializable]
public class PoolItem
{
    [Tooltip("The prefab to pool")]
    public GameObject prefab;
    [Tooltip("Initial number of objects to create")]
    public int initialSize = 10;
    [Tooltip("Parent transform for this pool's objects")]
    public Transform tempParent;
    [Tooltip("Auto-expand when pool is exhausted")]
    public bool autoExpand = true;
}