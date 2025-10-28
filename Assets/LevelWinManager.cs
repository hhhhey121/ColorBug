using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWinManager : MonoBehaviour
{
    public static LevelWinManager Instance; // 单例

    [Header("关卡设置")]
    // 【重要】在Inspector中设置本关需要多少金币
    public int requiredCoins = 1;

    // 【重要】在Inspector中设置需要死亡几次
    public int requiredDeaths = 3;

    [Header("状态 (只读)")]
    [SerializeField] private int currentDeathCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // PlayerLife 脚本将在死亡时调用这个
    public void NotifyDeathOnFinalSpike(Coin playerCoinCollector)
    {
        // 1. 检查金币是否足够
        if (playerCoinCollector == null)
        {
            Debug.LogError("WinManager 无法在玩家身上找到 Coin.cs 脚本!");
            return;
        }

        if (playerCoinCollector.GetCoinCount() >= requiredCoins)
        {
            // 金币够了，开始计算死亡次数
            currentDeathCount++;
            Debug.Log("在最终地刺上死亡! 死亡次数: " + currentDeathCount + " / " + requiredDeaths);

            if (currentDeathCount >= requiredDeaths)
            {
                // 条件达成，通关！
                Debug.Log("通关! 死亡次数达标。");
                CompleteLevel();
            }
        }
        else
        {
            // 金币不够，死亡（但不计入次数）
            Debug.Log("在最终地刺上死亡，但金币不足! (需要 " + requiredCoins + ")");
        }
    }

    private void CompleteLevel()
    {
        // (可选) 在这里播放通关音效

        // 加载下一关 (逻辑和你 Finish.cs 脚本中的一样)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}