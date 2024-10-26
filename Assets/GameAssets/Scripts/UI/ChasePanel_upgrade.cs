using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChasePanel_upgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite itemCell_base, itemCell_mouseOn, itemCell_selected;
    public bool listenMouseDown = false;
    public bool selected = false;
    public Vector3 targetPos;

    void Start()
    {

    }

    void Update()
    {
        //Éý¼¶µØ¿é
        if (Input.GetMouseButtonDown(0))
        {
            transform.GetComponent<Image>().sprite = itemCell_selected;
            if (Input.GetMouseButtonUp(0))
            {
                
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        listenMouseDown = true;
        transform.GetComponent<Image>().sprite = itemCell_mouseOn;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        listenMouseDown = false;
        transform.GetComponent<Image>().sprite = itemCell_base;
    }
}
