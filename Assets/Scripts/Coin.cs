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

            // ȷ�� coinText �Ѿ�����ק��ֵ
            if (coinText != null)
            {
                coinText.text = "" + coins;
            }
        }
    }

    // ��������
    // ���һ���������� (public method)
    // ���������ű� (���� Finish.cs) �Ϳ��Ե���������ȡ�������
    public int GetCoinCount()
    {
        return coins;
    }
}