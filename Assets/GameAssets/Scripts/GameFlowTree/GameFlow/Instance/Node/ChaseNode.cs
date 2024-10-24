using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChaseNode", menuName = "GameFlowNodes/ChaseNode")]
public class ChaseDirectionNode : ActionNode
{

    public override void Reset()
    {
        started = false;
    }
    protected override void OnStart()
    {
        UIManager.instance.showChaseDirectionUI = true;
        UIManager.instance.hideChaseDirectionUI = false;
        UIManager.instance.showChaseInteractPanel = false;
        UIManager.instance.hideChaseInteractPanel = true;
        ChaseCellManager.instance.levelChase = true;
        ChaseCellManager.instance.isChased = false;
        ChaseCellManager.instance.allowMove = true;
        //ChaseCellManager.instance.UpdateAllCells(UpdateRule.darken);
        ChaseCellManager.instance.UpdateCellLight(ChaseCellManager.instance.GetCell(new Vector2(0, 0)), 5);
    }

    protected override State OnUpdate()
    {
        //Í¬cell½»»¥
        ChaseCellManager.instance.ChaseCell();

        if (ChaseCellManager.instance.isChased)
        {
            return State.Success;
        }
        else
        {
            return State.Running;

        }

    }

    protected override void OnStop()
    {
        UIManager.instance.showChaseDirectionUI = false;
        UIManager.instance.hideChaseDirectionUI = true;
        UIManager.instance.showChaseInteractPanel = false;
        UIManager.instance.hideChaseInteractPanel = true;
        ChaseCellManager.instance.levelChase = false;
    }
}
