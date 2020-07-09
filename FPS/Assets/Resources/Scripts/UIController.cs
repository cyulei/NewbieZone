using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SceneLevel
{
    StartScene,
    GameScene,
    EndScene
}
public class UIController : MonoBehaviour
{
    public string ContinueUIName = "Menu";
    public string GameplayUIName = "GameplayUI";
    public string ContinueUIContinueButtonName = "ContinueButton";
    public string ContinueUIReturnStartName = "ReturnStart";
    public string StartUIName = "StartCanvas";
    public string StartUIStartButtonName = "StartButton";

    private SceneLevel sceneLevel;   // 当前所处场景

    GameObject gameplayUI;
    GameObject ContinueUI;
    GameObject StartUI;
    public bool isMenuOn = false;     // 暂停UI是否染出

    public static UIController Instance = null;    // 保证每个场景只有一个

    void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        Instance = this;
        GameObject.DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        Director.GetInstance().CurrentUIController = this;
        // 监听场景改变事件
        Director.GetInstance().CurrentSceneController.SceneChange += SceneChange;
        Director.GetInstance().CurrentSceneController.SceneChangeNow(SceneLevel.StartScene);
    }

    // 监听场景变换
    public void SceneChange(SceneLevel level)
    {
        //Debug.Log("收到通知");
        sceneLevel = level;
        if(sceneLevel == SceneLevel.StartScene)
        {
            StartUI = GameObject.Find(StartUIName);
            // 为按钮增加监听事件
            Button startButton = StartUI.transform.Find(StartUIStartButtonName).GetComponent<Button>();
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(GotoPlayGameScene);
        }
        else if(sceneLevel == SceneLevel.GameScene)
        {
            // 设置UI的显示或隐藏
            gameplayUI = GameObject.Find(GameplayUIName);
            ContinueUI = GameObject.Find(ContinueUIName);
            ContinueUI.SetActive(false);
            gameplayUI.SetActive(true);
            isMenuOn = false;
        }
        else if(sceneLevel == SceneLevel.EndScene)
        {

        }
    }

    // 监听退出按钮按下
    public void ESCButtonDown()
    {
        if(sceneLevel == SceneLevel.GameScene)
        {
            if(!isMenuOn)
            {
                // 暂停游戏
                Time.timeScale = 0; 
                ContinueUI.SetActive(true);
                gameplayUI.SetActive(false);

                // 为按钮增加监听事件
                Button continueButton = ContinueUI.transform.Find(ContinueUIContinueButtonName).GetComponent<Button>();
                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(ContinueButtonClick);
                Button returnStartButton = ContinueUI.transform.Find(ContinueUIReturnStartName).GetComponent<Button>();
                returnStartButton.onClick.RemoveAllListeners();
                returnStartButton.onClick.AddListener(ReturnStartButtonClick);

                isMenuOn = true;
            }
            else
            {
                // 设置UI的显示或隐藏
                ContinueUI.SetActive(false);
                gameplayUI.SetActive(true);
                isMenuOn = false;
                Time.timeScale = 1;
            }
        }
    }

    /// <summary>
    /// 继续游戏按钮点击
    /// </summary>
    void ContinueButtonClick()
    {
        // 恢复游戏时间
        Time.timeScale = 1;
        isMenuOn = false;
        ContinueUI.SetActive(false);
        gameplayUI.SetActive(true);
    }

    /// <summary>
    /// 返回开始游戏界面按钮点击
    /// </summary>
    void ReturnStartButtonClick()
    {
        // 恢复游戏时间
        Time.timeScale = 1;
        Director.GetInstance().CurrentSceneController.GotoStartScene();
    }

    /// <summary>
    /// 去游戏play界面
    /// </summary>
    void GotoPlayGameScene()
    {
        Director.GetInstance().CurrentSceneController.GotoPlayGameScene();
    }
}
