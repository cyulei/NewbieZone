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
    public string ENDSCENE = "EndScene";

    public AudioSource BGMPlayer;
    public AudioClip EndGameSound;
    public AudioClip PlayGameSound;
    public AudioClip StartGameSound;

    private void Start()
    {
        Director.GetInstance().CurrentSceneController = this;
        SceneManager.sceneLoaded += SceneChangeCallback;
    }

    // 场景改变委托和事件
    public delegate void ChangeSceneEvent(SceneLevel level);
    public event ChangeSceneEvent SceneChange;

    public bool isWin;
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
        BGMPlayer.clip = PlayGameSound;
        BGMPlayer.loop = true;
        BGMPlayer.Play();
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
        else  if(scene.name == ENDSCENE)
        {
            SceneChangeNow(SceneLevel.EndScene);
        }
    }
    /// <summary>
    /// 去开始游戏场景
    /// </summary>
    public void GotoStartScene()
    {
        BGMPlayer.clip = StartGameSound;
        BGMPlayer.loop = true;
        BGMPlayer.Play();
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 去结束场景
    /// </summary>
    /// <param name="isWin">是否获胜</param>
    public void GotoEndScene(bool iswin)
    {
        BGMPlayer.clip = EndGameSound;
        BGMPlayer.loop = false;
        BGMPlayer.Play();
        SceneManager.LoadScene(2);
        isWin = iswin;
    }
}
