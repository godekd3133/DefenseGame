using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

using UnityEngine;

public abstract class Scene : MonoBehaviour
{
    private SceneEntityAdmin _sceneEntityAdmin;
    private SceneUIAdmin _sceneUIAdmin;
    private SceneCamera _sceneCamera;
    private Dictionary<Type, object> _sceneManager;

    public SceneCamera Camera { get; }
    public SceneUIAdmin UIAdmin { get; }
    public SceneEntityAdmin EntityAdmin { get; }
    public T Manager<T>() => (T)_sceneManager[typeof(T)];

    private void Awake()
    {
        _sceneManager = new Dictionary<Type, object>();

        _sceneEntityAdmin = GetComponentInChildren<SceneEntityAdmin>();
        _sceneCamera = GetComponentInChildren<SceneCamera>();
        _sceneUIAdmin = GetComponentInChildren<SceneUIAdmin>();
    }

    //씬 로드/언로드 되었을때
    public virtual void OnSceneLoad() 
    {
        _sceneUIAdmin.OnSceneLoad();
        _sceneEntityAdmin.OnSceneLoad(); 
    }


    public virtual void OnSceneUnload()
    {
        _sceneUIAdmin.OnSceneUnload();
        _sceneEntityAdmin.OnSceneUnload();
    }

    //Update
    public virtual void OnSceneUpdate()
    {
        _sceneUIAdmin.OnSceneUpdate();
        _sceneEntityAdmin.OnSceneUpdate();
    }

    public virtual void OnSceneLateUpdate()
    {
        _sceneUIAdmin.OnSceneLateUpdate();
        _sceneEntityAdmin.OnSceneLateUpdate();
    }

    public virtual void OnSceneFixedUpdate()
    {
        _sceneUIAdmin.OnSceneFixedUpdate();
        _sceneEntityAdmin.OnSceneFixedUpdate();
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
