using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public List<EnemyList> enemyPrefebDatabase;
    public List<GameObject> bossPrefebList;

    public float spawnTimer, spawnInterval = 1.5f;
    public int maxSpawn;
    public bool allowSpawn;
    public List<GameObject> enemyList = new List<GameObject>();


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        allowSpawn = true;
    }
    public void ResetScene()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            Destroy(enemyList[i]);
        }
        enemyList.Clear();
    }
    //根据关卡等级生成敌人
    public void GenerateEnemy(int level)
    {
        if (allowSpawn)
        {
            if (enemyList.Count < maxSpawn)
            {
                spawnTimer -= Time.deltaTime;
                if (spawnTimer <= 0)
                {
                    int enemyIndex = Random.Range(0, enemyPrefebDatabase[0].enemyPrefebList.Count);
                    GameObject enemyTemp = Instantiate(enemyPrefebDatabase[0].enemyPrefebList[enemyIndex],
                        new Vector2(Random.Range(-1.7f, 1.7f), Random.Range(-1.7f, 1.7f)),
                        Quaternion.Euler(0, 0, Random.Range(-180, 180)));
                    enemyList.Add(enemyTemp);
                    spawnTimer = spawnInterval;
                }
            }

            //第五关生成boss1
            if (level == 5)
            {
                GameObject bossTemp = Instantiate(bossPrefebList[0], new Vector2(0, 0), Quaternion.identity);

                allowSpawn = false;
            }
        }

    }
}

[System.Serializable]
public class EnemyList
{
    public List<GameObject> enemyPrefebList;
}
