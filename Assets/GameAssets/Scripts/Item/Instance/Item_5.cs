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
| 5    | buff | 3     | player | None|
+------+------+-------+--------+-----+
 */
[CreateAssetMenu(fileName = "Item_5", menuName = "Item/Buff/Item_5")]
public class Item_5 : ItemData
{
    public override void Apply()
    {
        PlayerControllor.instance.oriDashSpeed += 0.2f;
        if(PlayerControllor.instance.oriDashSpeed > 0.95f)
        {
            PlayerControllor.instance.oriDashSpeed = 0.95f;
        }
    }
    public override void Delete()
    {
        PlayerControllor.instance.dashSpeed -= 0.2f;
        if (PlayerControllor.instance.dashSpeed < 0.05f)
        {
            PlayerControllor.instance.dashSpeed = 0.05f;
        }
    }
}
