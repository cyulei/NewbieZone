using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Tooltip("玩家在项目中的物体")]
    public GameObject player = null;
    [Tooltip("玩家发射子弹的位置")]
    public GameObject shootingPosition = null;
    private GameObject playerCamera = null;
    [Tooltip("玩家移动速度")]
    public float playerMoveSpeed = 5f;
    [Tooltip("玩家旋转速度")]
    public float playerRotateSpeed = 5f;
    [Tooltip("玩家跳跃速度")]
    public float playerJumpSpeed = 5f;

    Director director;
    bool isJumping = false;   // 正在跳跃
    Vector3 originVelocity;   // 初始的竖直高度

    public Slider slider;
    void Start()
    {
        director = Director.GetInstance();
        director.CurrentPlayerController = this;

        // 获取玩家身上的摄像机
        playerCamera = player.transform.Find("Main Camera").gameObject;

        originVelocity = player.GetComponent<Rigidbody>().velocity;

        // 初始化玩家血量
        Health health = player.GetComponent<Health>();
        health.MyHealthChange += PlayerHealthChange;
        slider.maxValue = health.maxHealth;
        slider.minValue = health.minHealth;
        slider.value = health.maxHealth;
    }

    void Update()
    {
        // 防止玩家从平台上跌落
        if(player.transform.position.y < -10)
        {
            player.transform.position = new Vector3(0, 15, 0);
        }
    }

    //玩家移动
    public void MovePlayer(float translationX, float translationZ)
    {
        if (player == null)
        {
            Debug.LogError("player为空");
        }
        //移动
        player.transform.Translate(0, 0, translationZ * playerMoveSpeed * Time.deltaTime);
        player.transform.Translate(translationX * playerMoveSpeed * Time.deltaTime, 0, 0);
    }

    public void RotationPlayerView(float rotationX, float rotationY)
    {
        if (player == null)
        {
            Debug.LogError("player为空");
        }
        if (playerCamera == null)
        {
            Debug.LogError("playerCamera为空");
        }
        playerCamera.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        player.transform.Rotate(0, rotationX * playerRotateSpeed * Time.deltaTime, 0);

    }

    public void LaunchBullet()
    {
        if(shootingPosition == null)
        {
            Debug.LogError("shootingPosition未设置射击点");
        }
        GameObject bullet = director.CurrentBulletFactory.GetBullet(shootingPosition.transform,BulletOwner.Player, director.CurrentWeaponsManager.CurrentWeaponType);
    }

    public void PlayerJump()
    {
        if(originVelocity == player.GetComponent<Rigidbody>().velocity)
        {
            isJumping = false;
        }
        if(!isJumping)
        {
            player.GetComponent<Rigidbody>().velocity += new Vector3(0, 5, 0);
            player.GetComponent<Rigidbody>().AddForce(Vector3.up * playerJumpSpeed);
            isJumping = true;
        }

    }

    public void PlayerHealthChange(int health)
    {
        slider.value = health;
    }
}
