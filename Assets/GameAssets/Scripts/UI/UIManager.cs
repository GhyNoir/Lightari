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

    public bool updateRadius = true,  showItemBar = true, showItemCell = false, arrangeCell = false;

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
                //cell∏°œ÷
                itemCells[i].GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(itemCells[i].GetComponent<CanvasRenderer>().GetAlpha(), 1, 0.05f));
                itemCells[i].transform.GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(
                    Mathf.Lerp(
                        itemCells[i].transform.GetChild(0).GetComponent<CanvasRenderer>().GetAlpha(),
                        1,
                        0.05f)
                    );
                //cell“∆∂Ø
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
            //cell∏°œ÷
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
}
