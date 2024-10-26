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
| 3    | buff | 1     | player | None|
+------+------+-------+--------+-----+
 */
[CreateAssetMenu(fileName = "Item_3", menuName = "Item/Buff/Item_3")]
public class Item_3 : ItemData
{
    public override void Apply()
    {
        PlayerControllor.instance.oriDashSpeed += 0.02f;
        if (PlayerControllor.instance.oriDashSpeed > 0.95f)
        {
            PlayerControllor.instance.oriDashSpeed = 0.95f;
        }
    }
    public override void Delete()
    {
        PlayerControllor.instance.dashSpeed -= 0.02f;
        if (PlayerControllor.instance.dashSpeed < 0.05f)
        {
            PlayerControllor.instance.dashSpeed = 0.05f;
        }
    }
}
