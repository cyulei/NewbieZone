using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : AIForMonster
{
    private static string DESTINATION = "Destination";
    private static string PLAYERLOCATION = "Location";
    private static string PLAYER_NAME = "Player";

    [Header("怪物属性")]
    [Tooltip("怪物类型")]
    public MonsterType myMonsterType;
    [Tooltip("怪物检测周围有玩家的半径")]
    public float checkPlayerRange;
    [Tooltip("怪物的近战攻击力")]
    public int meleeATK;
    [Tooltip("怪物的远程子弹攻击力")]
    public int longDistanceATK;
    [Tooltip("怪兽可以攻击的距离")]
    public float ATKdistance;

    [Header("怪兽移动范围")]
    public float moveX;
    public float moveZ;

    protected override void Init()
    {
        base.Init();
        root = new BTPrioritySelector();   // 根节点首先是一个选择器

        // 转移条件
        MonsterCheckPlayerInRange playerInRange = new MonsterCheckPlayerInRange(checkPlayerRange, PLAYER_NAME);
        MonsterCheckPlayerInRange playerNotInRange = new MonsterCheckPlayerInRange(checkPlayerRange, PLAYER_NAME,true);

        // 行为节点
        move = new MonsterMove(DESTINATION, moveSpeed);
        MonsterFindToTargetDestination findToTargetDestination = new MonsterFindToTargetDestination(PLAYER_NAME, PLAYERLOCATION,DESTINATION, ATKdistance);
        MonsterWait monsterWait = new MonsterWait();

        // 怪兽攻击
        MonsterAttack monsterAttack;
        if (myMonsterType == MonsterType.LongDistanceAttack)
            monsterAttack = new MonsterLongDistanceAttacks(longDistanceATK);
        else if (myMonsterType == MonsterType.MeleeAttack)
            monsterAttack = new MonsterMeleeAttack(meleeATK);
        else
            monsterAttack = new MonsterAttack(meleeATK);     // 先暂时为近战的攻击力

        MonsterRandomMoveDistance randomMoveDistance = new MonsterRandomMoveDistance(DESTINATION, moveX, moveZ);
        MonsterRotateToTarget monsterMoveRotate = new MonsterRotateToTarget(DESTINATION);
        MonsterRotateToTarget attackRotate = new MonsterRotateToTarget(PLAYERLOCATION);

        // 攻击
        BTSequence attack = new BTSequence(playerInRange);
        {
            BTParallel parallel = new BTParallel(BTParallel.ParallelFunction.Or);
            {
                parallel.AddChild(findToTargetDestination);    // 先找到走到攻击目标的目的地
                BTSequence rotateAndMove = new BTSequence();
                {
                    rotateAndMove.AddChild(monsterMoveRotate);
                    rotateAndMove.AddChild(move);
                }
                parallel.AddChild(rotateAndMove);             // 怪物朝着目的地移动
            }
            attack.AddChild(parallel);
            attack.AddChild(attackRotate);                     // 怪物朝向玩家
            attack.AddChild(monsterAttack);               // 进行攻击
        }
        root.AddChild(attack);

        // 随机巡逻
        BTSequence randomMove = new BTSequence(playerNotInRange);
        {
            randomMove.AddChild(monsterWait);                  // 怪物静止几秒

            BTParallel parallel = new BTParallel(BTParallel.ParallelFunction.And);
            {
                parallel.AddChild(randomMoveDistance);         // 随机找一个移动地点
                BTSequence rotateAndMove = new BTSequence();
                {
                    rotateAndMove.AddChild(monsterMoveRotate);
                    rotateAndMove.AddChild(move);
                }
                parallel.AddChild(rotateAndMove);             // 怪物朝着目的地移动
            }
            randomMove.AddChild(parallel);                                 
        } 
        root.AddChild(randomMove);
    }


}
