using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    private ReadOnlyDictionary<string, GameObject> _prefabs;
    private ReadOnlyDictionary<string, Sprite> _sprites;
    public bool Loaded = false;

    // 로딩 하나
    class QueuedLoading
    {
        public delegate Task<Object> LoaderFunc();
        public LoaderFunc Load { get; private set; }
        public TaskCompletionSource<Object> resolve;

        // for debug
        string _context;

        // disable new operator
        private QueuedLoading() { }


        public static QueuedLoading Create(TaskCompletionSource<Object> resolve, AssetReference reference, string context)
        {
            QueuedLoading load = new QueuedLoading()
            {
                resolve = resolve,
                _context = context,
            };

            load.Load = async () =>
            {
                if (reference.IsValid())
                    return (Object)reference.OperationHandle.Result;

                if (!reference.RuntimeKeyIsValid())
                {
                    // 지정되지 않은 AssetReference
                    return null;
                }

                try
                {
                    return await reference.LoadAssetAsync<Object>().Task;
                }
                catch (Exception)
                {
                    Debug.LogError($"failed to load, {context}");
                    return null;
                }
            };

            return load;
        }

        public static QueuedLoading Create<T>(TaskCompletionSource<Object> resolve, string path, string context)
            where T : Object
        {
            QueuedLoading load = new QueuedLoading()
            {
                resolve = resolve,
                _context = context,
            };

            load.Load = async () =>
            {
                // check path is valid
                var locations = await Addressables.LoadResourceLocationsAsync(path, typeof(T)).Task;
                if (locations == null || locations.Count != 1)
                    return null;

                return await Addressables.LoadAssetAsync<T>(locations[0]).Task;
            };

            return load;
        }
    }

    private List<QueuedLoading> _loadQueue = new List<QueuedLoading>();

    public ReadOnlyDictionary<string, GameObject> Prefab
    {
        get
        {
            CheckLoaded();
            return _prefabs;
        }
    }

    public ReadOnlyDictionary<string, Sprite> Sprite
    {
        get
        {
            CheckLoaded();
            return _sprites;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpinLoader());
        LoadAll();
    }

    void CheckLoaded()
    {
        Debug.Assert(Loaded);
    }

    Task<T> WrapResult<T>(Task<Object> original) where T : Object => original.Then(_ => (T)original.Result);

    public Task<T> RequestLoad<T>(AssetReference reference, string context)
        where T : Object
    {
        var resolve = new TaskCompletionSource<Object>(TaskCreationOptions.AttachedToParent);

        if (reference.IsValid())
        {
            // use cached
            // can be get from AssetReference.OperationHandle
            resolve.SetResult((T)reference.OperationHandle.Result);
        }
        else if (!reference.RuntimeKeyIsValid())
        {
            resolve.SetResult(null);
        }
        else
        {
            _loadQueue.Add(QueuedLoading.Create(resolve, reference, context));
        }

        return WrapResult<T>(resolve.Task);
    }

    public Task<T> RequestLoad<T>(string path, string context)
        where T : Object
    {
        var resolve = new TaskCompletionSource<Object>(TaskCreationOptions.AttachedToParent);

        _loadQueue.Add(QueuedLoading.Create<T>(resolve, path, context));
        return WrapResult<T>(resolve.Task);
    }

    IEnumerator SpinLoader()
    {
        yield return null;

        while (true)
        {
            if (_loadQueue.Count != 0)
            {
                var currentLoading = _loadQueue;
                _loadQueue = new List<QueuedLoading>();

                Debug.Log($"resource {currentLoading.Count} load start", this);

                List<Task<Object>> tasks = currentLoading.Select(q => q.Load()).ToList();
                var loadTask = Task.WhenAll(tasks);
                while (!loadTask.IsCompleted)
                    yield return null;

                Debug.Log($"load done", this);

                for (int i = 0; i < currentLoading.Count; i++)
                {
                    currentLoading[i].resolve.SetResult(tasks[i].Result);
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    public async Task LoadAll()
    {
        var prefabs = new Dictionary<string, GameObject>();
        var sprites = new Dictionary<string, Sprite>();
        var scenes = new Dictionary<string, Scene>();
        var tasks = new List<Task>();
        Loaded = false;

        async Task LoadPrefab(string _Key, string _Path)
        {
            if (prefabs.ContainsKey(_Key))
                return;

            GameObject prefab = await RequestLoad<GameObject>(_Path, _Path);
            if (prefab == null)
            {
                Debug.LogError("Prefab Load was Failed!\r\nKey : " + _Key + "   Path : " + _Path);
                return;
            }

            prefabs.Add(_Key, prefab);
        }

        async Task LoadSprite(string _Key, string _Path)
        {
            if (sprites.ContainsKey(_Key))
                return;

            Sprite sprite = await RequestLoad<Sprite>(_Path, _Path);
            if (sprite == null)
            {
                Debug.LogError("Sprite Load was Failed!\r\nKey : " + _Key + "   Path : " + _Path);
                return;
            }

            sprites.Add(_Key, sprite);
        }

        async Task LoadScene(string _Key, string _Path)
        {
            if (sprites.ContainsKey(_Key))
                return;

            GameObject scene = await RequestLoad<GameObject>(_Path, _Path);
            if (scene == null)
            {
                Debug.LogError("Scene Load was Failed!\r\nKey : " + _Key + "   Path : " + _Path);
                return;
            }
            scenes.Add(_Key, scene.GetComponent<Scene>());
        }

        //TODO :: 여기서 로딩
        tasks.Add(LoadScene("TitleScene", "Scene/TitleScene.scene"));
        tasks.Add(LoadScene("BattleScene", "Scene/BattleScene.scene"));

        // 실행
        await Task.WhenAll(tasks.ToArray());

        SceneManager.instance.Scenes = new ReadOnlyDictionary<string, Scene>(scenes);
        _prefabs = new ReadOnlyDictionary<string, GameObject>(prefabs);
        _sprites = new ReadOnlyDictionary<string, Sprite>(sprites);
        Loaded = true;

        SceneManager.instance.OnApplicationLoad();
    }
}

public static class TaskUtility
{
    // javascript Promise style
    public static Task<To> Then<From, To>(this Task<From> original, Func<Task<From>, To> convert)
    {
        return original.ContinueWith<To>(convert, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public static Task Then<From>(this Task<From> original, Action<Task<From>> convert)
    {
        return original.ContinueWith(convert, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public static Task<To> Then<To>(this Task original, Func<Task, To> convert)
    {
        return original.ContinueWith<To>(convert, TaskScheduler.FromCurrentSynchronizationContext());
    }
}
