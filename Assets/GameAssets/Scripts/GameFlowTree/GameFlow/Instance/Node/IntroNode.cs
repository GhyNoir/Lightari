using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroNode : ActionNode
{
    public GameBehaviorTree gameBehaviorTree;
    public Node thisNode;
    public bool isDone = false;
    public Vector2 moveVec;
    public override void Reset()
    {
        started = false;
    }
    protected override void OnStart()
    {
        isDone = false;
    }

    protected override State OnUpdate()
    {
        IntroTrans(moveVec);

        if (isDone)
            return State.Success;
        else
            return State.Running;
    }

    protected override void OnStop()
    {
        //gameBehaviorTree.DeleteNode(thisNode);
    }

    public void IntroTrans(Vector2 moveVec)
    {
        /*
        for (int i = 0; i < ChaseManager.instance.galaxys.galaxyLists.Count; i++)
        {
            for (int j = 0; j < ChaseManager.instance.galaxys.galaxyLists[i].galaxyDatas.Count; j++)
            {
                ChaseManager.instance.galaxys.galaxyLists[i].galaxyDatas[j].galaxyPrefeb.transform.position = Vector2.Lerp(ChaseManager.instance.galaxys.galaxyLists[i].galaxyDatas[j].galaxyPrefeb.transform.position,
                ChaseManager.instance.galaxys.galaxyLists[i].galaxyDatas[j].position + moveVec, 0.04f);
                for (int k = 0; k < ChaseManager.instance.galaxys.galaxyLists[i].galaxyDatas[j].trails.Count; k++)
                {
                    ChaseManager.instance.galaxys.galaxyLists[i].galaxyDatas[j].trails[k].SetPosition(0, ChaseManager.instance.galaxys.galaxyLists[i].galaxyDatas[j].neighbourNode[k].galaxyPrefeb.transform.position);
                    ChaseManager.instance.galaxys.galaxyLists[i].galaxyDatas[j].trails[k].SetPosition(1, ChaseManager.instance.galaxys.galaxyLists[i].galaxyDatas[j].galaxyPrefeb.transform.position);
                }
            }
        }

        if (Mathf.Abs(ChaseManager.instance.currentGalaxy.transform.position.magnitude) <= 0.01f)
        {
            for (int i = 0; i < ChaseManager.instance.galaxys.galaxyLists.Count; i++)
            {
                for (int j = 0; j < ChaseManager.instance.galaxys.galaxyLists[i].galaxyDatas.Count; j++)
                    ChaseManager.instance.galaxys.galaxyLists[i].galaxyDatas[j].position = ChaseManager.instance.galaxys.galaxyLists[i].galaxyDatas[j].galaxyPrefeb.transform.position;
            }

            UIControllor.instance.hideBattleUI.ResetData();
            isDone = true;
        }
        */
    }
}
