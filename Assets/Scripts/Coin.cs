using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 【已更改】 引入 TextMeshPro 命名空间

public class Coin : MonoBehaviour
{
    // 【新增】
    // [SerializeField] 会让这个私有变量显示在 Inspector 中
    [Header("金币设置")] // 添加一个标题
    [SerializeField] private int initialCoins = 0; // 在这里设置初始金币数量

    // 【修改】
    // 'coins' 变量现在会在 Start() 中被 'initialCoins' 初始化
    private int coins;

    [SerializeField] private TextMeshProUGUI coinText;

    // 【新增】
    // 添加 Start() 方法来应用初始值
    void Start()
    {
        // 1. 将当前金币数设置为你在 Inspector 中输入的初始值
        coins = initialCoins;

        // 2. 立即更新一次UI，以显示初始金币数
        UpdateCoinText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin")) //碰到coin标签时
        {
            Destroy(collision.gameObject); //销_hui_coin
            coins++;

            // 3. 调用更新UI的方法
            UpdateCoinText();
        }
    }

    // 【新增】
    // 将更新文本的逻辑提取到一个单独的方法中
    // 这样 Start() 和 OnTriggerEnter2D() 都可以调用它
    private void UpdateCoinText()
    {
        // 确保 coinText 已经被拖拽赋值
        if (coinText != null)
        {
            // 【已优化】 使用 .ToString() 更规范
            coinText.text = coins.ToString();
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
