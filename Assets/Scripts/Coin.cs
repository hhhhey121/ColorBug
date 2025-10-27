using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    private int coins = 0;

    [SerializeField] private Text coinText;//实例化

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))//碰到coin标签时
        {
            Destroy(collision.gameObject);//销毁coin
            coins++;

            // 确保 coinText 已经被拖拽赋值
            if (coinText != null)
            {
                coinText.text = "" + coins;
            }
        }
    }

    // 【新增】
    // 添加一个公共方法 (public method)
    // 这样其他脚本 (比如 Finish.cs) 就可以调用它来获取金币数量
    public int GetCoinCount()
    {
        return coins;
    }
}