using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

using UnityEngine;

public abstract class Scene : MonoBehaviour
{
    private Dictionary<Type, object> _sceneManager;

    public SceneCamera Camera { get; private set; }
    public SceneUIAdmin UIAdmin { get; private set; }
    public SceneEntityAdmin EntityAdmin { get; private set; }
    public T Manager<T>() => (T)_sceneManager[typeof(T)];

    private void Awake()
    {
        _sceneManager = new Dictionary<Type, object>();

        EntityAdmin = GetComponentInChildren<SceneEntityAdmin>();
        Camera = GetComponentInChildren<SceneCamera>();
        UIAdmin = GetComponentInChildren<SceneUIAdmin>();
    }

    //씬 로드/언로드 되었을때
    public virtual void OnSceneLoad()
    {
        EntityAdmin.OnSceneLoad();
        UIAdmin.OnSceneLoad();
    }


    public virtual void OnSceneUnload()
    {
        EntityAdmin.OnSceneUnload();
        UIAdmin.OnSceneUnload();
    }

    //Update
    public virtual void OnSceneUpdate()
    {
        EntityAdmin.OnSceneUpdate();
        UIAdmin.OnSceneUpdate();
    }

    public virtual void OnSceneLateUpdate()
    {
        EntityAdmin.OnSceneLateUpdate();
        UIAdmin.OnSceneLateUpdate();
    }

    public virtual void OnSceneFixedUpdate()
    {
        EntityAdmin.OnSceneFixedUpdate();
        UIAdmin.OnSceneFixedUpdate();
    }

    private void Update()
    {
        OnSceneUpdate();
    }

    private void LateUpdate()
    {
        OnSceneLateUpdate();
    }

    private void FixedUpdate()
    {
        OnSceneFixedUpdate();
    }

}
