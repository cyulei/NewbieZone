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

    public SceneLevel sceneLevel;

    GameObject gameplayUI;
    GameObject ContinueUI;
    bool isMenuOn = false;
    // Start is called before the first frame update
    void Start()
    {
        Director.GetInstance().SceneChange += SceneChange;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 监听场景变换
    public void SceneChange(SceneLevel level)
    {
        sceneLevel = level;
        if(sceneLevel == SceneLevel.StartScene)
        {

        }
        else if(sceneLevel == SceneLevel.GameScene)
        {
            gameplayUI = GameObject.Find(GameplayUIName);
            ContinueUI = GameObject.Find(ContinueUIName);
            ContinueUI.SetActive(false);
            gameplayUI.SetActive(true);
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
                Time.timeScale = 0; 
                ContinueUI.SetActive(true);
                gameplayUI.SetActive(false);
                Button continueButton = ContinueUI.transform.Find(ContinueUIContinueButtonName).GetComponent<Button>();
                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(ContinueButtonClick);
                Button returnStartButton = ContinueUI.transform.Find(ContinueUIReturnStartName).GetComponent<Button>();
                returnStartButton.onClick.RemoveAllListeners();
                returnStartButton.onClick.AddListener(ReturnStartButtonClick);
                isMenuOn = false;
            }
            else
            {
                ContinueUI.SetActive(false);
                gameplayUI.SetActive(true);
            }
        }
    }

    void ContinueButtonClick()
    {
        Time.timeScale = 1;
        ContinueUI.SetActive(false);
        gameplayUI.SetActive(true);
    }

    void ReturnStartButtonClick()
    {
        Time.timeScale = 1;
        Debug.Log("返回开始界面");
    }
}
