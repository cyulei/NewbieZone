using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private SceneLevel _currentSceneLevel;    // 当前所处场景
    public SceneLevel CurrentSceneLevel { get { return _currentSceneLevel; } }

    private void Start()
    {
        Director.GetInstance().CurrentSceneController = this;
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
        //Debug.Log("开始游戏");
        SceneManager.LoadScene(1);
        StartCoroutine(Delay(SceneLevel.GameScene));
    }

    /// <summary>
    /// 去开始游戏场景
    /// </summary>
    public void GotoStartScene()
    {
        SceneManager.LoadScene(0);
        StartCoroutine(Delay(SceneLevel.StartScene));
    }

    /// <summary>
    /// 用于延迟发送场景改变通知 使得场景先初始化
    /// </summary>
    /// <param name="sceneLevel">当前场景</param>
    /// <returns></returns>
    IEnumerator Delay(SceneLevel sceneLevel)
    {
        yield return new WaitForSeconds(0.07f);
        SceneChangeNow(sceneLevel);
    }
}
