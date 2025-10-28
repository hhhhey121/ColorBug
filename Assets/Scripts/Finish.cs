using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class Finish : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip successSound;
    public int requiredCoins = 1;
    public AudioClip lockedSound;
    public string targetSceneName;

    // 【新增】添加一个布尔值，防止玩家在延迟期间重复触发通关
    private bool isLoadingLevel = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 【修改】增加一个条件 !isLoadingLevel，防止重复触发
        if (collision.gameObject.CompareTag("Player") && !isLoadingLevel)
        {
            Coin playerCoinCollector = collision.gameObject.GetComponent<Coin>();
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            if (playerCoinCollector != null && playerMovement != null)
            {
                bool coinsOK = playerCoinCollector.GetCoinCount() >= requiredCoins;
                bool mergedOK = !playerMovement.GetIsSeparated();

                if (coinsOK && mergedOK)
                {
                    // 【新增】立即将状态设为“正在加载”，防止再次触发
                    isLoadingLevel = true;

                    if (successSound != null)
                    {
                        audioSource.PlayOneShot(successSound);
                    }

                    // 【修改】不再直接调用 finishLevel()，
                    // 而是启动一个带有 1.5 秒延迟的协程
                    StartCoroutine(LoadNextSceneAfterDelay(1.5f));
                }
                else if (!coinsOK)
                {
                    Debug.Log("金币不足! 需要: " + requiredCoins + ", 当前: " + playerCoinCollector.GetCoinCount());
                    PlayLockedSound();
                }
                else if (!mergedOK)
                {
                    Debug.Log("角色未合并!");
                    PlayLockedSound();
                }
            }
            else
            {
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

    private void PlayLockedSound()
    {
        if (lockedSound != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(lockedSound);
        }
    }

    // 【修改】删除了原来的 finishLevel() 方法
    // 【新增】创建了一个新的协程方法
    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        // 1. 暂停执行，等待 'delay' 参数指定的秒数（这里是 1.5 秒）
        yield return new WaitForSeconds(delay);

        // 2. 等待结束后，执行原来的场景加载逻辑
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("通关失败：请在 Finish 脚本的 'Target Scene Name' 字段中设置要加载的场景名称！");

            // 【新增】如果出错，别忘了把状态改回来，以便玩家可以重试
            isLoadingLevel = false;
            yield break; // 退出协程
        }

        // 3. 加载新场景
        SceneManager.LoadScene(targetSceneName);
    }
}