// ����Ϊ SceneSwitcher.cs
using UnityEngine;
using UnityEngine.SceneManagement; // ���������������ռ�

public class SceneSwitcher : MonoBehaviour
{
    // �ؼ�������һ�������������洢Ŀ�곡��������
    // �����ֱ���� Unity Inspector �������������ֵ
    [Tooltip("Ҫ���ص�Ŀ�곡�������ƣ������� Build Settings �е�������ȫһ�£�")]
    public string targetSceneName;

    // ����һ�������ķ��������ǽ��������ӵ���ť�� OnClick �¼�
    public void LoadTargetScene()
    {
        // ����û��Ƿ������� Inspector ����������
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError("SceneSwitcher: δָ�� targetSceneName������ Inspector ������Ҫ���صĳ������ơ�");
            return;
        }

        // �������� Inspector ��ָ�����Ǹ�����
        SceneManager.LoadScene(targetSceneName);
    }
}