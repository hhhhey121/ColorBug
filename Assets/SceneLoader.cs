// ����Ϊ SceneLoader.cs
using UnityEngine;
using UnityEngine.SceneManagement; // ���������������ռ�

public class SceneLoader : MonoBehaviour
{
    // ����һ�������ķ��������ǽ��������ӵ���ť�� OnClick �¼�
    public void RestartCurrentScene()
    {
        // ��ȡ��ǰ������� build index������������
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ���¼��ظó���
        SceneManager.LoadScene(currentSceneIndex);

        // --- ���ߣ���Ҳ����ʹ�ó������� ---
        // string currentSceneName = SceneManager.GetActiveScene().name;
        // SceneManager.LoadScene(currentSceneName);
        // (ʹ�� build index ͨ�����Ƽ�)
    }

    // ��Ҳ����������ű�������������ĳ����л�����
    // ���磺
    // public void LoadMainMenu()
    // {
    //     SceneManager.LoadScene("MainMenu"); // ����������˵������� "MainMenu"
    // }
}