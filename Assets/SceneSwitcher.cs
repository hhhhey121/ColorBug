// 命名为 SceneSwitcher.cs
using UnityEngine;
using UnityEngine.SceneManagement; // 必须包含这个命名空间

public class SceneSwitcher : MonoBehaviour
{
    // 关键：创建一个公共变量来存储目标场景的名称
    // 你可以直接在 Unity Inspector 窗口中设置这个值
    [Tooltip("要加载的目标场景的名称（必须与 Build Settings 中的名称完全一致）")]
    public string targetSceneName;

    // 这是一个公开的方法，我们将把它链接到按钮的 OnClick 事件
    public void LoadTargetScene()
    {
        // 检查用户是否忘记在 Inspector 中设置名称
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("SceneSwitcher: 未指定 targetSceneName！请在 Inspector 中设置要加载的场景名称。");
            return;
        }

        // 加载你在 Inspector 中指定的那个场景
        SceneManager.LoadScene(targetSceneName);
    }
}