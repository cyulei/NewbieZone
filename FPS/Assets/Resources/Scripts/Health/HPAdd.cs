using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HPAdd : MonoBehaviour
{
    [Header("道具拾取会显示的UI")]
    public Text addText;
    public void ShowUI(int AddBloodNumber)
    {
        // 需要改变UI
        addText.text = "+" + AddBloodNumber;

        addText.transform.DOScale(1f, 0.5f);
        StartCoroutine("HideAddBulletUI");
    }

    IEnumerator HideAddBulletUI()
    {
        yield return new WaitForSeconds(1.5f);
        addText.transform.DOScale(0f, 0.5f);
    }
}
