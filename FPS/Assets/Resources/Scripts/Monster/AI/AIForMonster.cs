using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIForMonster : BTTree
{
    [Header("怪兽的移动属性")]
    protected MonsterMove move;         // 行为移动节点
    [Tooltip("怪物的移动速度")]
    public float moveSpeed;

    /// <summary>
    /// 设置移动速度
    /// </summary>
    /// <param name="speed">设置的速度</param>
    public void ChangeMoveSpeed(float speed)
    {
        move.SetSpeed(speed);
    }
}
