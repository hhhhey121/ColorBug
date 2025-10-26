using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    private int coins = 0;

    [SerializeField] private Text coinText;//ʵ����
                                           
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))//����coin��ǩʱ
        {
            Destroy(collision.gameObject);//����coin
            coins++;
            coinText.text = "Coins:" + coins;
        }
    }
}
