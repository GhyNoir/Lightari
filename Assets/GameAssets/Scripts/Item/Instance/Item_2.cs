using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  
Item Description:
略微提升玩家移动速度

Item info:
+------+------+-------+--------+-----+
| ID   | type | level | target | max |
+------+------+-------+--------+-----+
| 3    | buff | 3     | player | None|
+------+------+-------+--------+-----+
 */
[CreateAssetMenu(fileName = "Item_2", menuName = "Item/Buff/Item_2")]
public class Item_2 : ItemData
{
    public override void Apply()
    {
        PlayerControllor.instance.moveSpeed += 0.04f;
    }
    public override void Delete()
    {
        PlayerControllor.instance.moveSpeed -= 0.04f;
    }
}
