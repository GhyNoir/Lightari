using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePoint : MonoBehaviour
{
    public Texture2D MenuMosue;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Cursor.SetCursor(MenuMosue, Vector2.zero, CursorMode.Auto);

    }
    void OnMouseEnter()
    {
        Cursor.SetCursor(MenuMosue, Vector2.zero, CursorMode.Auto);
    }
    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}