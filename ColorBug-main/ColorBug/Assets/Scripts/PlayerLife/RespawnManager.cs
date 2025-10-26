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

    // �����������ڴ洢�����ȷ��Z��λ��
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

            // ���ؼ�������Ϸ��ʼʱ����¼����ҵ�ԭʼZ��
            // ���Ǽ���������ҵ�Z��Ӧ����һ����
            playerZPosition = player1.transform.position.z;
        }
        if (player2 != null)
        {
            playerCheckpoints[player2] = player2.transform.position;
            // ��� player1 Ϊ�գ����� player2 ��Z��
            if (player1 == null)
            {
                playerZPosition = player2.transform.position.z;
            }
        }

        if (farRespawnPoint == null && currentMode == RespawnMode.FarPoint)
        {
            Debug.LogError("��ǰΪFarPointģʽ���� 'farRespawnPoint' δ��Inspector�����ã�");
        }

        Debug.Log("RespawnManager ��ʼ����ɣ���ҵ�Z������Ϊ: " + playerZPosition);
    }

    public void UpdateCheckpoint(Vector3 newCheckpointPosition)
    {
        Debug.Log("�浵�����Ϊ: " + newCheckpointPosition);

        // ���´浵��ʱ������ֻ���� X �� Y��
        // ��Ϊ�˼���������ǻ���������ʱͳһ����Z�ᡣ
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

        // 2. ��������λ�� (ֻ���� X, Y)
        Vector3 respawnPositionXY; // ��ʱ�洢

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
                Debug.LogWarning("δ�ҵ� " + playerToRespawn.name + " �Ĵ浵��, ���ڳ�ʼλ������");
                respawnPositionXY = (player1 == playerToRespawn ? player1.transform.position : player2.transform.position);
            }
        }

        // 3. ���ؼ��޸���
        // ʹ��������� X �� Y����ǿ��ʹ�����ǡ���ס���� playerZPosition
        Vector3 finalRespawnPosition = new Vector3(
            respawnPositionXY.x,
            respawnPositionXY.y,
            playerZPosition
        );

        // 4. ���ý�ɫ�Լ���Respawn����
        playerToRespawn.Respawn(finalRespawnPosition);
    }
}