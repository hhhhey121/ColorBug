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
            coinText.text = "Coins:" + coins;
        }
    }
}
