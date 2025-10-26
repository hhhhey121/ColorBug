using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private AudioSource finishSound;

    // 【新增】在Unity的Inspector中设置本关需要多少金币
    // 比如：第一关设为 1，之后的关卡设为 3
    public int requiredCoins = 1;

    // 【新增】(可选) 金币不够时播放的音效
    public AudioClip lockedSound;

    void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 【建议】最好使用标签(Tag)来检查玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. 尝试从碰撞到的玩家(collision.gameObject)身上获取 Coin 脚本
            Coin playerCoinCollector = collision.gameObject.GetComponent<Coin>();

            if (playerCoinCollector != null)
            {
                // 2. 检查金币数量是否达标
                if (playerCoinCollector.GetCoinCount() >= requiredCoins)
                {
                    // 金币足够，通关
                    finishSound.Play();
                    finishLevel();
                }
                else
                {
                    // 金币不够
                    Debug.Log("金币不足! 需要: " + requiredCoins + ", 当前: " + playerCoinCollector.GetCoinCount());
                    if (lockedSound != null && !finishSound.isPlaying)
                    {
                        finishSound.PlayOneShot(lockedSound);
                    }
                }
            }
            else
            {
                Debug.LogError("在玩家身上没有找到 Coin.cs 脚本!");
            }
        }
    }

    private void finishLevel()//关卡结束
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
