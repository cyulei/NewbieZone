using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器的状态
/// </summary>
public enum WeaponState
{
    Idle,
    Firing,
    Reloading,
    Selected
}

public class Weapon : MonoBehaviour
{
    [Header("动画片段")]
    public AnimationClip FireAnimationClip;
    public AnimationClip ReloadAnimationClip;

    [Header("音效片段")]
    public AudioClip FireAudioClip;
    public AudioClip ReloadAudioClip;

    Animator animator;                  // 动画机
    WeaponState currentState;           // 武器当前的状态

    AudioSource source;                 // 音效

    public float shotRate = 0.5f;       // 射击频率
    public float reloadTime = 2.0f;     // 换单频率

    [Tooltip("普通子弹攻击力")]
    public int ATK;                   // 伤害

    float shotTimer = -1.0f;            // 射击计时器   
    float reloadTimer = -1.0f;          // 换单计时器

    [Header("玩家控制器")]
    public PlayerController player;

    [Header("弹夹属性")]
    [Tooltip("子弹总数")]
    public int totalBulletNumber;
    [Tooltip("一个弹夹的最多子弹数量")]
    public int singleBulletClipNumer;
    [HideInInspector]
    public int currentBulletClipNumer = 0;

    WeaponsManager weaponsManager;

    private void Awake()
    {
       
        animator = GetComponentInChildren<Animator>();
        source = GetComponentInChildren<AudioSource>();
        currentState = WeaponState.Idle;

        // 装上子弹
        totalBulletNumber -= singleBulletClipNumer;
        currentBulletClipNumer = singleBulletClipNumer;
    }

    private void Start()
    {
        weaponsManager = Director.GetInstance().CurrentWeaponsManager;
        weaponsManager.ChangeWeaponBulletClip(currentBulletClipNumer, singleBulletClipNumer);
    }

    public void Fire(Transform firePosition)
    {
        if (currentState != WeaponState.Idle || shotTimer > 0 || reloadTimer > 0)
        {
            return;
        }

        // 子弹数量不足不能开火
        if (currentBulletClipNumer - 1 < 0)
            return;

        shotTimer = shotRate;

        currentState = WeaponState.Firing;

        // 播放开火动画
        animator.SetTrigger("fire");
        animator.SetBool("idle", false);
        //Debug.Log("开火");

        // 播放开火音乐
        source.pitch = Random.Range(0.7f, 1.0f);
        source.PlayOneShot(FireAudioClip);

        // 相机震动
        CameraShaker.Instance.Shake(0.2f, 0.05f * 0.6f);

        // 发射子弹
        GameObject bullet = Director.GetInstance().CurrentBulletFactory.GetBullet(firePosition, BulletOwner.Player, Director.GetInstance().CurrentWeaponsManager.CurrentWeaponType);
        if(Director.GetInstance().CurrentWeaponsManager.CurrentWeaponType == WeaponType.Normal)
            bullet.GetComponent<Bullet>().hurt = ATK;
        currentBulletClipNumer--;
        weaponsManager.ChangeWeaponBulletClip(currentBulletClipNumer, singleBulletClipNumer);
    }

    private void Update()
    {
        UpdateControllerState();
        if (shotTimer > 0)
            shotTimer -= Time.deltaTime;
        if (reloadTimer > 0)
            reloadTimer -= Time.deltaTime;
    }

    void UpdateControllerState()
    {
        animator.SetFloat("speed", player.weaponMoveSpeed);        // 移动速度
    }

    public void Reload()
    {
        if (currentState != WeaponState.Idle || reloadTimer > 0)
            return;

        // 没有子弹 隐藏武器
        if (totalBulletNumber == 0 && currentBulletClipNumer == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        reloadTimer = reloadTime;

        // 播放音乐
        if (ReloadAudioClip != null)
        {
            source.pitch = Random.Range(0.7f, 1.0f);
            source.PlayOneShot(ReloadAudioClip);
        }

        // 换子弹
        totalBulletNumber += currentBulletClipNumer;
        int remain = totalBulletNumber - singleBulletClipNumer;
        if(remain >= 0)
        {
            totalBulletNumber -= singleBulletClipNumer;
            currentBulletClipNumer = singleBulletClipNumer;
        }
        else
        {
            currentBulletClipNumer = totalBulletNumber;
            totalBulletNumber = 0;
        }
        // 通知已经换子弹数量
        weaponsManager.ChangeWeaponBulletClip(currentBulletClipNumer, singleBulletClipNumer);

        // 播放动画
        currentState = WeaponState.Reloading;
        animator.SetTrigger("reload");
        animator.SetBool("idle", false);
    }

    /// <summary>
    /// 被选中
    /// </summary>
    public void Selected()
    {
        if (weaponsManager == null)
            weaponsManager = Director.GetInstance().CurrentWeaponsManager;

        // 通知切换武器的子弹数量
        weaponsManager.ChangeWeaponBulletClip(currentBulletClipNumer, singleBulletClipNumer);

        // 没有子弹 隐藏武器
        if (totalBulletNumber == 0 && currentBulletClipNumer == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        // 播放动画
        currentState = WeaponState.Selected;
        animator.SetBool("idle", false);
        animator.SetTrigger("selected");
    }

    /// <summary>
    /// 重置武器动画
    /// </summary>
    public void ResetWeapon()
    {
        animator.WriteDefaultValues();
    }

    /// <summary>
    /// 改变武器状态
    /// </summary>
    /// <param name="newState">新状态</param>
    public void ChangeState(WeaponState newState)
    {
        if(newState == WeaponState.Idle)
        {
            animator.SetBool("idle", true);
            //Debug.Log("默认状态");
        }
        if (newState != currentState)
        {
            var oldState = currentState;
            currentState = newState;

            if (oldState == WeaponState.Firing)
            {
                // 如果开火过后没有子弹 就重新换弹夹
                if(currentBulletClipNumer == 0)
                {
                    Reload();
                }
            }
        }
    }
}
