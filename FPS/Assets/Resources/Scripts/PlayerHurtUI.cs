using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHurtUI : MonoBehaviour
{
    public Image hurtImage;
    public Color hurtColor;
    public float flashSpeed = 5;
    private bool hurted = false;

    void Update()
    {
        PlayHurtedEffect();
    }

    /// <summary>
    /// 玩家受伤后的屏幕效果
    /// </summary>
    void PlayHurtedEffect()
    {
        if (hurted)
        {
            hurtImage.color = hurtColor;
        }
        else
        {
            hurtImage.color = Color.Lerp(hurtImage.color, Color.clear, flashSpeed * Time.deltaTime);

        }
        hurted = false;
    }

    public void MonsterAttack()
    {
        hurted = true;
    }

}
