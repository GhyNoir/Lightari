using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OutMazeNode", menuName = "GameFlowNodes/BattleContentNode/OutMazeNode")]
public class OutMaze : ActionNode
{
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
        BackgroundManager.instance.showObstacles = false;

        //生成迷宫
        LevelManager.instance.outMaze = false;
        MazeGenerator.Instance.GenerateMaze();
        MazeGenerator.Instance.GenerateExitPoint();

        PlayerControllor.instance.transform.localScale = new Vector3(1, 1, 1);
        PlayerControllor.instance.allowDash = false;

        EnemyManager.instance.allowSpawn = false;
        LevelManager.instance.currentLevel += 1;

        LevelManager.instance.levelPause = false;
    }

    protected override State OnUpdate()
    {

        if (LevelManager.instance.outMaze)
        {
            return State.Success;
        }
        return State.Running;
    }

    protected override void OnStop()
    {
        LevelManager.instance.levelPause = true;

        MazeGenerator.Instance.ResetMap();
        EnemyManager.instance.ResetScene();
        PlayerControllor.instance.rig.velocity = Vector2.zero;

        //战斗后增加光能
        ChaseCellManager.instance.playerPhotonNum += (3 + ChaseCellManager.instance.lightedHouseNum);
        UIManager.instance.UpdatePhotonNumber();

        //战斗结束后更新关卡信息
        ChaseCellManager.instance.GetCell(new Vector2(0, 0)).transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = ChaseCellManager.instance.cellIcons[2];
        ChaseCellManager.instance.GetCell(new Vector2(0, 0)).GetComponent<ChaseCellControllor>().hasCreature = true;
        ChaseCellManager.instance.GetCell(new Vector2(0, 0)).GetComponent<ChaseCellControllor>().hasBattle = false;
    }
}
