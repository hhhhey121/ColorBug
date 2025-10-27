// 命名为 SceneLoader.cs
using UnityEngine;
using UnityEngine.SceneManagement; // 必须包含这个命名空间

public class SceneLoader : MonoBehaviour
{
    // 这是一个公开的方法，我们将把它链接到按钮的 OnClick 事件
    public void RestartCurrentScene()
    {
        // 获取当前活动场景的 build index（生成索引）
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 重新加载该场景
        SceneManager.LoadScene(currentSceneIndex);

        // --- 或者，你也可以使用场景名称 ---
        // string currentSceneName = SceneManager.GetActiveScene().name;
        // SceneManager.LoadScene(currentSceneName);
        // (使用 build index 通常更推荐)
    }

    // 你也可以在这个脚本里放其他独立的场景切换功能
    // 比如：
    // public void LoadMainMenu()
    // {
    //     SceneManager.LoadScene("MainMenu"); // 假设你的主菜单场景叫 "MainMenu"
    // }
}