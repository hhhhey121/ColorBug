using Cinemachine;//使用库
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    private void Start()
    {
        SwitchConfinerShape();
    }

    private void SwitchConfinerShape()
    {
        //寻找bounds，获取其身上的组件
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("Bounds").GetComponent<PolygonCollider2D>();
        //获取自身组件
        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
        //赋值
        confiner.m_BoundingShape2D = confinerShape;

        confiner.InvalidatePathCache();
    }
}

