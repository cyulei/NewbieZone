using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseWeapons : MonoBehaviour
{
    [Tooltip("选择武器面板可容纳的武器容器")]
    public List<GameObject> WeaponToggle;

    [Tooltip("武器显示第一个位置")]
    public Transform endPos;     
    [Tooltip("武器隐藏的位置")]
    public Transform startPos;   

    bool isShowWeaponToggle = false;       // 是否展示武器选择界面

    void Start()
    {
        // 监听Q键按下事件
        UserInput userInput =  Director.GetInstance().CurrentUserInput;
       // Debug.Log("ChooseWeapons初始化");
        if (!userInput) Debug.LogError("UserInput还没有初始化");
        userInput.QButtonDown += ChangeWeaponToggleShow;
    }

    public void ChangeWeaponToggleShow()
    {
      //  Debug.Log("移动");
        if (!isShowWeaponToggle)
        {

            for(int i = 0;i < WeaponToggle.Count; i++)
            {
                Vector3 pos = new Vector3(endPos.position.x, endPos.position.y - i * 90, endPos.position.z);
                WeaponToggle[i].transform.DOMove(pos, 0.5f);
            }
            isShowWeaponToggle = true;
        }
        else
        {
            for (int i = 0; i < WeaponToggle.Count; i++)
            {
                WeaponToggle[i].transform.DOMove(startPos.position, 0.5f);
            }
            isShowWeaponToggle = false;
        }
    }
}
