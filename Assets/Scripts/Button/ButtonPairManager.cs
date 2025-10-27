using UnityEngine;

public class ButtonPairManager : MonoBehaviour
{
    [Header("按钮")]
    public PressureButton button1; // 拖入 Button_A1
    public PressureButton button2; // 拖入 Button_A2

    [Header("激光")]
    public GameObject laserPrefab; // 拖入你的 "LethalLaser" 预制体
    public Transform laserSpawnPoint1; // 拖入激光生成点1
    public Transform laserSpawnPoint2; // 拖入激光生成点2

    [Header("状态 (只读)")]
    [SerializeField] private bool puzzleCompleted = false;
    private bool lasersAreSpawned = false;
    private GameObject spawnedLaser1;
    private GameObject spawnedLaser2;

    void Update()
    {
        if (puzzleCompleted) return; // 谜题已完成

        // 1. 检查两个按钮是否被同时按下
        if (button1.isPressed && button2.isPressed)
        {
            // 2. 如果激光还没有被生成过，就生成它们
            if (!lasersAreSpawned)
            {
                SpawnLasers();
                lasersAreSpawned = true;
            }
        }

        // 3. (可选) 检查激光是否被玩家吸收了
        //    这能防止玩家踩下按钮 -> 移开 -> 再踩下时，重复生成
        if (lasersAreSpawned)
        {
            // 如果两个激光都为 null (即被 PlayerLaserAbility 吸收并 Destroy 了)
            if (spawnedLaser1 == null && spawnedLaser2 == null)
            {
                Debug.Log("谜题 " + gameObject.name + " 已完成!");
                puzzleCompleted = true;

                // (可选) 在这里播放一个“谜题完成”的音效
            }
        }
    }

    private void SpawnLasers()
    {
        Debug.Log("生成激光!");
        spawnedLaser1 = Instantiate(laserPrefab, laserSpawnPoint1.position, laserSpawnPoint1.rotation);
        spawnedLaser2 = Instantiate(laserPrefab, laserSpawnPoint2.position, laserSpawnPoint2.rotation);
    }
}