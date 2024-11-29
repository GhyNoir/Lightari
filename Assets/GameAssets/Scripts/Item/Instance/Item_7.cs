using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  
Item Description:
允许玩家发射散射粒子

Item info:
+------+------+-------+--------+-----+
| ID   | type | level | target | max |
+------+------+-------+--------+-----+
| 7    | buff | 4     | player | None|
+------+------+-------+--------+-----+
 */
[CreateAssetMenu(fileName = "Item_7", menuName = "Item/Buff/Item_7")]
public class Item_7 : ItemData
{
    public override void Apply()
    {
        PlayerControllor.instance.sparkNum += 1;
    }
    public override void Delete()
    {
        PlayerControllor.instance.sparkNum -= 1;
    }
}
