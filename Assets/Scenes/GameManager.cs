using UnityEngine;

// ����һ���򵥵�ȫ�ֹ������ű�
public class GameManager : MonoBehaviour
{
    // ��������һ����̬ʵ����ȷ������������ǵ�����
    public static GameManager Instance;

    private void Awake()
    {
        // ����һ���򵥵ĵ���ģʽ
        // ����ȷ���ڼ����³���ʱ��������󲻻ᱻ����
        // ����ʼ��ֻ��һ�� GameManager ʵ��
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ������������л�����ʱ��������
        }
        else
        {
            // ����Ѿ�����һ�� GameManager���Ͱ�����µ����ٵ�
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �������Ƿ����� "Escape" ��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �����˳�����
            QuitGame();
        }
    }

    // ��װһ���������˳�����������������ť���������˵����˳���ť��Ҳ���Ե�����
    public void QuitGame()
    {
        Debug.Log("�����˳���Ϸ..."); // �ڿ���̨��ӡ��־

        // Application.Quit() �� Unity �༭���в�������
        // ��������ʹ����������������༭���ʹ�������Ϸ
#if UNITY_EDITOR
        // ����� Unity �༭���У���ֹͣ����
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ����Ǵ�������Ϸ (PC, Mac, Linux), ���˳�Ӧ�ó���
        Application.Quit();
#endif
    }
}
