using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RespawnMode
{
    Checkpoint,
    FarPoint
}

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;
    public RespawnMode currentMode = RespawnMode.Checkpoint;

    private Dictionary<PlayerLife, Vector3> playerCheckpoints = new Dictionary<PlayerLife, Vector3>();
    public Transform farRespawnPoint;
    public float respawnDelay = 3.0f;

    public PlayerLife player1;
    public PlayerLife player2;

    // 【新增】用于存储玩家正确的Z轴位置
    private float playerZPosition = 0f;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        if (player1 != null)
        {
            playerCheckpoints[player1] = player1.transform.position;

            // 【关键】在游戏开始时，记录下玩家的原始Z轴
            // 我们假设两个玩家的Z轴应该是一样的
            playerZPosition = player1.transform.position.z;
        }
        if (player2 != null)
        {
            playerCheckpoints[player2] = player2.transform.position;
            // 如果 player1 为空，就用 player2 的Z轴
            if (player1 == null)
            {
                playerZPosition = player2.transform.position.z;
            }
        }

        if (farRespawnPoint == null && currentMode == RespawnMode.FarPoint)
        {
            Debug.LogError("当前为FarPoint模式，但 'farRespawnPoint' 未在Inspector中设置！");
        }

        Debug.Log("RespawnManager 初始化完成，玩家的Z轴锁定为: " + playerZPosition);
    }

    public void UpdateCheckpoint(Vector3 newCheckpointPosition)
    {
        Debug.Log("存档点更新为: " + newCheckpointPosition);

        // 更新存档点时，我们只关心 X 和 Y。
        // 但为了简单起见，我们还是在重生时统一修正Z轴。
        if (player1 != null) playerCheckpoints[player1] = newCheckpointPosition;
        if (player2 != null) playerCheckpoints[player2] = newCheckpointPosition;
    }

    public void HandlePlayerDeath(PlayerLife deadPlayer)
    {
        StartCoroutine(RespawnCoroutine(deadPlayer));
    }

    private IEnumerator RespawnCoroutine(PlayerLife playerToRespawn)
    {
        yield return new WaitForSeconds(respawnDelay);

        // 2. 决定重生位置 (只关心 X, Y)
        Vector3 respawnPositionXY; // 临时存储

        if (currentMode == RespawnMode.FarPoint)
        {
            respawnPositionXY = farRespawnPoint.position;
        }
        else // currentMode == RespawnMode.Checkpoint
        {
            if (playerCheckpoints.ContainsKey(playerToRespawn))
            {
                respawnPositionXY = playerCheckpoints[playerToRespawn];
            }
            else
            {
                Debug.LogWarning("未找到 " + playerToRespawn.name + " 的存档点, 将在初始位置重生");
                respawnPositionXY = (player1 == playerToRespawn ? player1.transform.position : player2.transform.position);
            }
        }

        // 3. 【关键修复】
        // 使用重生点的 X 和 Y，但强制使用我们“记住”的 playerZPosition
        Vector3 finalRespawnPosition = new Vector3(
            respawnPositionXY.x,
            respawnPositionXY.y,
            playerZPosition
        );

        // 4. 调用角色自己的Respawn方法
        playerToRespawn.Respawn(finalRespawnPosition);
    }
}