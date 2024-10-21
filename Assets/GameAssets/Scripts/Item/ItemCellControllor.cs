using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemCellControllor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData item;
    public Sprite itemCell_base, itemCell_mouseOn, itemCell_selected;
    public bool listenMouseDown = false;
    public bool selected = false;
    public Vector3 targetPos;

    public void Update()
    {
        if (ItemManager.instance.allowSelect)
        {
            if (listenMouseDown)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    transform.GetComponent<Image>().sprite = itemCell_selected;
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    transform.GetComponent<Image>().sprite = itemCell_base;
                    item.Apply();
                    item.generatedTime += 1;
                    ItemManager.instance.selectedItem.Add(item);
                    ItemManager.instance.isSelected = true;
                    selected = true;
                }
            }
        }
    }

    public void FixedUpdate()
    {
        if (selected)
        {
            HideAnim();
        }
    }
    public void ShowAnim()
    {

    }
    public void HideAnim()
    {
        transform.GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(transform.GetComponent<CanvasRenderer>().GetAlpha(), 0, 0.2f));
        transform.GetChild(0).GetComponent<CanvasRenderer>().SetAlpha(Mathf.Lerp(transform.transform.GetChild(0).GetComponent<CanvasRenderer>().GetAlpha(), 0, 0.2f));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        listenMouseDown = true;
        transform.GetComponent<Image>().sprite = itemCell_mouseOn;
        ItemManager.instance.mouseOnItem = item;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        listenMouseDown = false;
        transform.GetComponent<Image>().sprite = itemCell_base;
        ItemManager.instance.mouseOnItem = null;
    }
}
