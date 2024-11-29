using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeRunner : MonoBehaviour
{
    public GameBehaviorTree behaviorTree;

    void Start()
    {
        behaviorTree = behaviorTree.Clone();
    }

    // Update is called once per frame
    void Update()
    {
        if(behaviorTree != null)
        {
            behaviorTree.Update();
        }        
    }
}
