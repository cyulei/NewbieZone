using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [Header("血量的偏移")]
    public float xOffset = 2.5f;
    public float yOffset = 2.5f;

    private Slider slider;
    private Vector2 monster2DPosition;

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        // 生成血条
        Health myhealth = gameObject.AddComponent<Health>();

        monster2DPosition = Camera.main.WorldToScreenPoint(transform.position);
        GameObject haemalStrand = Instantiate(Resources.Load<GameObject>("Prefabs/MonsterHaemalStrand"), monster2DPosition, Quaternion.identity) as GameObject;
        if (haemalStrand == null) Debug.LogError("初始化血条错误");
        slider = haemalStrand.transform.Find("Slider").GetComponent<Slider>();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
       // CalculateHaemalStrandPosition();
    }

    private void CalculateHaemalStrandPosition()
    {
        monster2DPosition = Camera.main.WorldToScreenPoint(transform.position);
        slider.transform.position = monster2DPosition + new Vector2(xOffset, yOffset);

        //计算怪物和主相机的距离
        float Distance = Vector3.Distance(transform.position, mainCamera.gameObject.transform.position);
        //Debug.Log("Distance:" + Distance);
        float temScale = Distance * 0.07f;

        Debug.Log("Distance:" + Distance + " temScale:" + temScale);
        //置新大小
        slider.transform.localScale = new Vector3(temScale, temScale, temScale);
    }
}
