using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneUI : MonoBehaviour
{
    public SceneUIAdmin ManagedAdmin;

    //씬 로드/언로드 되었을때
    public virtual void OnSceneLoad() { }

    public virtual void OnSceneUnload() { }

    //Update
    public virtual void OnSceneUpdate() { }

    public virtual void OnSceneLateUpdate() { }

    public virtual void OnSceneFixedUpdate() { }
}
