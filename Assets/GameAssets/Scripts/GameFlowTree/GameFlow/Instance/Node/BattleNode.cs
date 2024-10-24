using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleNode", menuName = "GameFlowNodes/BattleNode")]
public class BattleNode : CompositeNode
{
    public string nodeName;

    public override void Reset()
    {
        started = false;
        for(int i = 0; i < children.Count; i++)
        {
            children[i].Reset();
        }
    }
    protected override void OnStart()
    {
        for (int i = 0; i < children.Count; i++)
        {
            children[i].Reset();
        }
        /*
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
        */
    }

    protected override State OnUpdate()
    {
        for(int i = 0; i < children.Count; i++)
        {
            children[i].Update();
        }

        for(int i = 0; i < children.Count; i++)
        {
            if (children[i].state == State.Success)
            {
                return State.Success;
            }
        }

        return State.Running;
    }

    protected override void OnStop()
    {
        LevelManager.instance.levelPause = true;

        //战斗后增加光能
        ChaseCellManager.instance.playerPhotonNum += (3 + ChaseCellManager.instance.lightedHouseNum);
        UIManager.instance.UpdatePhotonNumber();

        EnemyManager.instance.ResetScene();
        PlayerControllor.instance.rig.velocity = Vector2.zero;

        //战斗结束后更新关卡信息
        ChaseCellManager.instance.GetCell(new Vector2(0,0)).transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = ChaseCellManager.instance.cellIcons[2];
        ChaseCellManager.instance.GetCell(new Vector2(0, 0)).GetComponent<ChaseCellControllor>().hasCreature = true;
        ChaseCellManager.instance.GetCell(new Vector2(0, 0)).GetComponent<ChaseCellControllor>().hasBattle = false;
    }
}
