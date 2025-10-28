using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private AudioSource finishSound;

    // 在Unity的Inspector中设置本关需要多少金币
    public int requiredCoins = 1;

    // (可选) 金币不够时播放的音效
    public AudioClip lockedSound;

    // 【修改点 1】
    // 在Unity Inspector中设置通关后要加载的场景名称
    // 比如："Level_2", "VictoryScene", 或者 "MainMenu"
    public string targetSceneName;


    void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 建议使用标签(Tag)来检查玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. 尝试从碰撞到的玩家(collision.gameObject)身上获取 Coin 脚本
            Coin playerCoinCollector = collision.gameObject.GetComponent<Coin>();
            // 获取 PlayerMovement 脚本
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerCoinCollector != null && playerMovement != null)
            {
                bool coinsOK = playerCoinCollector.GetCoinCount() >= requiredCoins;
                // "合并" 意味着 "isSeparated" 为 false
                bool mergedOK = !playerMovement.GetIsSeparated();

                if (coinsOK && mergedOK)
                {
                    // 条件 1: 金币足够
                    // 条件 2: 角色已合并
                    finishSound.Play();
                    finishLevel();
                }
                // 【逻辑修正】
                // 将金币检查和合并检查分开，使其能正确触发
                else if (!coinsOK)
                {
                    // 失败原因 1: 金币不够
                    Debug.Log("金币不足! 需要: " + requiredCoins + ", 当前: " + playerCoinCollector.GetCoinCount());
                    PlayLockedSound();
                }
                else if (!mergedOK)
                {
                    // 失败原因 2: 金币够了，但角色未合并
                    Debug.Log("角色未合并!");
                    PlayLockedSound();
                }
            }
            else
            {
                // 脚本缺失的错误处理
                if (playerCoinCollector == null)
                {
                    Debug.LogError("在玩家身上没有找到 Coin.cs 脚本!");
                }
                if (playerMovement == null)
                {
                    Debug.LogError("在玩家身上没有找到 PlayerMovement.cs 脚本!");
                }
            }
        }
    }

    // (辅助方法，避免重复代码)
    private void PlayLockedSound()
    {
        if (lockedSound != null && !finishSound.isPlaying)
        {
            finishSound.PlayOneShot(lockedSound);
        }
    }

    private void finishLevel() //关卡结束
    {
        // 【修改点 2】
        // 检查是否设置了目标场景名称
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("通关失败：请在 Finish 脚本的 'Target Scene Name' 字段中设置要加载的场景名称！");
            return; // 阻止加载，因为不知道要去哪个场景
        }

        // 加载在 Inspector 中指定的场景
        SceneManager.LoadScene(targetSceneName);
    }
}