using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Database/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemList> baseItems;

    public List<ItemList> weaponItems;

    public List<ItemList> specialItems;
}

[System.Serializable]
public class ItemList
{
    public List<ItemData> itemDatas;

}
