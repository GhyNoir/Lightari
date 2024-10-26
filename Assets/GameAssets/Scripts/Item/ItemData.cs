using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ItemData : ScriptableObject
{
    public int itemID;
    public int itemLevel;
    public ItemType itemType;
    public string itemName;
    public bool isInfite;

    [TextArea]
    public string discription;

    public bool generated = false;
    public int maxGeneratedTime;
    public int generatedTime;
    public float probability;
    public GameObject itemObj;
    public Sprite icon;

    public abstract void Apply();
    public abstract void Delete();

    public virtual ItemData Clone()
    {
        return Instantiate(this);
    }
}


public enum ItemType
{
    none,
    buff,
    weapon,

}

public enum ItemSelectType
{
    pick,
    all,
}

public enum ItemSelectSource
{
    Level,
    Event,
}
