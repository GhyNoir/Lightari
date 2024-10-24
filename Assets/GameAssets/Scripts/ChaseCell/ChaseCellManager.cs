using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCellManager : MonoBehaviour
{
    public static ChaseCellManager instance;

    public GameObject cellPrefeb;
    //弃用
    public Sprite lightSprite, darkSprite;

    public List<Sprite> cellIcons;
    public List<ActionNode> levelContentNodes;

    public List<GameObject> cells = new List<GameObject>();

    public float width, height;
    float cellSize = 4.5f;

    //选择方向（弃用）
    public Vector2 chasedDirection;
    public bool isChasedDirection;

    //Chase move
    public bool allowMove;
    public Vector2 moveTarget;

    public GameObject mouseOnCell;
    public GameObject boxOnCell;

    //chase界面玩法交互参数
    public int playerPhotonNum;
    public int lightedHouseNum;

    public bool isChased;
    public bool levelChase;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GenerateCellMap();
        boxOnCell = GetCell(new Vector2(0,0));
    }

    public void GenerateCellMap()
    {
        for(int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 cellPos = new Vector2(i * cellSize - (width - 1) * cellSize / 2, j * cellSize - (height - 1) * cellSize / 2);
                float value = Mathf.PerlinNoise(cellPos.x, cellPos.y) + Random.Range(-0.5f,0.5f);

                //战斗关卡
                if(value >= 0.6f)
                {
                    GameObject cellTemp = Instantiate(cellPrefeb, cellPos, Quaternion.identity);
                    cellTemp.GetComponent<ChaseCellControllor>().targetPos = cellTemp.transform.position;
                    cellTemp.GetComponent<ChaseCellControllor>().levelContent = levelContentNodes[0];
                    cellTemp.GetComponent<ChaseCellControllor>().hasBattle = true;
                    cellTemp.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = cellIcons[0];
                    cells.Add(cellTemp);
                }
                //迷宫关卡
                else if(value >= 0.4f)
                {
                    GameObject cellTemp = Instantiate(cellPrefeb, cellPos, Quaternion.identity);
                    cellTemp.GetComponent<ChaseCellControllor>().targetPos = cellTemp.transform.position;
                    cellTemp.GetComponent<ChaseCellControllor>().levelContent = levelContentNodes[0];
                    cellTemp.GetComponent<ChaseCellControllor>().hasBattle = true;
                    cellTemp.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = cellIcons[1];
                    cells.Add(cellTemp);
                }
                //空白关卡
                else
                {
                    GameObject cellTemp = Instantiate(cellPrefeb, cellPos, Quaternion.identity);
                    cellTemp.GetComponent<ChaseCellControllor>().targetPos = cellTemp.transform.position;
                    cellTemp.GetComponent<ChaseCellControllor>().levelContent = levelContentNodes[0];
                    cellTemp.GetComponent<ChaseCellControllor>().hasBattle = true;
                    cellTemp.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = cellIcons[0];
                    cells.Add(cellTemp);
                }

            }
        }

        //记录相邻9格内邻居
        for(int i = 0; i < cells.Count; i++)
        {
            for(int j = 0; j < cells.Count; j++)
            {
                if (i != j)
                {
                    if (((cells[i].transform.position - cells[j].transform.position).magnitude - 4.5f)<=0.1f
                        || ((cells[i].transform.position - cells[j].transform.position).magnitude - 6.363f) <= 0.1f)
                    {
                        cells[i].GetComponent<ChaseCellControllor>().neighbours.Add(cells[j]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 更新所有的cell, 默认的更新规则是玩家每走一步都变暗
    /// </summary>
    /// <param name="rule"></param>
    /// <returns></returns>
    public void UpdateAllCells(UpdateRule rule)
    {
        if(rule == UpdateRule.darken)
        {
            //全局变暗
            for(int i = 0; i < cells.Count; i++)
            {
                UpdateCellLight(cells[i], -1);
            }
        }
    }

    //在chase界面和cell交互
    public void ChaseCell()
    {
        if (UIManager.instance.waitChaseButton)
        {
            //按Q退出选择界面
            if (Input.GetKeyUp(KeyCode.Q))
            {
                //隐藏cell交互UI
                UIManager.instance.waitChaseButton = false;
                UIManager.instance.showChaseInteractPanel = false;
                UIManager.instance.hideChaseInteractPanel = true;
            }
            //按E获取选择结果
            if (Input.GetKeyUp(KeyCode.E))
            {
                //显示cell交互UI
                UIManager.instance.waitChaseButton = false;
                UIManager.instance.showChaseInteractPanel = false;
                UIManager.instance.hideChaseInteractPanel = true;

                if (boxOnCell.GetComponent<ChaseCellControllor>().lightLevel > 0)
                {
                    //已经点亮建筑
                    if (boxOnCell.GetComponent<ChaseCellControllor>().hasCreature)
                    {
                        UpgradeCell(boxOnCell);
                    }
                    //有敌人占据
                    else if (boxOnCell.GetComponent<ChaseCellControllor>().hasBattle)
                    {
                        //更新关卡内容并进入战斗
                        LevelManager.instance.GameFlowTree.UpdateBattleNode(boxOnCell.GetComponent<ChaseCellControllor>().levelContent);
                        isChased = true;
                    }
                }
                else
                {
                    LightUpCell(boxOnCell);
                    
                }
            }

            //更新玩家选择的button
            UIManager.instance.UpdateBoxOnCell();
        }
        else
        {
            //按E进入选择界面
            if (Input.GetKeyUp(KeyCode.E))
            {
                //显示cell交互UI
                UIManager.instance.waitChaseButton = true;
                UIManager.instance.showChaseInteractPanel = true;
                UIManager.instance.hideChaseInteractPanel = false;
                UIManager.instance.boxOnButton = UIManager.instance.chasePanelButtons[0];
            }

            //场景地块移动
            ChaseMove();
        }
    }

    //选择界面进行移动
    public void ChaseMove()
    {
        Vector2 move = new Vector2(0, 0);
        if (allowMove)
        {
            if (Input.GetKeyUp(KeyCode.W))
            {
                move = new Vector2(0, 1);
                allowMove = false;
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                move = new Vector2(-1, 0);
                allowMove = false;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                move = new Vector2(0, -1);
                allowMove = false;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                move = new Vector2(1, 0);
                allowMove = false;
            }

            for(int i = 0; i < cells.Count; i++)
            {
                cells[i].GetComponent<ChaseCellControllor>().targetPos -= move * cellSize;
            }
        }
        else
        {
            MoveCells();
        }
    }

    public void MoveCells()
    {
        for(int i = 0; i < cells.Count; i++)
        {
            cells[i].transform.position = Vector2.Lerp(cells[i].transform.position, cells[i].GetComponent<ChaseCellControllor>().targetPos,0.4f);
        }

        if(((Vector2)cells[0].transform.position - cells[0].GetComponent<ChaseCellControllor>().targetPos).magnitude <= 0.2f)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].transform.position = cells[i].GetComponent<ChaseCellControllor>().targetPos;
            }
            boxOnCell = GetCell(new Vector2(0,0));
            allowMove = true;
        }
    }

    public void UpdateCellLight(GameObject targetCell, int level)
    {
        //更新lightEnergy
        targetCell.GetComponent<ChaseCellControllor>().lightEnergy += level;

        //根据lightEnergy更新lightLevel
        //高亮状态
        if(targetCell.GetComponent<ChaseCellControllor>().lightEnergy >= 3)
        {
            targetCell.GetComponent<ChaseCellControllor>().lightLevel = 2;
        }
        //仅可视状态
        else if(targetCell.GetComponent<ChaseCellControllor>().lightEnergy >= 1)
        {
            targetCell.GetComponent<ChaseCellControllor>().lightLevel = 1;
        }
        //不可视状态
        else
        {
            targetCell.GetComponent<ChaseCellControllor>().lightLevel = 0;
        }

        if (targetCell.GetComponent<ChaseCellControllor>().lightLevel > 2)
        {
            targetCell.GetComponent<ChaseCellControllor>().lightLevel = 2;
        }
        if(targetCell.GetComponent<ChaseCellControllor>().lightLevel < 0)
        {
            targetCell.GetComponent<ChaseCellControllor>().lightLevel = 0;
        }

        //照亮周围cell
        if(targetCell.GetComponent<ChaseCellControllor>().lightLevel == 2)
        {
            for(int i = 0; i < targetCell.GetComponent<ChaseCellControllor>().neighbours.Count; i++)
            {
                if(targetCell.GetComponent<ChaseCellControllor>().neighbours[i].GetComponent<ChaseCellControllor>().lightLevel == 0)
                {
                    targetCell.GetComponent<ChaseCellControllor>().neighbours[i].GetComponent<ChaseCellControllor>().lightLevel = 1;
                }
            }
        }
    }
    //点亮cell
    public void LightUpCell(GameObject targetCell)
    {
        if(playerPhotonNum > 0)
        {
            UpdateCellLight(targetCell, 5);
            playerPhotonNum -= 1;
            UIManager.instance.UpdatePhotonNumber();
        }
    }
    public void UpgradeCell(GameObject targetCell)
    {
        GetCell((Vector2)targetCell.transform.position + cellSize * new Vector2(0, 2)).GetComponent<ChaseCellControllor>().lightLevel = 1;
        GetCell((Vector2)targetCell.transform.position + cellSize * new Vector2(0, -2)).GetComponent<ChaseCellControllor>().lightLevel = 1;
        GetCell((Vector2)targetCell.transform.position + cellSize * new Vector2(2, 0)).GetComponent<ChaseCellControllor>().lightLevel = 1;
        GetCell((Vector2)targetCell.transform.position + cellSize * new Vector2(-2, 0)).GetComponent<ChaseCellControllor>().lightLevel = 1;
        playerPhotonNum -= 1;
        UIManager.instance.UpdatePhotonNumber();
    }

    public void DarkenCell(GameObject targetCell, int level)
    {
        targetCell.GetComponent<SpriteRenderer>().sprite = darkSprite;
        targetCell.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
    }

    public GameObject GetCell(Vector2 targetPos)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if(((Vector2)cells[i].transform.position - targetPos).magnitude < 0.2f)
            {
                return cells[i];
            }
        }
        return null;
    }
}

public enum UpdateRule
{
    darken,
    lighten,
    unchange,
}
