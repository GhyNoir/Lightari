using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPointControllor : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void Spark()
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            LevelManager.instance.outMaze = true;
        }
    }
}
