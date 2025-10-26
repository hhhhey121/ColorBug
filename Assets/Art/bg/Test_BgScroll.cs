using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_BgScroll : MonoBehaviour
{
    [Tooltip("�ƶ��ٶ�"), Range(0.01f, 1f)]
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
    /// ����ͼƬ���ظ�����
    /// </summary>
    public void BgScroll()
    {
        //ͼƬģʽ������Ϊrepeat��ͨ�������޸Ĳ����е�offset����ʵ�ֹ���
        render.material.mainTextureOffset += new Vector2(moveSpeed * Time.deltaTime, 0);
    }
}

