using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private PlayerController player;

    [Tooltip("鼠标移动敏感度")]
    public float sensitivityMouseX = 10.0f;
    public float sensitivityMouseY = 10.0f;

    [Tooltip("鼠标是否锁定")]
    public bool lockCursor = true;
    private bool isCursorLocked = true;
    [Tooltip("上下最大视角（Y视角）")]
    public float minmumY = -60f;
    public float maxmunY = 60f;

    float rotationY = 0f;

    private void Start()
    {
        player = Director.GetInstance().CurrentPlayerController;
        Director.GetInstance().CurrentUserInput = this;
      //  Debug.Log("UserInput初始化");
    }
    private void Update()
    {
        //获取方向键的偏移量
        float translationX = Input.GetAxis("Horizontal");
        float translationZ = Input.GetAxis("Vertical");
        //移动玩家
        player.MovePlayer(translationX, translationZ);

        //获得相机上下左右旋转的角度
        float rotationX = transform.eulerAngles.y + Input.GetAxis("Mouse X") * sensitivityMouseX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityMouseY;
        //角度限制
        rotationY = Clamp(rotationY, maxmunY, minmumY);
        player.RotationPlayerView(rotationX,rotationY);

        UpdateCursorLock();

        if(Input.GetMouseButtonDown(0))
        {
            player.LaunchBullet();
        }

        if(Input.GetButtonDown("Jump"))
        {
            player.PlayerJump();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Director.GetInstance().CurrentWeaponsManager.ChangeWeaponTypeByKeyborad(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Director.GetInstance().CurrentWeaponsManager.ChangeWeaponTypeByKeyborad(2);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Director.GetInstance().CurrentWeaponsManager.ChangeWeaponTypeByKeyborad(3);
        }

        Director.GetInstance().CurrentWeaponsManager.ChangeWeaponTypeByMouse(Input.GetAxis("Mouse ScrollWheel"));

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject uiController = GameObject.Find("UIController");
            uiController.GetComponent<UIController>().ESCButtonDown();
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("当前场景为游戏场景");
            Director.GetInstance().SceneChangeNow(SceneLevel.GameScene);
        }
    }

    public float Clamp(float value, float max, float min)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isCursorLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isCursorLocked = true;
        }

        if (isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
