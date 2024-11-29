using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject selectBar , itemCellPrefeb;
    public GameObject descriptImage, description;
    public List<GameObject> itemCells = new List<GameObject>();
    public Color colorTemp;
    //（弃用）
    public List<GameObject> chaseDirectionObj;

    public bool updateRadius = true,  showItemBar = true, showItemCell = false, arrangeCell = false;

    //（弃用）
    public bool showChaseDirectionUI, hideChaseDirectionUI;

    //chase button
    public bool showChaseInteractPanel, hideChaseInteractPanel;
    public bool waitChaseButton;
    public int boxOnButtonIndex;
    public GameObject boxOnButton;
    public List<GameObject> chasePanelButtons = new List<GameObject>();
    public List<GameObject> chaseButtonInSight = new List<GameObject>();
    public GameObject hintPanel;
    public GameObject chasePanel_lightUp, chasePanel_upgrade, chasePanel_combat;
    public GameObject chasePanel_photonNum;
    public Sprite buttonIcon_base, buttonIcon_boxOn, buttonIcon_selected;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ResetData();
    }

    private void FixedUpdate()
    {
        if(ItemManager.instance.mouseOnItem != null)
        {
            ShowItemDescription();
        }
        else
        {
            HideItemDescription();
        }


        if (showChaseInteractPanel)
        {
            ShowChaseInteractPanel();
        }
        if (hideChaseInteractPanel)
        {
            HideChaseInteractPanel();
        }
        /*
        if (showChaseDirectionUI)
        {
            StartCoroutine(ShowChaseDirectionUI());
        }
        if (hideChaseDirectionUI)
        {
            StartCoroutine(HideChaseDirectionUI());
        }
        */
    }

    public void GenerateItemCells()
    {
        for (int i = 0; i < ItemManager.instance.generatedItem.Count; i++)
        {
            if (ItemManager.instance.generatedItem[i].itemType != ItemType.none)
            {
                GameObject cellTemp = Instantiate(itemCellPrefeb, new Vector3(0, 0, 0), Quaternion.identity);

                cellTemp.GetComponent<ItemCellControllor>().item = ItemManager.instance.generatedItem[i];
                cellTemp.GetComponent<ItemCellControllor>().targetPos = new Vector3(-45 + 25 * i, 0, 0);
                cellTemp.transform.parent = selectBar.transform;
                if (ItemManager.instance.generatedItem[i].icon != null)
                {
                    cellTemp.transform.GetChild(0).GetComponent<Image>().sprite = ItemManager.instance.generatedItem[i].icon;
                }
                cellTemp.GetComponent<RectTransform>().localScale = new Vector3(0.2f, 0.2f, 0.2f);
                cellTemp.GetComponent<RectTransform>().localPosition = new Vector3(-45 + 50 * i, 40, 0);
                itemCells.Add(cellTemp);
            }
        }
    }

    public void ResetData()
    {
        updateRadius = true;  showItemBar = true; showItemCell = false; arrangeCell = false;

        selectBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);
        selectBar.GetComponent<CanvasRenderer>().SetAlpha(1);
        selectBar.GetComponent<Image>().fillAmount = 0;

        for (int i = 0; i < itemCells.Count; i++)
        {
            itemCells[i].GetComponent<CanvasRenderer>().SetAlpha(0);
            itemCells[i].transform.GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(0);
        }
    }
    public void ShowAwardPanel()
    {
        if (showItemBar)
        {
            selectBar.GetComponent<Image>().fillAmount = Mathf.Lerp(selectBar.GetComponent<Image>().fillAmount, 1, 0.1f);

            if (selectBar.GetComponent<Image>().fillAmount >= 0.99f)
            {
                showItemBar = false;
                showItemCell = true;
            }
            if (updateRadius)
            {
                BackgroundManager.instance.targetExpRadius = 0;
                updateRadius = false;
            }
        }
        if (showItemCell)
        {
            selectBar.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(selectBar.GetComponent<CanvasRenderer>().GetAlpha(), 1, 0.1f));
            selectBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(selectBar.GetComponent<RectTransform>().rect.height, 27, 0.2f));
            for (int i = 0; i < itemCells.Count; i++)
            {
                //cell浮现
                itemCells[i].GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(itemCells[i].GetComponent<CanvasRenderer>().GetAlpha(), 1, 0.05f));
                itemCells[i].transform.GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(
                    Mathf.Lerp(
                        itemCells[i].transform.GetChild(0).GetComponent<CanvasRenderer>().GetAlpha(),
                        1,
                        0.05f)
                    );
                //cell移动
                itemCells[i].GetComponent<RectTransform>().localPosition = Vector3.Lerp(
                    itemCells[i].GetComponent<RectTransform>().localPosition,
                    new Vector3(itemCells[i].GetComponent<RectTransform>().localPosition.x, 0, 0),
                    0.05f
                    );

            }

            if(itemCells[itemCells.Count - 1].GetComponent<RectTransform>().localPosition.y <= 0.01f)
            {
                itemCells[itemCells.Count - 1].GetComponent<RectTransform>().localPosition = 
                    new Vector2(itemCells[itemCells.Count - 1].GetComponent<RectTransform>().localPosition.x,0);
                showItemCell = false;
                ItemManager.instance.allowSelect = true;
            }
        }
    }
    public void HideAwardPanel()
    {
        for (int i = 0; i < itemCells.Count; i++)
        {
            //cell浮现
            itemCells[i].GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(itemCells[i].GetComponent<CanvasRenderer>().GetAlpha(), 0, 0.2f));
            itemCells[i].transform.GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(
                Mathf.Lerp(
                    itemCells[i].transform.GetChild(0).GetComponent<CanvasRenderer>().GetAlpha(),
                    1,
                    0.03f)
                );

        }
        selectBar.GetComponent<Image>().fillAmount = Mathf.Lerp(selectBar.GetComponent<Image>().fillAmount, 0, 0.03f);

        if(selectBar.GetComponent<Image>().fillAmount<= 0.02f)
        {
            for (int i = 0; i < itemCells.Count; i++)
            {
                Destroy(itemCells[i]);
            }
            itemCells.Clear();
            ItemManager.instance.selectAwardItem = true;
        }
    }

    public void ShowItemDescription()
    {
        descriptImage.GetComponent<Image>().color = Color.Lerp(descriptImage.GetComponent<Image>().color,
            new Color(descriptImage.GetComponent<Image>().color.r,
                      descriptImage.GetComponent<Image>().color.g,
                      descriptImage.GetComponent<Image>().color.b, 1), 0.3f);
        description.GetComponent<Graphic>().color = Color.Lerp(descriptImage.GetComponent<Graphic>().color,
            new Color(descriptImage.GetComponent<Image>().color.r,
                      descriptImage.GetComponent<Image>().color.g,
                      descriptImage.GetComponent<Image>().color.b, 1), 0.3f);
        description.GetComponent<Text>().text = ItemManager.instance.mouseOnItem.discription;
    }
    public void HideItemDescription()
    {
        descriptImage.GetComponent<Image>().color = Color.Lerp(descriptImage.GetComponent<Image>().color,
            new Color(descriptImage.GetComponent<Image>().color.r,
                      descriptImage.GetComponent<Image>().color.g,
                      descriptImage.GetComponent<Image>().color.b, 0), 0.3f);
        description.GetComponent<Graphic>().color = Color.Lerp(descriptImage.GetComponent<Graphic>().color,
            new Color(descriptImage.GetComponent<Image>().color.r,
                      descriptImage.GetComponent<Image>().color.g,
                      descriptImage.GetComponent<Image>().color.b, 0), 0.3f);
    }

    IEnumerator ShowChaseDirectionUI()
    {
        showChaseDirectionUI = false;
        while(chaseDirectionObj[0].GetComponent<Image>().color.a <= 0.98f)
        {
            for(int i = 0; i < chaseDirectionObj.Count; i++)
            {
                colorTemp = chaseDirectionObj[i].GetComponent<Image>().color;
                chaseDirectionObj[i].GetComponent<Image>().color = new Color(colorTemp.r, colorTemp.g, colorTemp.b, Mathf.Lerp(colorTemp.a, 1, 0.1f));
            }
            yield return new WaitForSecondsRealtime(0.02f);
        }

        for (int i = 0; i < chaseDirectionObj.Count; i++)
        {
            colorTemp = chaseDirectionObj[i].GetComponent<Image>().color;
            chaseDirectionObj[i].GetComponent<Image>().color = new Color(colorTemp.r, colorTemp.g, colorTemp.b, 1);
        }
    }

    IEnumerator HideChaseDirectionUI()
    {
        hideChaseDirectionUI = false;

        showChaseDirectionUI = false;
        while (chaseDirectionObj[0].GetComponent<Image>().color.a >= 0.02f)
        {
            for (int i = 0; i < chaseDirectionObj.Count; i++)
            {
                colorTemp = chaseDirectionObj[i].GetComponent<Image>().color;
                chaseDirectionObj[i].GetComponent<Image>().color = new Color(colorTemp.r, colorTemp.g, colorTemp.b, Mathf.Lerp(colorTemp.a, 0, 0.1f));
            }
            yield return new WaitForSecondsRealtime(0.02f);
        }

        for (int i = 0; i < chaseDirectionObj.Count; i++)
        {
            colorTemp = chaseDirectionObj[i].GetComponent<Image>().color;
            chaseDirectionObj[i].GetComponent<Image>().color = new Color(colorTemp.r, colorTemp.g, colorTemp.b, 0);
        }
    }

    public void ShowChaseInteractPanel()
    {
        //有建筑时允许升级
        if (ChaseCellManager.instance.boxOnCell.GetComponent<ChaseCellControllor>().hasCreature)
        {
            Vector2 posTemp = chasePanel_upgrade.GetComponent<RectTransform>().localPosition;
            chasePanel_upgrade.GetComponent<RectTransform>().localPosition = Vector2.Lerp(posTemp, new Vector2(posTemp.x, -280), 0.2f);
        }
        else
        {
            Vector2 posTemp = chasePanel_upgrade.GetComponent<RectTransform>().localPosition;
            chasePanel_upgrade.GetComponent<RectTransform>().localPosition = Vector2.Lerp(posTemp, new Vector2(posTemp.x, -650), 0.2f);
        }


        //黑暗地块允许点亮
        if (ChaseCellManager.instance.boxOnCell.GetComponent<ChaseCellControllor>().lightLevel == 0 && ChaseCellManager.instance.playerPhotonNum > 0)
        {
            Vector2 posTemp = chasePanel_lightUp.GetComponent<RectTransform>().localPosition;
            chasePanel_lightUp.GetComponent<RectTransform>().localPosition = Vector2.Lerp(posTemp, new Vector2(posTemp.x, -280), 0.2f);
        }
        else
        {
            Vector2 posTemp = chasePanel_lightUp.GetComponent<RectTransform>().localPosition;
            chasePanel_lightUp.GetComponent<RectTransform>().localPosition = Vector2.Lerp(posTemp, new Vector2(posTemp.x, -650), 0.2f);
        }

        //可视范围内关卡内容允许进入
        if (ChaseCellManager.instance.boxOnCell.GetComponent<ChaseCellControllor>().hasBattle&& ChaseCellManager.instance.boxOnCell.GetComponent<ChaseCellControllor>().lightLevel > 0)
        {
            Vector2 posTemp = chasePanel_combat.GetComponent<RectTransform>().localPosition;
            chasePanel_combat.GetComponent<RectTransform>().localPosition = Vector2.Lerp(posTemp, new Vector2(posTemp.x, -280), 0.2f);
        }
        else
        {
            Vector2 posTemp = chasePanel_combat.GetComponent<RectTransform>().localPosition;
            chasePanel_combat.GetComponent<RectTransform>().localPosition = Vector2.Lerp(posTemp, new Vector2(posTemp.x, -650), 0.2f);
        }
        /*
        Color colorTemp = chasePanel_photonNum.transform.GetChild(0).GetComponent<Text>().color;
        chasePanel_photonNum.transform.GetChild(0).GetComponent<Text>().color = Color.Lerp(
            chasePanel_photonNum.transform.GetChild(0).GetComponent<Text>().color, new Color(colorTemp.r, colorTemp.g, colorTemp.b, 1), 0.2f);

        colorTemp = chasePanel_photonNum.transform.GetChild(1).GetComponent<Text>().color;
        chasePanel_photonNum.transform.GetChild(1).GetComponent<Text>().color = Color.Lerp(
            chasePanel_photonNum.transform.GetChild(1).GetComponent<Text>().color, new Color(colorTemp.r, colorTemp.g, colorTemp.b, 1), 0.2f);
        */

        colorTemp = hintPanel.transform.GetChild(1).GetComponent<Text>().color;
        chasePanel_photonNum.transform.GetChild(1).GetComponent<Text>().color = Color.Lerp(
            chasePanel_photonNum.transform.GetChild(1).GetComponent<Text>().color, new Color(colorTemp.r, colorTemp.g, colorTemp.b, 1), 0.2f);

        colorTemp = hintPanel.transform.GetChild(0).GetComponent<Text>().color;
        hintPanel.transform.GetChild(0).GetComponent<Text>().color = Color.Lerp(
            hintPanel.transform.GetChild(0).GetComponent<Text>().color, new Color(colorTemp.r, colorTemp.g, colorTemp.b, 1), 0.2f);

        colorTemp = hintPanel.transform.GetChild(1).GetComponent<Text>().color;
        hintPanel.transform.GetChild(1).GetComponent<Text>().color = Color.Lerp(
            hintPanel.transform.GetChild(1).GetComponent<Text>().color, new Color(colorTemp.r, colorTemp.g, colorTemp.b, 1), 0.2f);

    }

    public void HideChaseInteractPanel()
    {
        Vector2 posTemp = chasePanel_lightUp.GetComponent<RectTransform>().localPosition;
        chasePanel_lightUp.GetComponent<RectTransform>().localPosition = Vector2.Lerp(posTemp, new Vector2(posTemp.x, -650), 0.2f);

        posTemp = chasePanel_upgrade.GetComponent<RectTransform>().localPosition;
        chasePanel_upgrade.GetComponent<RectTransform>().localPosition = Vector2.Lerp(posTemp, new Vector2(posTemp.x, -650), 0.2f);

        posTemp = chasePanel_combat.GetComponent<RectTransform>().localPosition;
        chasePanel_combat.GetComponent<RectTransform>().localPosition = Vector2.Lerp(posTemp, new Vector2(posTemp.x, -650), 0.2f);
        /*
        Color colorTemp = chasePanel_photonNum.transform.GetChild(0).GetComponent<Text>().color;
        chasePanel_photonNum.transform.GetChild(0).GetComponent<Text>().color = Color.Lerp(
            chasePanel_photonNum.transform.GetChild(0).GetComponent<Text>().color, new Color(colorTemp.r, colorTemp.g, colorTemp.b,0),0.2f);
        */
        colorTemp = hintPanel.transform.GetChild(0).GetComponent<Text>().color;
        hintPanel.transform.GetChild(0).GetComponent<Text>().color = Color.Lerp(
            hintPanel.transform.GetChild(0).GetComponent<Text>().color, new Color(colorTemp.r, colorTemp.g, colorTemp.b, 0), 0.2f);

        colorTemp = hintPanel.transform.GetChild(1).GetComponent<Text>().color;
        hintPanel.transform.GetChild(1).GetComponent<Text>().color = Color.Lerp(
            hintPanel.transform.GetChild(1).GetComponent<Text>().color, new Color(colorTemp.r, colorTemp.g, colorTemp.b, 0), 0.2f);
    }
    //选择界面更新玩家输入
    public void UpdateBoxOnCell()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            boxOnButtonIndex -= 1;
            if(boxOnButtonIndex < 0)
            {
                boxOnButtonIndex = chasePanelButtons.Count - 1;
            }
            boxOnButton = chasePanelButtons[boxOnButtonIndex];

            for (int i = 0; i < chasePanelButtons.Count; i++)
            {
                chasePanelButtons[i].GetComponent<Image>().sprite = buttonIcon_base;
            }
            boxOnButton.GetComponent<Image>().sprite = buttonIcon_boxOn;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            boxOnButtonIndex += 1;
            if (boxOnButtonIndex > (chasePanelButtons.Count -1))
            {
                boxOnButtonIndex = 0;
            }
            boxOnButton = chasePanelButtons[boxOnButtonIndex];

            for(int i = 0; i < chasePanelButtons.Count; i++)
            {
                chasePanelButtons[i].GetComponent<Image>().sprite = buttonIcon_base;
            }
            boxOnButton.GetComponent<Image>().sprite = buttonIcon_boxOn;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            boxOnButton.GetComponent<Image>().sprite = buttonIcon_selected;
        }
    }
    //更新UI光粒数字
    public void UpdatePhotonNumber()
    {
        chasePanel_photonNum.transform.GetChild(1).GetComponent<Text>().text = ChaseCellManager.instance.playerPhotonNum.ToString();
    }
}
