using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 【已更改】 引入 TextMeshPro 命名空间

public class Coin : MonoBehaviour
{
    private int coins = 0;

    // 【已更改】 将 Text 类型改为 TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI coinText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin")) //碰到coin标签时
        {
            Destroy(collision.gameObject); //销毁coin
            coins++;

            // 确保 coinText 已经被拖拽赋值
            if (coinText != null)
            {
                // 【已优化】 使用 .ToString() 更规范，但 "" + coins 也能用
                coinText.text = coins.ToString();
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