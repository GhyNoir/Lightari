using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyManager : MonoBehaviour
{
    public static SupplyManager instance;
    public float spawnTimer,spawnInterval = 1.5f;
    public int maxSpawn;
    public GameObject supplyPrefeb;
    public List<GameObject> supplyList = new List<GameObject>();
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    public void GenerateSupply(int level)
    {
        if (supplyList.Count < maxSpawn)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                GameObject supplyTemp = Instantiate(supplyPrefeb,
                    new Vector2(Random.Range(-1.7f, 1.7f), Random.Range(-1.7f, 1.7f)), Quaternion.identity);
                supplyList.Add(supplyTemp);
                spawnTimer = spawnInterval;
            }
        }
    }
}
