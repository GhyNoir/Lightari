using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCellControllor : MonoBehaviour
{
    //三个亮度等级
    public int lightLevel;
    public int lightEnergy;
    public bool isSolid;

    public Sprite itemIcon, cellIcon;
    private SpriteRenderer itemIconRenderer, cellIconRenderer, darkMaskRenderer;

    //该关卡内容结点
    public bool hasBattle, hasCreature;
    public ActionNode levelContent;

    //chase移动相关
    public Vector2 currentPos, targetPos;

    public List<GameObject> neighbours = new List<GameObject>();

    Color colorTemp;

    void Start()
    {
        cellIconRenderer = GetComponent<SpriteRenderer>();
        darkMaskRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        itemIconRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (ChaseCellManager.instance.levelChase)
        {
            ToChase();
        }
        else
        {
            ToBattle();
        }

        //根据亮度等级改变颜色
        if (lightLevel == 0)
        {
            darkMaskRenderer.color = new Color(darkMaskRenderer.color.r, darkMaskRenderer.color.g, darkMaskRenderer.color.b,
                Mathf.Lerp(darkMaskRenderer.color.a, 1, 0.1f));
        }
        else if (lightLevel == 1)
        {
            darkMaskRenderer.color = new Color(darkMaskRenderer.color.r, darkMaskRenderer.color.g, darkMaskRenderer.color.b,
                Mathf.Lerp(darkMaskRenderer.color.a, 0.6f, 0.1f));
        }
        else
        {
            darkMaskRenderer.color = new Color(darkMaskRenderer.color.r, darkMaskRenderer.color.g, darkMaskRenderer.color.b,
                Mathf.Lerp(darkMaskRenderer.color.a, 0, 0.1f));
        }
    }

    public void ToBattle()
    {
        cellIconRenderer.color = new Color(cellIconRenderer.color.r, cellIconRenderer.color.g, cellIconRenderer.color.b,
            Mathf.Lerp(cellIconRenderer.color.a, 0,0.1f));
        itemIconRenderer.color = new Color(itemIconRenderer.color.r, itemIconRenderer.color.g, itemIconRenderer.color.b,
            Mathf.Lerp(itemIconRenderer.color.a, 0, 0.1f));
        darkMaskRenderer.color = new Color(darkMaskRenderer.color.r, darkMaskRenderer.color.g, darkMaskRenderer.color.b,
            Mathf.Lerp(darkMaskRenderer.color.a, 0, 0.1f));
    }

    public void ToChase()
    {
        cellIconRenderer.color = new Color(cellIconRenderer.color.r, cellIconRenderer.color.g, cellIconRenderer.color.b,
            Mathf.Lerp(cellIconRenderer.color.a, 1, 0.1f));
        itemIconRenderer.color = new Color(itemIconRenderer.color.r, itemIconRenderer.color.g, itemIconRenderer.color.b,
            Mathf.Lerp(itemIconRenderer.color.a, 1, 0.1f));
        darkMaskRenderer.color = new Color(darkMaskRenderer.color.r, darkMaskRenderer.color.g, darkMaskRenderer.color.b,
            Mathf.Lerp(darkMaskRenderer.color.a, 1, 0.1f));
    }

    public void OnMouseEnter()
    {
        ChaseCellManager.instance.mouseOnCell = gameObject;
    }
}
