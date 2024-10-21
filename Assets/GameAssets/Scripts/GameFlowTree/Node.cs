using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject, IComparer<Node>
{
    public enum State 
    {
        Running,
        Failure,
        Success
    }
    public enum NodeType
    {
        RootNode,
        GameFlowNode,
        BattleNode,
        BattleContentNode,
        StoreNode,
        AwardNode,
        TransNode,
    }
    public State state = State.Running;
    public NodeType nodeType;
    public bool started = false;
    public string guid;
    public int level;  //等级
    public int order;  //优先级
    public Vector2 position;

    public State Update()
    {
        if (!started)
        {
            OnStart();
            started = true;
        }
        state = OnUpdate();

        if(state == State.Failure || state == State.Success)
        {
            OnStop();
            started = false;
        }

        return state;
    }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }
    public int Compare(Node x, Node y)
    {
        return x.order.CompareTo(y.order);
    }
    public abstract void Reset();
    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
