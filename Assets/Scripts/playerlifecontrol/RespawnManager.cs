using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ��������ģʽ
public enum RespawnMode
{
    Checkpoint, // ģʽ1����������һ���浵��
    FarPoint    // ģʽ2��������ָ����Զ��
}

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance; // ����

    // --- ��Inspector������ ---
    public RespawnMode currentMode = RespawnMode.Checkpoint;
    public Transform farRespawnPoint; // ��ġ���Զˢ�µ㡱
    public float respawnDelay = 2.0f; // �������� + �����ӳ�
    public PlayerLife player1; // ������Ľ�ɫ1
    public PlayerLife player2; // ������Ľ�ɫ2
    // ------------------------

    private Dictionary<PlayerLife, Vector3> playerCheckpoints = new Dictionary<PlayerLife, Vector3>();

    // ����������������͡���˸���Ĺؼ���
    private float playerZPosition = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // ����Ϸ��ʼʱ����¼�������ȷ��Z��
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

    // (Checkpoint �ű���������)
    public void UpdateCheckpoint(Vector3 newCheckpointPosition)
    {
        if (player1 != null) playerCheckpoints[player1] = newCheckpointPosition;
        if (player2 != null) playerCheckpoints[player2] = newCheckpointPosition;
    }

    // (PlayerLife �ű���������)
    public void HandlePlayerDeath(PlayerLife deadPlayer)
    {
        StartCoroutine(RespawnCoroutine(deadPlayer));
    }

    private IEnumerator RespawnCoroutine(PlayerLife playerToRespawn)
    {
        // 1. �ȴ��ӳ�
        yield return new WaitForSeconds(respawnDelay);

        // 2. ���������� (ֻ���� X, Y)
        Vector3 respawnPositionXY;
        if (currentMode == RespawnMode.FarPoint)
        {
            respawnPositionXY = farRespawnPoint.position;
        }
        else // Checkpoint ģʽ
        {
            respawnPositionXY = playerCheckpoints[playerToRespawn];
        }

        // 3. ���ؼ��޸���
        //    ʹ��������� X �� Y����ǿ��ʹ�����ǡ���ס���� playerZPosition
        Vector3 finalRespawnPosition = new Vector3(
            respawnPositionXY.x,
            respawnPositionXY.y,
            playerZPosition // ǿ��Z�ᣬ��������������͡���˸��
        );

        // 4. ���ý�ɫ�Լ�����������
        playerToRespawn.Respawn(finalRespawnPosition);
    }
}