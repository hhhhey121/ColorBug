using UnityEngine;

public class ButtonPairManager : MonoBehaviour
{
    [Header("��ť")]
    public PressureButton button1; // ���� Button_A1
    public PressureButton button2; // ���� Button_A2

    [Header("����")]
    public GameObject laserPrefab; // ������� "LethalLaser" Ԥ����
    public Transform laserSpawnPoint1; // ���뼤�����ɵ�1
    public Transform laserSpawnPoint2; // ���뼤�����ɵ�2

    [Header("״̬ (ֻ��)")]
    [SerializeField] private bool puzzleCompleted = false;
    private bool lasersAreSpawned = false;
    private GameObject spawnedLaser1;
    private GameObject spawnedLaser2;

    void Update()
    {
        if (puzzleCompleted) return; // ���������

        // 1. ���������ť�Ƿ�ͬʱ����
        if (button1.isPressed && button2.isPressed)
        {
            // 2. ������⻹û�б����ɹ�������������
            if (!lasersAreSpawned)
            {
                SpawnLasers();
                lasersAreSpawned = true;
            }
        }

        // 3. (��ѡ) ��鼤���Ƿ����������
        //    ���ܷ�ֹ��Ҳ��°�ť -> �ƿ� -> �ٲ���ʱ���ظ�����
        if (lasersAreSpawned)
        {
            // ����������ⶼΪ null (���� PlayerLaserAbility ���ղ� Destroy ��)
            if (spawnedLaser1 == null && spawnedLaser2 == null)
            {
                Debug.Log("���� " + gameObject.name + " �����!");
                puzzleCompleted = true;

                // (��ѡ) �����ﲥ��һ����������ɡ�����Ч
            }
        }
    }

    private void SpawnLasers()
    {
        Debug.Log("���ɼ���!");
        spawnedLaser1 = Instantiate(laserPrefab, laserSpawnPoint1.position, laserSpawnPoint1.rotation);
        spawnedLaser2 = Instantiate(laserPrefab, laserSpawnPoint2.position, laserSpawnPoint2.rotation);
    }
}