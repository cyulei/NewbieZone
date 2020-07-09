using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private SceneLevel _currentSceneLevel;    // 当前所处场景
    public SceneLevel CurrentSceneLevel { get { return _currentSceneLevel; } }

    [Header("场景名称")]
    public string STARTSCENE = "StartScene";
    public string PLAYSCENE = "SampleScene";

    private void Start()
    {
        Director.GetInstance().CurrentSceneController = this;
        SceneManager.sceneLoaded += SceneChangeCallback;
    }

    // 场景改变委托和事件
    public delegate void ChangeSceneEvent(SceneLevel level);
    public event ChangeSceneEvent SceneChange;

    /// <summary>
    /// 改变场景
    /// </summary>
    /// <param name="level">当前场景</param>
    public void SceneChangeNow(SceneLevel level)
    { 
        _currentSceneLevel = level;
        // 发送场景改变通知
        SceneChange?.Invoke(level);
    }

    /// <summary>
    /// 去游戏play场景
    /// </summary>
    public void GotoPlayGameScene()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// 场景改变的回调函数
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void SceneChangeCallback(Scene scene,LoadSceneMode mode)
    {
       //Debug.Log(scene.name + "is load complete");
        if(scene.name == STARTSCENE)
        {
            SceneChangeNow(SceneLevel.StartScene);
        }
        else if(scene.name == PLAYSCENE)
        {
            SceneChangeNow(SceneLevel.GameScene);
        }
    }
    /// <summary>
    /// 去开始游戏场景
    /// </summary>
    public void GotoStartScene()
    {
        SceneManager.LoadScene(0);
    }
}
