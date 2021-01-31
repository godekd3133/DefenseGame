using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using System.Collections.ObjectModel;
using UnityEditor;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    public ReadOnlyDictionary<string, Scene> Scenes;
    private Scene CurrentActiveScene = null;
    public T GetCurrentActiveScene<T>() where T : Scene
    { return CurrentActiveScene as T; }

    public Transform ActiveSceneParent;


    private void Awake()
    {
        instance = this;
    }

    public void OnApplicationLoadSuccess()
    {
        ChangeScene("TitleScene");
    }

    public void ChangeScene(string sceneName)
    {
        if (CurrentActiveScene != null)
        {
            CurrentActiveScene.OnSceneUnload();
            Destroy(CurrentActiveScene.gameObject);
        }

        CurrentActiveScene = Instantiate(Scenes[sceneName].gameObject, ActiveSceneParent).GetComponent<Scene>();

        CurrentActiveScene.OnSceneLoad();

        Debug.Log(sceneName + " Scene Loading Successed.");
    }
}
