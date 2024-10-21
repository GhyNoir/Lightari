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
| 2    | buff | 2     | player | None|
+------+------+-------+--------+-----+
 */
[CreateAssetMenu(fileName = "Item_1", menuName = "Item/Buff/Item_1")]
public class Item_1 : ItemData
{
    public override void Apply()
    {
        PlayerControllor.instance.moveSpeed += 0.02f;
    }
    public override void Delete()
    {
        PlayerControllor.instance.moveSpeed -= 0.02f;
        if(PlayerControllor.instance.moveSpeed <= 0)
        {
            PlayerControllor.instance.moveSpeed = 0.001f;
        }
    }
}
