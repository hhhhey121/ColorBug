using UnityEngine;

// 这是一个简单的全局管理器脚本
public class GameManager : MonoBehaviour
{
    // 【新增】一个静态实例，确保这个管理器是单例的
    public static GameManager Instance;

    private void Awake()
    {
        // 设置一个简单的单例模式
        // 这能确保在加载新场景时，这个对象不会被销毁
        // 并且始终只有一个 GameManager 实例
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 让这个对象在切换场景时不被销毁
        }
        else
        {
            // 如果已经存在一个 GameManager，就把这个新的销毁掉
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 检查玩家是否按下了 "Escape" 键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 调用退出功能
            QuitGame();
        }
    }

    // 封装一个公共的退出方法，这样其他按钮（比如主菜单的退出按钮）也可以调用它
    public void QuitGame()
    {
        Debug.Log("正在退出游戏..."); // 在控制台打印日志

        // Application.Quit() 在 Unity 编辑器中不起作用
        // 所以我们使用条件编译来处理编辑器和打包后的游戏
#if UNITY_EDITOR
        // 如果在 Unity 编辑器中，则停止播放
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 如果是打包后的游戏 (PC, Mac, Linux), 则退出应用程序
        Application.Quit();
#endif
    }
}
