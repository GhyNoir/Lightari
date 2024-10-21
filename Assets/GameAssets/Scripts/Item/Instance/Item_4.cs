using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  
Item Description:
略微提升玩家冲刺速度

Item info:
+------+------+-------+--------+-----+
| ID   | type | level | target | max |
+------+------+-------+--------+-----+
| 4    | buff | 2     | player | None|
+------+------+-------+--------+-----+
 */
[CreateAssetMenu(fileName = "Item_4", menuName = "Item/Buff/Item_4")]
public class Item_4 : ItemData
{
    public override void Apply()
    {
        PlayerControllor.instance.oriDashSpeed += 0.1f;
        if (PlayerControllor.instance.oriDashSpeed > 0.95f)
        {
            PlayerControllor.instance.oriDashSpeed = 0.95f;
        }
    }
    public override void Delete()
    {
        PlayerControllor.instance.dashSpeed -= 0.1f;
        if (PlayerControllor.instance.dashSpeed < 0.05f)
        {
            PlayerControllor.instance.dashSpeed = 0.05f;
        }
    }
}
