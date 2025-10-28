using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // ���Ѹ��ġ� ���� TextMeshPro �����ռ�

public class Coin : MonoBehaviour
{
    // ��������
    // [SerializeField] �������˽�б�����ʾ�� Inspector ��
    [Header("�������")] // ���һ������
    [SerializeField] private int initialCoins = 0; // ���������ó�ʼ�������

    // ���޸ġ�
    // 'coins' �������ڻ��� Start() �б� 'initialCoins' ��ʼ��
    private int coins;

    [SerializeField] private TextMeshProUGUI coinText;

    // ��������
    // ��� Start() ������Ӧ�ó�ʼֵ
    void Start()
    {
        // 1. ����ǰ���������Ϊ���� Inspector ������ĳ�ʼֵ
        coins = initialCoins;

        // 2. ��������һ��UI������ʾ��ʼ�����
        UpdateCoinText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin")) //����coin��ǩʱ
        {
            Destroy(collision.gameObject); //��_hui_coin
            coins++;

            // 3. ���ø���UI�ķ���
            UpdateCoinText();
        }
    }

    // ��������
    // �������ı����߼���ȡ��һ�������ķ�����
    // ���� Start() �� OnTriggerEnter2D() �����Ե�����
    private void UpdateCoinText()
    {
        // ȷ�� coinText �Ѿ�����ק��ֵ
        if (coinText != null)
        {
            // �����Ż��� ʹ�� .ToString() ���淶
            coinText.text = coins.ToString();
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
