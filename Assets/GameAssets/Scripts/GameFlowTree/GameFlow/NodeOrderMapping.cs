using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Summary:
 * ���order��level���ֶ�Ӧʵ����ӳ���ϵ
 * ����˳��ƴ������ӳ��ֵ�õ�����order
 */
public class NodeOrderMapping
{
    Dictionary<int, string> levelMapping = new Dictionary<int, string>
    {
        { 0,"root node" },
        { 1,"game flow node" },
        { 2,"base node" },
        { 3,"content node" },
    };

    //Level : 2
    Dictionary<int, string> BaseNodeOrderMapping = new Dictionary<int, string>
    {
        { 000,"chase to battle node" },
        { 100,"battle node" },
        { 200,"reward node" },
        { 300,"store node" },
        { 400,"event node" },
        { 500,"planet node" },
        { 600,"level end node" },
    };

    //Level : 3
    Dictionary<int, string> BattleNodeOrderMapping = new Dictionary<int, string>
    {
        { 000,"base battle content node" },
        { 001,"check end content node" },
    };
}
