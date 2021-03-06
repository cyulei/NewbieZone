﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [Header("血条显示的偏移值")]
    public float xOffset = 0f;
    public float yOffset = 0.7f;
    public float zOffset = 0f;

    private Slider slider;           // 血条滑动条

    [Header("怪物音频")]
    public AudioPlayer audioPlayer;  // 音频播放器
    public AudioSource idleSource;   // 怪物周围的声音

    [Header("怪兽血条")]
    public GameObject haemalStrand;
    public bool isBoss;
    // Start is called before the first frame update
    void Start()
    {
        Health myhealth = gameObject.GetComponent<Health>();
        if (myhealth == null)
        {
            Debug.LogError("血量组件未初始化");
        }

        if(haemalStrand == null)
        {
            // 生成血条
            haemalStrand = Instantiate(Resources.Load<GameObject>("Prefabs/MonsterHaemalStrand"), transform.position, Quaternion.identity) as GameObject;
            if (haemalStrand == null) Debug.LogError("初始化血条错误");
            // 将血条设置为子节点设置位置偏移
            haemalStrand.transform.SetParent(transform);
            haemalStrand.GetComponent<RectTransform>().position += new Vector3(xOffset, yOffset, zOffset);
        }

        // 初始化血条属性
        slider = haemalStrand.transform.Find("Slider").GetComponent<Slider>();
        slider.minValue = myhealth.minHealth;
        slider.maxValue = myhealth.maxHealth;
        slider.value = myhealth.maxHealth;

        // 健康值组件的事件监听
        myhealth.MyHealthChange += MonsterHealthChange;   // 血量改变
        myhealth.NeedToDeath += MonsterDeath;             // 血量为最小值

        if (idleSource != null)
            idleSource.time = Random.Range(0.0f, idleSource.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBoss)
        {
            UpdateHaemalStrand();
        }
    }

    /// <summary>
    /// 血条一直看向相机
    /// </summary>
    private void UpdateHaemalStrand()
    {
        slider.gameObject.transform.LookAt(Camera.main.transform.position);
    }

    /// <summary>
    /// 怪兽血量改变
    /// </summary>
    /// <param name="health">当前健康值</param>
    public void MonsterHealthChange(int health)
    {
        // 改变血量
        slider.value = health;

        // boss在特定血量会做出的动作
        if(gameObject.tag == "Boss")
        {
            if (health <= 280 && health >= 260)
            {
                gameObject.GetComponent<BossAI>().SendCircleBullet(4);
            }
            if (health <= 250 && health >= 220)
            {
                gameObject.GetComponent<BossAI>().SendCircleBullet(6);
            }
            if (health <= 200)
            {
                 gameObject.GetComponent<BossAI>().ShowAllHideMonsters();
            }
            if (health <= 150 && health >= 130)
            {
                gameObject.GetComponent<BossAI>().SendCircleBullet(8);
            }
            if(health <= 70 && health >= 50)
            {
                gameObject.GetComponent<BossAI>().SendCircleBullet(12);
            }
        }

        // 播放被击中音频
        if (audioPlayer != null)
            audioPlayer.PlayRandom();
    }

    /// <summary>
    /// 怪兽血量达到最小值
    /// </summary>
    public void MonsterDeath()
    {
        // 如果是Boss死亡 则获胜
        if(gameObject.tag == "Boss")
        {
            Director.GetInstance().CurrentSceneController.GotoEndScene(true);
        }
        Destroy(this.gameObject);
    }
}

/// <summary>
/// 怪物的类型
/// </summary>
public enum MonsterType
{
    LongDistanceAttack,
    MeleeAttack,
    MixedAttack
}