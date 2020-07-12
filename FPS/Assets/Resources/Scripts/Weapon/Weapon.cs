using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState
{
    Idle,
    Firing,
    Reloading,
    Selected
}

public class Weapon : MonoBehaviour
{
    [Header("Animation Clips")]
    public AnimationClip FireAnimationClip;
    public AnimationClip ReloadAnimationClip;

    [Header("Audio Clips")]
    public AudioClip FireAudioClip;
    public AudioClip ReloadAudioClip;

    Animator m_Animator;
    WeaponState m_CurrentState;

    AudioSource m_Source;

    int fireNameHash = Animator.StringToHash("fire");
    int reloadNameHash = Animator.StringToHash("reload");

    public float fireRate = 0.5f;
    public float reloadTime = 2.0f;

    float m_ShotTimer = -1.0f;
    float m_ReloadTimer = -1.0f;

    public PlayerController player;

    public int totalBulletNumber;
    public int singleBulletClipNumer;
    public int currentBulletClipNumer = 0;

    WeaponsManager weaponsManager;
    private void Awake()
    {
       
        m_Animator = GetComponentInChildren<Animator>();
        m_Source = GetComponentInChildren<AudioSource>();
        m_CurrentState = WeaponState.Idle;

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
        if (m_CurrentState != WeaponState.Idle || m_ShotTimer > 0 || m_ReloadTimer > 0)
        {
          //  Debug.Log("不能开火:" + m_ShotTimer + "state:" + m_CurrentState);
            return;
        }

        if (currentBulletClipNumer - 1 < 0)
            return;

        m_ShotTimer = fireRate;

        m_CurrentState = WeaponState.Firing;

        m_Animator.SetTrigger("fire");
        m_Animator.SetBool("idle", false);
        //Debug.Log("开火");
        m_Source.pitch = Random.Range(0.7f, 1.0f);
        m_Source.PlayOneShot(FireAudioClip);

        // 相机震动

        // 发射子弹
        CameraShaker.Instance.Shake(0.2f, 0.05f * 0.6f);
        GameObject bullet = Director.GetInstance().CurrentBulletFactory.GetBullet(firePosition, BulletOwner.Player, Director.GetInstance().CurrentWeaponsManager.CurrentWeaponType);
        currentBulletClipNumer--;
        weaponsManager.ChangeWeaponBulletClip(currentBulletClipNumer, singleBulletClipNumer);
        //m_CurrentState = WeaponState.Idle;

    }

    private void Update()
    {
        UpdateControllerState();
        if (m_ShotTimer > 0)
            m_ShotTimer -= Time.deltaTime;
        if (m_ReloadTimer > 0)
            m_ReloadTimer -= Time.deltaTime;
    }

    void UpdateControllerState()
    {
        m_Animator.SetFloat("speed", player.weaponMoveSpeed);        // 移动速度
    }

    public void Reload()
    {
        if (m_CurrentState != WeaponState.Idle || m_ReloadTimer > 0)
            return;

        //int remainingBullet = 100;//= m_Owner.GetAmmo(ammoType);

        if (totalBulletNumber == 0 && currentBulletClipNumer == 0)
        {
            //No more bullet, so we disable the gun so it's not displayed anymore and change weapon
            gameObject.SetActive(false);
            return;
        }

        m_ReloadTimer = reloadTime;

        if (ReloadAudioClip != null)
        {
            m_Source.pitch = Random.Range(0.7f, 1.0f);
            m_Source.PlayOneShot(ReloadAudioClip);
        }

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
        weaponsManager.ChangeWeaponBulletClip(currentBulletClipNumer, singleBulletClipNumer);

        //Debug.Log("换单");
        m_CurrentState = WeaponState.Reloading;
        m_Animator.SetTrigger("reload");
        m_Animator.SetBool("idle", false);
        // 换弹
    }

    public void Selected()
    {
        if (weaponsManager == null)
            weaponsManager = Director.GetInstance().CurrentWeaponsManager;
        weaponsManager.ChangeWeaponBulletClip(currentBulletClipNumer, singleBulletClipNumer);
        if (totalBulletNumber == 0 && currentBulletClipNumer == 0)
        {
            //No more bullet, so we disable the gun so it's not displayed anymore and change weapon
            gameObject.SetActive(false);
            return;
        }

        m_CurrentState = WeaponState.Selected;
        m_Animator.SetBool("idle", false);
       


        m_Animator.SetTrigger("selected");
    }

    public void ResetWeapon()
    {
        m_Animator.WriteDefaultValues();
    }

    public void ChangeState(WeaponState newState)
    {
        if(newState == WeaponState.Idle)
        {
            m_Animator.SetBool("idle", true);
            //Debug.Log("默认状态");
        }
        if (newState != m_CurrentState)
        {
            var oldState = m_CurrentState;
            m_CurrentState = newState;

            if (oldState == WeaponState.Firing)
            {//we just finished firing, so check if we need to auto reload
           //     if (m_ClipContent == 0)
              //      Reload();
                if(currentBulletClipNumer == 0)
                {
                    Reload();
                }
            }
        }
    }
}
