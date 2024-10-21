using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameFlowNode", menuName = "GameFlowNodes/GameFlowNode")]
public class GameFlowNode : CompositeNode
{
    public int current;

    public override void Reset()
    {
        started = false;
        current = 0;
        for (int i = 0; i< children.Count; i++)
        {
            children[i].Reset();
        }
    }
    protected override void OnStart()
    {
        
    }

    protected override State OnUpdate()
    {
        var child = children[current];

        switch (child.Update())
        {
            case State.Running:
                return State.Running;

            case State.Success:
                current++;
                if (current == children.Count)
                    current = 0;
                return State.Running;

            case State.Failure:
                break;
            
        }

        return State.Failure;
    }

    protected override void OnStop()
    {
        //¹Ø¿¨½áÊø
        //stateHnadle.instance.currentState = stateHnadle.State.Chase;
    }
}
