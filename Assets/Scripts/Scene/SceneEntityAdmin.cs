using System.Collections;
using System.Collections.Generic;

using UnityEditor.Build.Pipeline;

using UnityEngine;

public class SceneEntityAdmin : MonoBehaviour
{
    private Dictionary<string, EntityPool> _entityPools;

    public EntityPool GetEntityPool(string _Name) => _entityPools[_Name];

    public void MakeEntityPool(string _Name, GameObject _Prefab, int _StartAlloc)
    {
        MakeEntityPool(_Name, transform, _Prefab, _StartAlloc);
    }

    public void MakeEntityPool(string _Name, Transform _Parent, GameObject _Prefab, int _StartAlloc)
    {
        EntityPool pool = new GameObject(_Name).AddComponent<EntityPool>();
        pool.StartAllocSize = _StartAlloc;
        pool.PoolingObject = _Prefab;
        pool.transform.parent = _Parent;
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
    }

    public void OnSceneUpdate()
    {
    }
}
