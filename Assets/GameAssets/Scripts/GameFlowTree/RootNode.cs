using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RootNode", menuName = "GameFlowNodes/RootNode")]
public class RootNode : Node
{
    public Node child;
    
    public override void Reset()
    {
        started = false;
        child.Reset();
    }
    protected override void OnStart()
    {
        
    }

    protected override State OnUpdate()
    {
        return child.Update();
    }

    protected override void OnStop()
    {
        
    }

    public override Node Clone()
    {
        RootNode rootNode = Instantiate(this);
        rootNode.child = child.Clone();
        return rootNode;
    }
}
