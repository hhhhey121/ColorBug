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
    public static RespawnManager Instance; // 单例模式，方便全局访问

    // 在Inspector中设置当前的重生逻辑
    public RespawnMode currentMode = RespawnMode.Checkpoint;

    // ----- 重生点 -----
    // 模式1 (Checkpoint): 存储每个角色的最后一个存档点位置
    // 【修改】我们使用你的 PlayerLife 作为 Key
    private Dictionary<PlayerLife, Vector3> playerCheckpoints = new Dictionary<PlayerLife, Vector3>();

    // 模式2 (FarPoint): 那个“较远的刷新点”
    // 在Inspector中把它拖拽过来
    public Transform farRespawnPoint;

    // ----- 其他设置 -----
    public float respawnDelay = 3.0f; // 死亡后多久重生

    // ----- 角色引用 -----
    // 【修改】在Inspector中把你场景中的两个角色拖到这里
    public PlayerLife player1;
    public PlayerLife player2;


    void Awake()
    {
        // 设置单例
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        // 假设角色初始位置就是第一个“存档点”
        if (player1 != null)
        {
            playerCheckpoints[player1] = player1.transform.position;
        }
        if (player2 != null)
        {
            playerCheckpoints[player2] = player2.transform.position;
        }

        if (farRespawnPoint == null && currentMode == RespawnMode.FarPoint)
        {
            Debug.LogError("当前为FarPoint模式，但 'farRespawnPoint' 未在Inspector中设置！");
        }
    }

    // (公共方法) Checkpoint 脚本将调用此方法来更新存档点
    public void UpdateCheckpoint(Vector3 newCheckpointPosition)
    {
        Debug.Log("存档点更新为: " + newCheckpointPosition);

        // 更新所有已知角色的存档点
        if (player1 != null) playerCheckpoints[player1] = newCheckpointPosition;
        if (player2 != null) playerCheckpoints[player2] = newCheckpointPosition;
    }

    // (公共方法) PlayerLife 脚本调用此方法
    // 【修改】参数类型改为 PlayerLife
    public void HandlePlayerDeath(PlayerLife deadPlayer)
    {
        // 另一个角色不受影响
        StartCoroutine(RespawnCoroutine(deadPlayer));
    }

    // 【修改】参数类型改为 PlayerLife
    private IEnumerator RespawnCoroutine(PlayerLife playerToRespawn)
    {
        // 1. 等待延迟
        yield return new WaitForSeconds(respawnDelay);

        // 2. 决定重生位置
        Vector3 respawnPosition;

        if (currentMode == RespawnMode.FarPoint)
        {
            // 模式2：使用指定的远点
            respawnPosition = farRespawnPoint.position;
        }
        else // currentMode == RespawnMode.Checkpoint
        {
            // 模式1：使用该角色最后记录的存档点
            if (playerCheckpoints.ContainsKey(playerToRespawn))
            {
                respawnPosition = playerCheckpoints[playerToRespawn];
            }
            else
            {
                // 备用逻辑
                Debug.LogWarning("未找到 " + playerToRespawn.name + " 的存档点, 将在初始位置重生");
                // 找到 player1 或 player2 的初始位置...
                // 为了简单起见，我们就在它死亡的地方重生
                respawnPosition = playerToRespawn.transform.position;
            }
        }

        // 3. 调用角色自己的Respawn方法
        playerToRespawn.Respawn(respawnPosition);
    }
}
