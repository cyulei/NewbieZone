using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseWeapons : MonoBehaviour
{
    [Tooltip("选择武器面板可容纳的武器容器")]
    public List<GameObject> WeaponToggle = new List<GameObject>();

    public List<Image> toggleImage = new List<Image>();

    [Tooltip("武器显示第一个位置")]
    public Transform endPos;     
    [Tooltip("武器隐藏的位置")]
    public Transform startPos;   

    bool isShowWeaponToggle = false;       // 是否展示武器选择界面

    [Header("选择武器面板动画速度")]
    public float toggleScaleSpeed = 0.5f;
    public float toggleScaleSize = 1.5f;

    void Start()
    {
        WeaponsManager weaponsManager = Director.GetInstance().CurrentWeaponsManager;
        weaponsManager.WeaponTypeChange += WeaponChange;

        if (toggleImage.Count != Director.GetInstance().CurrentWeaponsManager.bulletSprites.Count)
        {
            Debug.LogError("可选的武器UI列表与提供的UI数量不同");
        }
        for(int i = 0; i < toggleImage.Count; i++)
        {
            toggleImage[i].sprite = Director.GetInstance().CurrentWeaponsManager.bulletSprites[i];
        }
    }

    /// <summary>
    /// 改变武器
    /// </summary>
    /// <param name="typeIndex">武器序号</param>
    private void WeaponChange(int typeIndex)
    {
        StopCoroutine("HideToggle");
        ShowToggle();
        for (int i = 0; i < WeaponToggle.Count; i++)
        {
            if(i == typeIndex)
            {
                WeaponToggle[typeIndex].transform.DOScale(new Vector3(toggleScaleSize, toggleScaleSize, toggleScaleSize), toggleScaleSpeed);
            }
            else
            {
                WeaponToggle[i].transform.DOScale(new Vector3(1, 1, 1), toggleScaleSpeed);
            }
        }
        
        StartCoroutine("HideToggle");
    }

    public void ChangeWeaponToggleShow()
    {
      //  Debug.Log("移动");
        if (!isShowWeaponToggle)
        {
            ShowToggle();
        }
        else
        {
            HideToggle();
        }
    }

    /// <summary>
    /// 展示UI
    /// </summary>
    private void ShowToggle()
    {
        for (int i = 0; i < WeaponToggle.Count; i++)
        {
            Vector3 pos = new Vector3(endPos.position.x, endPos.position.y - i * 90, endPos.position.z);
            WeaponToggle[i].transform.DOMove(pos, 0.5f);
        }
        isShowWeaponToggle = true;
    }

    /// <summary>
    /// 隐藏UI
    /// </summary>
    /// <returns></returns>
    IEnumerator HideToggle()
    {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < WeaponToggle.Count; i++)
        {
            WeaponToggle[i].transform.DOMove(startPos.position, 0.5f);
            WeaponToggle[i].transform.DOScale(new Vector3(1, 1, 1), toggleScaleSpeed);
        }
        isShowWeaponToggle = false;
    }
}
