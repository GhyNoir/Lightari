using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public LevelState levelState;
    public GameBehaviorTree GameFlowTree;
    public int currentLevel;

    public float startExp, requiredExp;
    public float currentExp, targetExp;

    public float startHealth, requiredHealth;
    public float currentHealth, targetHealth;

    public float defaultFixTime;
    public float currentTimeScale, targetTimeScale;

    public bool levelPause;

    private void Awake()
    {
        instance = this;
        defaultFixTime = Time.fixedDeltaTime;
    }
    void Start()
    {
        GameFlowTree.Reset();
    }

    void Update()
    {
        GameFlowTree.Update();

        //关卡内按Tab查看大地图
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ChaseCellManager.instance.levelChase = !ChaseCellManager.instance.levelChase;
        }
    }

    public void UpdateExp(float value)
    {
        targetExp += value;
        if (targetExp > requiredExp)
        {
            targetExp = requiredExp;
        }
        BackgroundManager.instance.UpdateExpRadius();
    }

    public void UpdatePlayerHealth(float value)
    {
        targetHealth += value;
        if(targetHealth > requiredHealth)
        {
            targetHealth = requiredHealth;
        }
        BackgroundManager.instance.UpdateHealthRadius();
    }

    public void UpdateTimeScale(float value)
    {
        targetTimeScale = Mathf.Clamp01(value);
    }
}

public enum LevelState
{
    battle,
    award,
    pause,
}
