using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : BTTree
{
    private static string DESTINATION = "Destination";
    private static string PLAYERLOCATION = "Location";
    private static string PLAYER_NAME = "Player";

    [Tooltip("怪物的移动速度")]
    public float moveSpeed;

    protected override void Init()
    {
        base.Init();
        root = new BTPrioritySelector();   // 根节点首先是一个选择器

        // 转移条件
        MonsterCheckPlayerInRange playerInRange = new MonsterCheckPlayerInRange(5f, PLAYER_NAME);
        MonsterCheckPlayerInRange playerNotInRange = new MonsterCheckPlayerInRange(5f, PLAYER_NAME,true);

        // 行为节点
        MonsterMove move = new MonsterMove(DESTINATION, moveSpeed);
        MonsterFindToTargetDestination findToTargetDestination = new MonsterFindToTargetDestination(PLAYER_NAME, PLAYERLOCATION,DESTINATION, 5f);
        MonsterWait monsterWait = new MonsterWait();
        MonsterLongDistanceAttacks monsterMeleeAttack = new MonsterLongDistanceAttacks(10);
        MonsterRandomMoveDistance randomMoveDistance = new MonsterRandomMoveDistance(DESTINATION, 10, 10);
        MonsterRotateToTarget monsterMoveRotate = new MonsterRotateToTarget(DESTINATION);
        MonsterRotateToTarget attackRotate = new MonsterRotateToTarget(PLAYERLOCATION);

        // 攻击
        BTSequence attack = new BTSequence(playerInRange);
        {
            BTParallel parallel = new BTParallel(BTParallel.ParallelFunction.Or);
            {
                parallel.AddChild(findToTargetDestination);    // 先找到走到攻击目标的目的地
                parallel.AddChild(monsterMoveRotate);          // 怪物转向目的地
                parallel.AddChild(move);                       // 进行移动
            }
            attack.AddChild(parallel);
            attack.AddChild(attackRotate);                     // 怪物朝向玩家
            attack.AddChild(monsterMeleeAttack);               // 进行攻击
        }
        root.AddChild(attack);

        // 随机巡逻
        BTSequence randomMove = new BTSequence(playerNotInRange);
        {
            randomMove.AddChild(monsterWait);                  // 怪物静止几秒

            BTParallel parallel = new BTParallel(BTParallel.ParallelFunction.And);
            {
                parallel.AddChild(randomMoveDistance);         // 随机找一个移动地点
                parallel.AddChild(monsterMoveRotate);          // 转向目的地
                parallel.AddChild(move);                       // 进行移动
            }
            randomMove.AddChild(parallel);                                 
        } 
        root.AddChild(randomMove);
    }
}
