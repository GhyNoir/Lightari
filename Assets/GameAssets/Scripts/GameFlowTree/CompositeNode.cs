using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : Node
{
    public List<Node> children = new List<Node>();

    public override Node Clone()
    {
        CompositeNode rootNode = Instantiate(this);
        rootNode.children = children.ConvertAll(c => c.Clone());
        return rootNode;
    }
}
