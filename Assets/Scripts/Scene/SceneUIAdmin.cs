using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUIAdmin : MonoBehaviour
{
    [HideInInspector] public Scene CurrentScene;
    public SceneUI ActiveUIPrefab;
    public SceneUI CurrentActiveUI;

    public void OnSceneFixedUpdate()
    {
        CurrentActiveUI.OnSceneFixedUpdate();
    }

    public void OnSceneLateUpdate()
    {
        CurrentActiveUI.OnSceneLateUpdate();
    }

    public void OnSceneLoad()
    {
        CurrentScene = SceneManager.instance.CurrentActiveScene;

        CurrentActiveUI = Instantiate(ActiveUIPrefab.gameObject, transform).GetComponent<SceneUI>();

        CurrentActiveUI.ManagedAdmin = this;

        CurrentActiveUI.OnSceneLoad();
    }

    public void OnSceneUnload()
    {
        CurrentActiveUI.OnSceneUnload();
        Destroy(CurrentActiveUI.gameObject);
    }

    public void OnSceneUpdate()
    {
        CurrentActiveUI.OnSceneUpdate();
    }
}
