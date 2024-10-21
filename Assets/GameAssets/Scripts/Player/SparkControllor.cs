using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkControllor : MonoBehaviour
{
    public bool scale_phase1, scale_phase2;

    public Vector2 moveDirection;
    public float moveSpeed;

    void Start()
    {
        scale_phase1 = true; scale_phase2 = false;
    }

    void Update()
    {
        if (!LevelManager.instance.levelPause)
        {
            Move();
        }
    }
    void FixedUpdate()
    {

    }
    public void Move()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime * PlayerControllor.instance.globalSpeed);
    }
    public void ScaleChange()
    {
        if (scale_phase1)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3,3),0.05f);
            if(Vector2.Distance(transform.localScale, new Vector2(3,3)) < 0.05f)
            {
                scale_phase1 = false;
                scale_phase2 = true;
            }
        }
        if (scale_phase2)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0, 0), 0.1f);
            if (Vector2.Distance(transform.localScale, new Vector2(0, 0)) < 0.05f)
            {
                Destroy(gameObject);
            }
        }
    }
}
