using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode : Node
{
    public Node child;

    public override Node Clone()
    {
        DecoratorNode rootNode = Instantiate(this);
        rootNode.child = child.Clone();
        return rootNode;
    }
}
