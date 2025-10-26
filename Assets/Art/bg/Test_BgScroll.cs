using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_BgScroll : MonoBehaviour
{
    [Tooltip("移动速度"), Range(0.01f, 1f)]
    public float moveSpeed;
    private SpriteRenderer render;
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        BgScroll();
    }
    /// <summary>
    /// 单张图片的重复滚动
    /// </summary>
    public void BgScroll()
    {
        //图片模式已设置为repeat，通过代码修改材质中的offset即可实现滚动
        render.material.mainTextureOffset += new Vector2(moveSpeed * Time.deltaTime, 0);
    }
}

