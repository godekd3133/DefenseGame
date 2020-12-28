using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneUI : SceneUI
{
    [SerializeField] Button _startButton;

    public override void OnSceneLoad()
    {
        base.OnSceneLoad();

        _startButton.onClick.AddListener(() => {
            SceneManager.instance.ChangeScene("BattleScene");
        });
    }

    public override void OnSceneUnload()
    {
        base.OnSceneUnload();
    }

    public override void OnSceneUpdate()
    {
        base.OnSceneUpdate();
    }

    public override void OnSceneLateUpdate()
    {
        base.OnSceneLateUpdate();
    }

    public override void OnSceneFixedUpdate()
    {
        base.OnSceneFixedUpdate();
    }
}
