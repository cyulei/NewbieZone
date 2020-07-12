using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : AIForMonster
{
    private static string DESTINATION = "Destination";
    private static string PLAYERLOCATION = "Location";
    private static string PLAYER_NAME = "Player";

    [Header("怪物属性")]
    [Tooltip("怪物检测周围有玩家的半径")]
    public float checkPlayerRange;
    [Tooltip("怪物的近战攻击力")]
    public int meleeATK;
    [Tooltip("怪物的远程子弹攻击力")]
    public int longDistanceATK;
    [Tooltip("怪兽可以进程攻击的距离")]
    public float ATKMeleeDistance;
    [Tooltip("怪兽可以远程攻击的距离")]
    public float ATKLongDistance;

    public List<GameObject> monsters;

    bool isShowHideMonsters = false;
    protected override void Init()
    {
        base.Init();
        root = new BTPrioritySelector();   // 根节点首先是一个选择器

        // 转移条件
        MonsterCheckPlayerInRangeAndRandomAttack longAttack = new MonsterCheckPlayerInRangeAndRandomAttack(checkPlayerRange, PLAYER_NAME, true);
        MonsterCheckPlayerInRangeAndRandomAttack meleeAttack = new MonsterCheckPlayerInRangeAndRandomAttack(checkPlayerRange, PLAYER_NAME, false);
        MonsterCheckPlayerInRange playerNotInRange = new MonsterCheckPlayerInRange(checkPlayerRange, PLAYER_NAME, true);

        // 行为节点
        move = new MonsterMove(DESTINATION, moveSpeed);
        MonsterFindToTargetDestination findMeleeToTargetDestination = new MonsterFindToTargetDestination(PLAYER_NAME, PLAYERLOCATION, DESTINATION, ATKMeleeDistance);
        MonsterFindToTargetDestination findLongToTargetDestination = new MonsterFindToTargetDestination(PLAYER_NAME, PLAYERLOCATION, DESTINATION, ATKLongDistance);
        MonsterWait monsterWait = new MonsterWait();

        MonsterLongDistanceAttacks monsterLongDistanceAttacks = new MonsterLongDistanceAttacks(longDistanceATK);
        MonsterMeleeAttack monsterMeleeAttack = new MonsterMeleeAttack(meleeATK);

        MonsterRotateToTarget monsterMoveRotate = new MonsterRotateToTarget(DESTINATION);
        MonsterRotateToTarget attackRotate = new MonsterRotateToTarget(PLAYERLOCATION);

        // 静止（不在攻击范围内）
        BTSequence idle = new BTSequence(playerNotInRange);
        {
            idle.AddChild(new BTAction());                  // 怪物静止
        }
        root.AddChild(idle);
        // 进程攻击(一个随机数并且玩家在攻击范围内)
        BTSequence meleeattack = new BTSequence(meleeAttack);
        {
            BTParallel parallel = new BTParallel(BTParallel.ParallelFunction.Or);
            {
                parallel.AddChild(findMeleeToTargetDestination);    // 先找到走到攻击目标的目的地
                BTSequence rotateAndMove = new BTSequence();
                {
                    rotateAndMove.AddChild(monsterMoveRotate);
                    rotateAndMove.AddChild(move);
                }
                parallel.AddChild(rotateAndMove);             // 怪物朝着目的地移动
            }
            meleeattack.AddChild(parallel);
            meleeattack.AddChild(attackRotate);                     // 怪物朝向玩家
            meleeattack.AddChild(monsterMeleeAttack);               // 进行攻击
        }
        root.AddChild(meleeattack);

        // 远程攻击
        BTSequence attack = new BTSequence(longAttack);
        {
            BTParallel parallel = new BTParallel(BTParallel.ParallelFunction.Or);
            {
                parallel.AddChild(findLongToTargetDestination);    // 先找到走到攻击目标的目的地
                BTSequence rotateAndMove = new BTSequence();
                {
                    rotateAndMove.AddChild(monsterMoveRotate);
                    rotateAndMove.AddChild(move);
                }
                parallel.AddChild(rotateAndMove);             // 怪物朝着目的地移动
            }
            attack.AddChild(parallel);
            attack.AddChild(attackRotate);                     // 怪物朝向玩家
            attack.AddChild(monsterLongDistanceAttacks);               // 进行攻击
        }
        root.AddChild(attack);
    }

    public void ShowAllHideMonsters()
    {
        if(isShowHideMonsters)
        {
            return;
        }
        foreach(GameObject monster in monsters)
        {
            monster.SetActive(true);
        }
        isShowHideMonsters = true;
    }
}
