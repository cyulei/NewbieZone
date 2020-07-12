using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [Header("音频")]
    public AudioPlayer footAudioPlayer;
    public AudioClip jumpAudioClip;
    public AudioClip landAudioClip;

    [Header("玩家属性")]
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
    public bool isJumping = false;   // 正在跳跃
    Vector3 originVelocity;   // 初始的竖直高度

    public Slider slider;

    [HideInInspector]
    public float weaponMoveSpeed = 0f;
    void Start()
    {
        director = Director.GetInstance();
        director.CurrentPlayerController = this;

        // 获取玩家身上的摄像机
        playerCamera = player.transform.Find("Main Camera").gameObject;

        originVelocity = player.GetComponent<Rigidbody>().velocity;
        //Debug.Log("玩家初始originVelocity:" + originVelocity);
        // 初始化玩家血量
        Health health = player.GetComponent<Health>();
        health.MyHealthChange += PlayerHealthChange;
        health.NeedToDeath += PlayerDeath;
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

        if (originVelocity == player.GetComponent<Rigidbody>().velocity && isJumping)
        {
            footAudioPlayer.PlayClip(landAudioClip, 0.8f, 1.1f);
            isJumping = false;
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

        if (translationZ < 0.01f && translationX < 0.01f)
            weaponMoveSpeed = 0f;
        else
            weaponMoveSpeed = Mathf.Max(translationZ * playerMoveSpeed * Time.deltaTime, translationX * playerMoveSpeed * Time.deltaTime);
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
        director.CurrentWeaponsManager.currentWeapon.Fire(shootingPosition.transform);
    }

    public void PlayerJump()
    {
        //Debug.Log("velocity:" + player.GetComponent<Rigidbody>().velocity);
        if(originVelocity == player.GetComponent<Rigidbody>().velocity)
        {
            isJumping = false;
        }
        if(!isJumping)
        {
            footAudioPlayer.PlayClip(jumpAudioClip, 0.8f, 1.1f);
            player.GetComponent<Rigidbody>().velocity += new Vector3(0, 5, 0);
            player.GetComponent<Rigidbody>().AddForce(Vector3.up * playerJumpSpeed);
            isJumping = true;
        }

    }

    public void PlayerHealthChange(int health)
    {
        slider.value = health;
    }

    public void PlayerDeath()
    {
        //Debug.Log("游戏结束");
        director.CurrentSceneController.GotoEndScene(false);
    }

    public void PlayFootstep()
    {
        footAudioPlayer.PlayRandom();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Environment" && isJumping)
        {
            footAudioPlayer.PlayClip(landAudioClip, 0.8f, 1.1f);
            isJumping = false;
        }
    }
}
