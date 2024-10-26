using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//#if UNITY_EDITOR


[CreateAssetMenu()]
public class GameBehaviorTree :ScriptableObject
{
    public RootNode rootNode;
    public List<Node> nodes = new List<Node>();
    public Node.State treeState = Node.State.Running;
    public void Reset()
    {
        rootNode.Reset();
    }
    public Node.State Update()
    {
        if(rootNode.state == Node.State.Running)
        {
            treeState = rootNode.Update();
        }
        return treeState;
    }
    /*
    public Node CreateNodeInGame(System.Type type)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);
        return node;
    }
    */

    //更新游戏流内的战斗内容
    public void UpdateBattleNode(ActionNode battleContent)
    {
        GameFlowNode gameFlowNode = LevelManager.instance.GameFlowTree.rootNode.child as GameFlowNode;
        for(int i = 0; i< gameFlowNode.children.Count; i++)
        {
            if(gameFlowNode.children[i].GetType() == typeof(BattleNode))
            {
                BattleNode battleNode = gameFlowNode.children[i] as BattleNode;
                battleNode.children.Clear();
                battleNode.children.Add(battleContent);

                break;
            }
        }
    }
    public Node CreateNodeInGame(Node node)
    {
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);
        return node;
    }

    public Node CreateNode(System.Type type)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);
        //AssetDatabase.AddObjectToAsset(node, this);
        //AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(Node node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(Node parent,Node child)
    {

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            decorator.child = child;
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            rootNode.child = child;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.children.Add(child);
        }
        AssetDatabase.SaveAssets();
    }
    public void InsertChild(int index, Node parent, Node child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            decorator.child = child;
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            rootNode.child = child;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.children.Insert(index,child);
        }
        AssetDatabase.SaveAssets();
    }
    public void RemoveChild(Node parent, Node child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            decorator.child = null;
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            rootNode.child = null;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.children.Remove(child);
        }
        AssetDatabase.SaveAssets();
    }

    public List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator && decorator.child != null)
        {
            children.Add(decorator.child);
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode && rootNode.child != null)
        {
            children.Add(rootNode.child);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            return composite.children;
        }
        return children;
    }

    public GameBehaviorTree Clone()
    {
        GameBehaviorTree tree = Instantiate(this);
        tree.rootNode = tree.rootNode.Clone() as RootNode;
        return tree;
    }
}
//#endif