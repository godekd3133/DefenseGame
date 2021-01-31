using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using UnityEditor.Build.Pipeline;

using UnityEngine;

public class SceneEntityAdmin : MonoBehaviour
{
    private Dictionary<string, EntityPool> _entityPools;

    private GameObject EntityPoolPrefab;

    public EntityPool GetEntityPool(string _Name) => _entityPools[_Name];

    private void Awake()
    {
        EntityPoolPrefab = new GameObject();
        EntityPoolPrefab.hideFlags = HideFlags.HideInHierarchy;
    }

    public void MakeEntityPool(string _Name, GameObject _Prefab, int _StartAlloc)
    {
        MakeEntityPool(_Name, transform, _Prefab, _StartAlloc);
    }

    public void MakeEntityPool(string _Name, Transform _Parent, GameObject _Prefab, int _StartAlloc)
    {
        EntityPool pool = Instantiate(EntityPoolPrefab, _Parent).AddComponent<EntityPool>();
        pool.StartAllocSize = _StartAlloc;
        pool.PoolingObject = _Prefab;
        pool.gameObject.name = _Name;
    }


    public void OnSceneFixedUpdate()
    {
    }

    public void OnSceneLateUpdate()
    {
    }

    public void OnSceneLoad()
    {
    }

    public void OnSceneUnload()
    {
        Destroy(EntityPoolPrefab.gameObject);
    }

    public void OnSceneUpdate()
    {
    }
}
