using System.Collections;
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

    [Header("Audio")]
    public AudioPlayer HitPlayer;
    public AudioSource IdleSource;
    // Start is called before the first frame update
    void Start()
    {

        Health myhealth = gameObject.GetComponent<Health>();
        if (myhealth == null)
        {
            Debug.LogError("血量组件未初始化");
        }

        // 生成血条
        GameObject haemalStrand = Instantiate(Resources.Load<GameObject>("Prefabs/MonsterHaemalStrand"), transform.position, Quaternion.identity) as GameObject;
        if (haemalStrand == null) Debug.LogError("初始化血条错误");

        // 将血条设置为子节点设置位置偏移
        haemalStrand.transform.SetParent(transform);
        haemalStrand.GetComponent<RectTransform>().position += new Vector3(xOffset, yOffset, zOffset);

        // 初始化血条属性
        slider = haemalStrand.transform.Find("Slider").GetComponent<Slider>();
        slider.minValue = myhealth.minHealth;
        slider.maxValue = myhealth.maxHealth;
        slider.value = myhealth.maxHealth;

        // 健康值组件的事件监听
        myhealth.MyHealthChange += MonsterHealthChange;   // 血量改变
        myhealth.NeedToDeath += MonsterDeath;             // 血量为最小值

        if (IdleSource != null)
            IdleSource.time = Random.Range(0.0f, IdleSource.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHaemalStrand();
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
        slider.value = health;
        if(health <= 150 && gameObject.tag == "Boss")
        {
            gameObject.GetComponent<BossAI>().ShowAllHideMonsters();
        }
        if (HitPlayer != null)
            HitPlayer.PlayRandom();
    }

    /// <summary>
    /// 怪兽血量达到最小值
    /// </summary>
    public void MonsterDeath()
    {
        if(gameObject.tag == "Boss")
        {
            Director.GetInstance().CurrentSceneController.GotoEndScene(true);
        }
        Destroy(this.gameObject);
    }
}

public enum MonsterType
{
    LongDistanceAttack,
    MeleeAttack,
    MixedAttack
}