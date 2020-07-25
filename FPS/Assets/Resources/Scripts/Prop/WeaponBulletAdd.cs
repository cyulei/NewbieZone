using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class WeaponBulletAdd : MonoBehaviour
{
    [Header("道具拾取会显示的UI")]
    public Image addImage;
    public Text addText;

    public void ShowUI(int index, int addBulletNumer)
    {
        // 需要改变UI
        addImage.sprite = Director.GetInstance().CurrentWeaponsManager.bulletSprites[index];
        addText.text = "+" + addBulletNumer;

        addImage.transform.DOScale(0.8f, 0.5f);
        StartCoroutine("HideAddBulletUI");
    }

    IEnumerator HideAddBulletUI()
    {
        yield return new WaitForSeconds(1.5f);
        addImage.transform.DOScale(0f, 0.5f);
    }
}
