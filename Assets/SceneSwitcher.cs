using UnityEngine;
using UnityEngine.SceneManagement; // 用于场景管理
using System.Collections; // 用于协程 (IEnumerator)

// 命名为 SceneSwitcher.cs
public class SceneSwitcher : MonoBehaviour
{
    [Header("UI & 场景设置")]
    [Tooltip("你要控制显示/隐藏/闪烁的Panel")]
    public GameObject targetPanel; // 拖入你要控制的Panel

    [Tooltip("第五次点击要加载的场景名称")]
    public string startSceneName = "StartScene"; // 默认设置为 "StartScene"，你可以在Inspector中修改

    [Header("连续点击逻辑")]
    [Tooltip("被视为“连续”点击的最大时间间隔（秒）")]
    public float maxTimeBetweenClicks = 1.0f; // 超过1秒再点，就重置计数

    [Tooltip("第2-4次点击时，Panel闪烁（关闭）的持续时间（秒）")]
    public float flashDuration = 0.1f; // 闪烁时黑屏的时间，0.1秒非常快

    // --- 私有变量 ---
    private int consecutiveClickCount = 0; // 连续点击的计数器
    private float lastClickTime = 0f; // 上次点击的时间
    private Coroutine currentFlashCoroutine = null; // 用于跟踪当前的闪烁协程

    /// <summary>
    /// 确保Panel在开始时是隐藏的
    /// </summary>
    void Start()
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("SceneSwitcher: 未指定 targetPanel！请在 Inspector 中拖入一个GameObject。");
        }

        if (string.IsNullOrEmpty(startSceneName))
        {
            Debug.LogError("SceneSwitcher: 未指定 startSceneName！请在 Inspector 中设置要加载的场景名称。");
        }
    }

    /// <summary>
    /// 这是你要链接到按钮 OnClick 事件的新方法
    /// </summary>
    public void HandlePanelClick()
    {
        // 检查是否为时已晚，如果两次点击间隔太长，则重置计数器
        // (我们允许第一次点击时 consecutiveClickCount 为 0)
        if (Time.time - lastClickTime > maxTimeBetweenClicks && consecutiveClickCount > 0)
        {
            Debug.Log("点击间隔超时，重置计数器。");
            consecutiveClickCount = 0;

            // 如果超时了，并且Panel是开着的，就关掉它
            if (targetPanel != null && targetPanel.activeSelf)
            {
                targetPanel.SetActive(false);
            }
        }

        // 更新点击时间
        lastClickTime = Time.time;
        // 增加计数器
        consecutiveClickCount++;

        Debug.Log($"连续点击第 {consecutiveClickCount} 次");

        // 如果之前有闪烁协程在跑，先停掉它，确保我们是从一个干净的状态开始
        if (currentFlashCoroutine != null)
        {
            StopCoroutine(currentFlashCoroutine);
            currentFlashCoroutine = null;
            // 确保Panel是可见的，以防它在闪烁中途被停止
            if (targetPanel != null) targetPanel.SetActive(true);
        }

        // 根据点击次数执行不同操作
        switch (consecutiveClickCount)
        {
            case 1:
                // 第一次点击：显示Panel
                if (targetPanel != null)
                {
                    targetPanel.SetActive(true);
                }
                break;

            case 2:
            case 3:
            case 4:
                // 第2、3、4次点击：闪烁Panel
                if (targetPanel != null && targetPanel.activeSelf) // 确保Panel是激活状态才闪烁
                {
                    // 启动协程来处理 "先关后开"
                    currentFlashCoroutine = StartCoroutine(FlashPanel());
                }
                else if (targetPanel != null && !targetPanel.activeSelf)
                {
                    // 如果Panel因为某些原因被关了，就把它打开（类似Case 1）
                    targetPanel.SetActive(true);
                }
                break;

            case 5:
                // 第五次点击：加载场景
                Debug.Log($"加载场景: {startSceneName}");
                if (!string.IsNullOrEmpty(startSceneName))
                {
                    SceneManager.LoadScene(startSceneName);
                }
                // (加载场景后这个脚本实例会被销毁，计数器自动重置)
                // 但为了严谨，我们还是在这里重置
                consecutiveClickCount = 0;
                break;

            default:
                // 理论上不应该到这里，但以防万一
                consecutiveClickCount = 0;
                break;
        }
    }

    /// <summary>
    /// 协程：用于处理Panel的“闪烁”效果
    /// (快速关闭 -> 等待 -> 重新打开)
    /// </summary>
    private IEnumerator FlashPanel()
    {
        // 1. 快速关闭
        targetPanel.SetActive(false);

        // 2. 等待一个极短的时间 (flashDuration)
        yield return new WaitForSeconds(flashDuration);

        // 3. 重新打开
        targetPanel.SetActive(true);

        // 协程结束，清空引用
        currentFlashCoroutine = null;
    }
}
