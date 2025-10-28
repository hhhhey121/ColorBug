using UnityEngine;
using UnityEngine.SceneManagement; // ���ڳ�������
using System.Collections; // ����Э�� (IEnumerator)

// ����Ϊ SceneSwitcher.cs
public class SceneSwitcher : MonoBehaviour
{
    [Header("UI & ��������")]
    [Tooltip("��Ҫ������ʾ/����/��˸��Panel")]
    public GameObject targetPanel; // ������Ҫ���Ƶ�Panel

    [Tooltip("����ε��Ҫ���صĳ�������")]
    public string startSceneName = "StartScene"; // Ĭ������Ϊ "StartScene"���������Inspector���޸�

    [Header("��������߼�")]
    [Tooltip("����Ϊ����������������ʱ�������룩")]
    public float maxTimeBetweenClicks = 1.0f; // ����1���ٵ㣬�����ü���

    [Tooltip("��2-4�ε��ʱ��Panel��˸���رգ��ĳ���ʱ�䣨�룩")]
    public float flashDuration = 0.1f; // ��˸ʱ������ʱ�䣬0.1��ǳ���

    // --- ˽�б��� ---
    private int consecutiveClickCount = 0; // ��������ļ�����
    private float lastClickTime = 0f; // �ϴε����ʱ��
    private Coroutine currentFlashCoroutine = null; // ���ڸ��ٵ�ǰ����˸Э��

    /// <summary>
    /// ȷ��Panel�ڿ�ʼʱ�����ص�
    /// </summary>
    void Start()
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("SceneSwitcher: δָ�� targetPanel������ Inspector ������һ��GameObject��");
        }

        if (string.IsNullOrEmpty(startSceneName))
        {
            Debug.LogError("SceneSwitcher: δָ�� startSceneName������ Inspector ������Ҫ���صĳ������ơ�");
        }
    }

    /// <summary>
    /// ������Ҫ���ӵ���ť OnClick �¼����·���
    /// </summary>
    public void HandlePanelClick()
    {
        // ����Ƿ�Ϊʱ����������ε�����̫���������ü�����
        // (���������һ�ε��ʱ consecutiveClickCount Ϊ 0)
        if (Time.time - lastClickTime > maxTimeBetweenClicks && consecutiveClickCount > 0)
        {
            Debug.Log("��������ʱ�����ü�������");
            consecutiveClickCount = 0;

            // �����ʱ�ˣ�����Panel�ǿ��ŵģ��͹ص���
            if (targetPanel != null && targetPanel.activeSelf)
            {
                targetPanel.SetActive(false);
            }
        }

        // ���µ��ʱ��
        lastClickTime = Time.time;
        // ���Ӽ�����
        consecutiveClickCount++;

        Debug.Log($"��������� {consecutiveClickCount} ��");

        // ���֮ǰ����˸Э�����ܣ���ͣ������ȷ�������Ǵ�һ���ɾ���״̬��ʼ
        if (currentFlashCoroutine != null)
        {
            StopCoroutine(currentFlashCoroutine);
            currentFlashCoroutine = null;
            // ȷ��Panel�ǿɼ��ģ��Է�������˸��;��ֹͣ
            if (targetPanel != null) targetPanel.SetActive(true);
        }

        // ���ݵ������ִ�в�ͬ����
        switch (consecutiveClickCount)
        {
            case 1:
                // ��һ�ε������ʾPanel
                if (targetPanel != null)
                {
                    targetPanel.SetActive(true);
                }
                break;

            case 2:
            case 3:
            case 4:
                // ��2��3��4�ε������˸Panel
                if (targetPanel != null && targetPanel.activeSelf) // ȷ��Panel�Ǽ���״̬����˸
                {
                    // ����Э�������� "�ȹغ�"
                    currentFlashCoroutine = StartCoroutine(FlashPanel());
                }
                else if (targetPanel != null && !targetPanel.activeSelf)
                {
                    // ���Panel��ΪĳЩԭ�򱻹��ˣ��Ͱ����򿪣�����Case 1��
                    targetPanel.SetActive(true);
                }
                break;

            case 5:
                // ����ε�������س���
                Debug.Log($"���س���: {startSceneName}");
                if (!string.IsNullOrEmpty(startSceneName))
                {
                    SceneManager.LoadScene(startSceneName);
                }
                // (���س���������ű�ʵ���ᱻ���٣��������Զ�����)
                // ��Ϊ���Ͻ������ǻ�������������
                consecutiveClickCount = 0;
                break;

            default:
                // �����ϲ�Ӧ�õ�������Է���һ
                consecutiveClickCount = 0;
                break;
        }
    }

    /// <summary>
    /// Э�̣����ڴ���Panel�ġ���˸��Ч��
    /// (���ٹر� -> �ȴ� -> ���´�)
    /// </summary>
    private IEnumerator FlashPanel()
    {
        // 1. ���ٹر�
        targetPanel.SetActive(false);

        // 2. �ȴ�һ�����̵�ʱ�� (flashDuration)
        yield return new WaitForSeconds(flashDuration);

        // 3. ���´�
        targetPanel.SetActive(true);

        // Э�̽������������
        currentFlashCoroutine = null;
    }
}
