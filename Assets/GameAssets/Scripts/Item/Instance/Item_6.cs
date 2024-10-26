using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  
Item Description:
提升一点最大反弹次数

Item info:
+------+------+-------+--------+-----+
| ID   | type | level | target | max |
+------+------+-------+--------+-----+
| 6    | buff | 3     | player | None|
+------+------+-------+--------+-----+
 */
[CreateAssetMenu(fileName = "Item_6", menuName = "Item/Buff/Item_6")]
public class Item_6 : ItemData
{
    public override void Apply()
    {
        PlayerControllor.instance.maxReflectTime += 1;
    }
    public override void Delete()
    {
        PlayerControllor.instance.maxReflectTime -= 1;
    }
}
