using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleNode", menuName = "GameFlowNodes/BattleNode")]
public class BattleNode : ActionNode
{
    public string nodeName;

    public override void Reset()
    {
        started = false;
    }
    protected override void OnStart()
    {
        LevelManager.instance.levelState = LevelState.battle;

        LevelManager.instance.startExp = LevelManager.instance.requiredExp;
        LevelManager.instance.requiredExp += 10;
        LevelManager.instance.targetExp = LevelManager.instance.startExp;
        LevelManager.instance.currentExp = LevelManager.instance.startExp;

        LevelManager.instance.startHealth = LevelManager.instance.requiredHealth;
        LevelManager.instance.requiredHealth += 10;
        LevelManager.instance.targetHealth = LevelManager.instance.startHealth;
        LevelManager.instance.currentHealth = LevelManager.instance.startHealth;

        BackgroundManager.instance.UpdateExpRadius();
        BackgroundManager.instance.UpdateHealthRadius();

        EnemyManager.instance.allowSpawn = true;
        LevelManager.instance.currentLevel += 1;

        LevelManager.instance.levelPause = false;
    }

    protected override State OnUpdate()
    {

        EnemyManager.instance.GenerateEnemy(LevelManager.instance.currentLevel);
        SupplyManager.instance.GenerateSupply(LevelManager.instance.currentLevel);


        //监测exp数值
        if (Mathf.Abs(LevelManager.instance.currentExp - LevelManager.instance.requiredExp) <= 0.05f)
        {
            return State.Success;
        }
        if (Mathf.Abs(LevelManager.instance.currentExp - LevelManager.instance.targetExp) >= 0.01f)
        {
            LevelManager.instance.currentExp = Mathf.Lerp(LevelManager.instance.currentExp, LevelManager.instance.targetExp, 0.1f);
        }

        //监测玩家生命
        if(Mathf.Abs(LevelManager.instance.currentHealth - LevelManager.instance.requiredHealth) <= 0.05f)
        {
            //游戏结束
            return State.Running;
        }
        if (Mathf.Abs(LevelManager.instance.currentHealth - LevelManager.instance.targetHealth) >= 0.01f)
        {
            LevelManager.instance.currentHealth = Mathf.Lerp(LevelManager.instance.currentHealth, LevelManager.instance.targetHealth, 0.1f);
        }
        return State.Running; 
    }

    protected override void OnStop()
    {
        LevelManager.instance.levelPause = true;

        EnemyManager.instance.ResetScene();
        PlayerControllor.instance.rig.velocity = Vector2.zero;

        //生成下一关内容结点

    }
}
