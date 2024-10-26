using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSelectNode", menuName = "GameFlowNodes/ItemSelectNode")]
public class SelectItemNode : ActionNode
{
    public override void Reset()
    {
        started = false;
    }
    protected override void OnStart()
    {
        LevelManager.instance.levelState = LevelState.award;
        ItemManager.instance.GenerateAwardItem(ItemSelectType.pick);
        UIManager.instance.ResetData();
        ItemManager.instance.allowSelect = false;
        ItemManager.instance.isSelected = false;
        ItemManager.instance.selectAwardItem = false;
    }

    protected override State OnUpdate()
    {
        if (ItemManager.instance.selectAwardItem)
        {
            return State.Success;
        }
        else if(ItemManager.instance.isSelected)
        {
            UIManager.instance.HideAwardPanel();
            return State.Running;
        }
        else
        {
            UIManager.instance.ShowAwardPanel();
            return State.Running;
        }
    }

    protected override void OnStop()
    {
        ItemManager.instance.UpdateItemPool();
        ItemManager.instance.mouseOnItem = null;
    }
}
