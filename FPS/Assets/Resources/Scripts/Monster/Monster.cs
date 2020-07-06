using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [Header("血量的偏移")]
    public float xOffset = 0f;
    public float yOffset = 0.7f;
    public float zOffset = 0f;

    private Slider slider;

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        // 生成血条
        Health myhealth = gameObject.AddComponent<Health>();

        GameObject haemalStrand = Instantiate(Resources.Load<GameObject>("Prefabs/MonsterHaemalStrand"), transform.position, Quaternion.identity) as GameObject;
        if (haemalStrand == null) Debug.LogError("初始化血条错误");
        haemalStrand.transform.SetParent(transform);

        haemalStrand.GetComponent<RectTransform>().position += new Vector3(xOffset, yOffset, zOffset);

        slider = haemalStrand.transform.Find("Slider").GetComponent<Slider>();

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
}
