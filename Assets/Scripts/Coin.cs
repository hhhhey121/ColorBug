using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // ���Ѹ��ġ� ���� TextMeshPro �����ռ�

public class Coin : MonoBehaviour
{
    private int coins = 0;

    // ���Ѹ��ġ� �� Text ���͸�Ϊ TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI coinText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin")) //����coin��ǩʱ
        {
            Destroy(collision.gameObject); //����coin
            coins++;

            // ȷ�� coinText �Ѿ�����ק��ֵ
            if (coinText != null)
            {
                // �����Ż��� ʹ�� .ToString() ���淶���� "" + coins Ҳ����
                coinText.text = coins.ToString();
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