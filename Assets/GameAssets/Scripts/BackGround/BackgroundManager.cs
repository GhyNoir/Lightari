using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;
    public GameObject mainTempalte;
    public GameObject colorInstractor;
    public Texture2D mainTemplateTex;
    public Material colorInstractorMAT;

    public Color expColor;
    public Color playerHealthColor;

    public float currentExpRadius, targetExpRadius;
    public float currentHealthRadius, targetHealthRadius;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mainTemplateTex = mainTempalte.GetComponent<SpriteRenderer>().sprite.texture;
        colorInstractorMAT = colorInstractor.GetComponent<SpriteRenderer>().material;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(targetExpRadius - currentExpRadius) >= 0.01f)
        {
            currentExpRadius = Mathf.Lerp(currentExpRadius, targetExpRadius, 0.1f);
            //DrawCircle(mainTemplateTex,currentExpRadius,expColor);
            colorInstractorMAT.SetFloat("_Radius2", currentExpRadius);
        }


        if (Mathf.Abs(targetHealthRadius - currentHealthRadius) >= 0.01f)
        {
            currentHealthRadius = Mathf.Lerp(currentHealthRadius, targetHealthRadius, 0.1f);
            //DrawCircle(mainTemplateTex,currentExpRadius,expColor);
            colorInstractorMAT.SetFloat("_Radius1", currentHealthRadius);
        }
    }

    public void UpdateExpRadius()
    {
        targetExpRadius = Mathf.Clamp01((LevelManager.instance.targetExp - LevelManager.instance.startExp) / (LevelManager.instance.requiredExp - LevelManager.instance.startExp)) * 0.701f;
    }
    public void UpdateHealthRadius()
    {
        targetHealthRadius = Mathf.Clamp01((LevelManager.instance.targetHealth - LevelManager.instance.startHealth) / (LevelManager.instance.requiredHealth - LevelManager.instance.startHealth)) * 0.701f;
    }
    void DrawCircle(Texture2D texture, float radius, Color color)
    {
        // 将纹理坐标转换为像素坐标
        int centerX = Mathf.RoundToInt(0.5f * texture.width);
        int centerY = Mathf.RoundToInt(0.5f * texture.height);
        int radiusInPixels = Mathf.RoundToInt(radius * texture.width);

        // 修改纹理上的像素颜色
        for (int y = -radiusInPixels; y <= radiusInPixels; y++)
        {
            for (int x = -radiusInPixels; x <= radiusInPixels; x++)
            {
                if (x * x + y * y <= radiusInPixels * radiusInPixels)
                {
                    int pixelX = centerX + x;
                    int pixelY = centerY + y;

                    // 确保像素在纹理范围内
                    if (CheckBounds(texture, new Vector2(pixelX,pixelY)))
                    {
                        texture.SetPixel(pixelX, pixelY, color);
                    }
                }
            }
        }

        texture.Apply();
    }
    public void ResetPixel(Texture2D texture)
    {
        for(int i = 1; i < (texture.width - 1); i++)
        {
            for(int j = 1; j < (texture.height - 1); j++)
            {
                texture.SetPixel(i, j, Color.black);
            }
        }
        texture.Apply();
    }
    public bool CheckBounds(Texture2D texture, Vector2 pos)
    {
        if (pos.x >= 1 && pos.x < (texture.width - 1) && pos.y >= 1 && pos.y < (texture.height - 1))
        {
            return true;
        }
        return false;
    }
}
