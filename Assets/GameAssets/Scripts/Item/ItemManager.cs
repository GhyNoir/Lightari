using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public ItemDatabase itemDatabase;

    //��ؿ��ȼ��仯��Ʒ��������
    public AnimationCurve itemNumCurve;

    public int GenerateTime;
    public List<ItemData> currentItemPool = new List<ItemData>();
    public List<ItemData> generatedItem = new List<ItemData>();
    public List<ItemData> selectedItem = new List<ItemData>();
    public ItemData mouseOnItem, currentSelectedItem;

    public bool allowSelect, isSelected, selectAwardItem;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < itemDatabase.baseItems[0].itemDatas.Count; i++)
        {
            currentItemPool.Add(itemDatabase.baseItems[0].itemDatas[i].Clone());
        }
    }
    private void Update()
    {
        //������Ʒѡ��
        if (allowSelect)
        {
            if (isSelected)
            {
                generatedItem.Clear();
                selectedItem.Clear();
                UpdateItemPool();
            }

            if(mouseOnItem != null)
            {

            }
        }

    }
    public void GenerateAwardItem(ItemSelectType selectType)
    {
        //���ݹؿ��ȼ����ɿ�ѡ����Ʒ
        isSelected = false;
        generatedItem.Clear();
        selectedItem.Clear();

        if (selectType == ItemSelectType.pick)
        {
            generatedItem = GatherItemByLevel(LevelManager.instance.currentLevel, 3);
        }

        //��UIչʾѡ�е���Ʒ
        if (generatedItem.Count > 0)
        {
            UIManager.instance.itemCells.Clear();
            UIManager.instance.GenerateItemCells();
        }
    }

    /* Summary:
     * ����Level��Ϣ������Ʒ�б�
     */
    public List<ItemData> GatherItemByLevel(int level, int requireItemNum)
    {
        List<ItemData> itemsTemp = new List<ItemData>();

        if (currentItemPool.Count != 0)
        {
            for (int i = 0; i < currentItemPool.Count; i++)
            {
                currentItemPool[i].generated = false;
            }

            for (int i = 0; i < requireItemNum; i++)
            {
                Dictionary<float, ItemData> itemDict = GenerateItemDict(currentItemPool);
                float itemIndex = Random.Range(0f, 1f) * 100;
                bool generated = false;
                for (int j = 0; j < (itemDict.Count - 1); j++)
                {
                    if (itemIndex >= itemDict.ElementAt(j).Key && itemIndex < itemDict.ElementAt(j + 1).Key)
                    {
                        if (itemDict.ElementAt(j).Value.itemType != ItemType.none)
                        {
                            itemsTemp.Add(itemDict.ElementAt(j).Value);
                            itemDict.ElementAt(j).Value.generated = true;
                            generated = true;
                            break;
                        }
                    }
                }
                if (!generated)
                {
                    itemsTemp.Add(itemDict.ElementAt(itemDict.Count - 1).Value);
                    itemDict.ElementAt(itemDict.Count - 1).Value.generated = true;
                }
            }
        }
        return itemsTemp;
    }

    /* Summary:
     * ��Ȩ���ѡ����Ʒ
     */
    public Dictionary<float, ItemData> GenerateItemDict(List<ItemData> itemSet)
    {
        Dictionary<float, ItemData> itemDict = new Dictionary<float, ItemData>();

        if (itemSet != null)
        {
            //��ȡ���ʺ�
            float totalProb = 0;
            for (int j = 0; j < itemSet.Count; j++)
            {
                if (!itemSet[j].generated)
                {
                    totalProb += itemSet[j].probability;
                }

            }

            //�����ֵ�Ԫ��
            float currentNum = 0;
            for (int j = 0; j < itemSet.Count; j++)
            {
                if (!itemSet[j].generated)
                {
                    float probTemp = (float)(itemSet[j].probability / totalProb * 100);
                    itemDict.Add(currentNum, itemSet[j]);
                    currentNum += probTemp;
                }
            }
            return itemDict;
        }
        return null;
    }

    /* Summary:
     * ����ѡ����������Ʒ��
     */
    public void UpdateItemPool()
    {
        //��������������޵���Ʒ
        List<ItemData> delateTemp = new List<ItemData>();
        for (int i = 0; i < currentItemPool.Count; i++)
        {
            if (currentItemPool[i].generatedTime >= currentItemPool[i].maxGeneratedTime)
            {
                delateTemp.Add(currentItemPool[i]);
            }
        }
        for (int i = 0; i < delateTemp.Count; i++)
        {
            DelateItemFromPool(delateTemp[i]);
        }

        //������Ʒ���ɸ���
    }

    public void AddItemToPool(ItemData targetItem)
    {
        currentItemPool.Add(targetItem);
    }

    public void DelateItemFromPool(ItemData targetItem)
    {
        currentItemPool.Remove(targetItem);
    }
}
