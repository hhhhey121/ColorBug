using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private AudioSource finishSound;

    // ����������Unity��Inspector�����ñ�����Ҫ���ٽ��
    // ���磺��һ����Ϊ 1��֮��Ĺؿ���Ϊ 3
    public int requiredCoins = 1;

    // ��������(��ѡ) ��Ҳ���ʱ���ŵ���Ч
    public AudioClip lockedSound;

    void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �����顿���ʹ�ñ�ǩ(Tag)��������
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. ���Դ���ײ�������(collision.gameObject)���ϻ�ȡ Coin �ű�
            Coin playerCoinCollector = collision.gameObject.GetComponent<Coin>();

            if (playerCoinCollector != null)
            {
                // 2. ����������Ƿ���
                if (playerCoinCollector.GetCoinCount() >= requiredCoins)
                {
                    // ����㹻��ͨ��
                    finishSound.Play();
                    finishLevel();
                }
                else
                {
                    // ��Ҳ���
                    Debug.Log("��Ҳ���! ��Ҫ: " + requiredCoins + ", ��ǰ: " + playerCoinCollector.GetCoinCount());
                    if (lockedSound != null && !finishSound.isPlaying)
                    {
                        finishSound.PlayOneShot(lockedSound);
                    }
                }
            }
            else
            {
                Debug.LogError("���������û���ҵ� Coin.cs �ű�!");
            }
        }
    }

    private void finishLevel()//�ؿ�����
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
