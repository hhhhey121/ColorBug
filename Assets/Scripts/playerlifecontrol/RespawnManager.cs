using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 定义重生模式
public enum RespawnMode
{
    Checkpoint, // 模式1：重生到上一个存档点
    FarPoint    // 模式2：重生到指定的远点
}

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance; // 单例

    // --- 在Inspector中设置 ---
    public RespawnMode currentMode = RespawnMode.Checkpoint;
    public Transform farRespawnPoint; // 你的“较远刷新点”
    public float respawnDelay = 2.0f; // 死亡动画 + 重生延迟
    public PlayerLife player1; // 拖入你的角色1
    public PlayerLife player2; // 拖入你的角色2
    // ------------------------

    private Dictionary<PlayerLife, Vector3> playerCheckpoints = new Dictionary<PlayerLife, Vector3>();

    // 【解决“看不见”和“闪烁”的关键】
    private float playerZPosition = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 在游戏开始时，记录下玩家正确的Z轴
        if (player1 != null)
        {
            playerCheckpoints[player1] = player1.transform.position;
            playerZPosition = player1.transform.position.z;
        }
        if (player2 != null)
        {
            playerCheckpoints[player2] = player2.transform.position;
            if (player1 == null) playerZPosition = player2.transform.position.z;
        }
    }

    // (Checkpoint 脚本会调用这个)
    public void UpdateCheckpoint(Vector3 newCheckpointPosition)
    {
        if (player1 != null) playerCheckpoints[player1] = newCheckpointPosition;
        if (player2 != null) playerCheckpoints[player2] = newCheckpointPosition;
    }

    // (PlayerLife 脚本会调用这个)
    public void HandlePlayerDeath(PlayerLife deadPlayer)
    {
        StartCoroutine(RespawnCoroutine(deadPlayer));
    }

    private IEnumerator RespawnCoroutine(PlayerLife playerToRespawn)
    {
        // 1. 等待延迟
        yield return new WaitForSeconds(respawnDelay);

        // 2. 决定重生点 (只关心 X, Y)
        Vector3 respawnPositionXY;
        if (currentMode == RespawnMode.FarPoint)
        {
            respawnPositionXY = farRespawnPoint.position;
        }
        else // Checkpoint 模式
        {
            respawnPositionXY = playerCheckpoints[playerToRespawn];
        }

        // 3. 【关键修复】
        //    使用重生点的 X 和 Y，但强制使用我们“记住”的 playerZPosition
        Vector3 finalRespawnPosition = new Vector3(
            respawnPositionXY.x,
            respawnPositionXY.y,
            playerZPosition // 强制Z轴，解决“看不见”和“闪烁”
        );

        // 4. 调用角色自己的重生方法
        playerToRespawn.Respawn(finalRespawnPosition);
    }
}