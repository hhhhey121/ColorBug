using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWinManager : MonoBehaviour
{
    public static LevelWinManager Instance; // ����

    [Header("�ؿ�����")]
    // ����Ҫ����Inspector�����ñ�����Ҫ���ٽ��
    public int requiredCoins = 1;

    // ����Ҫ����Inspector��������Ҫ��������
    public int requiredDeaths = 3;

    [Header("״̬ (ֻ��)")]
    [SerializeField] private int currentDeathCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // PlayerLife �ű���������ʱ�������
    public void NotifyDeathOnFinalSpike(Coin playerCoinCollector)
    {
        // 1. ������Ƿ��㹻
        if (playerCoinCollector == null)
        {
            Debug.LogError("WinManager �޷�����������ҵ� Coin.cs �ű�!");
            return;
        }

        if (playerCoinCollector.GetCoinCount() >= requiredCoins)
        {
            // ��ҹ��ˣ���ʼ������������
            currentDeathCount++;
            Debug.Log("�����յش�������! ��������: " + currentDeathCount + " / " + requiredDeaths);

            if (currentDeathCount >= requiredDeaths)
            {
                // ������ɣ�ͨ�أ�
                Debug.Log("ͨ��! ����������ꡣ");
                CompleteLevel();
            }
        }
        else
        {
            // ��Ҳ������������������������
            Debug.Log("�����յش�������������Ҳ���! (��Ҫ " + requiredCoins + ")");
        }
    }

    private void CompleteLevel()
    {
        // (��ѡ) �����ﲥ��ͨ����Ч

        // ������һ�� (�߼����� Finish.cs �ű��е�һ��)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}