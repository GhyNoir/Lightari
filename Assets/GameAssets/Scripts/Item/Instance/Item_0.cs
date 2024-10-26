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
| 0    | buff | 1     | player | None|
+------+------+-------+--------+-----+
 */
[CreateAssetMenu(fileName = "Item_0", menuName = "Item/Buff/Item_0")]
public class Item_0 : ItemData
{
    public override void Apply()
    {
        PlayerControllor.instance.moveSpeed += 0.01f;
    }
    public override void Delete()
    {
        PlayerControllor.instance.moveSpeed -= 0.01f;
    }
}
