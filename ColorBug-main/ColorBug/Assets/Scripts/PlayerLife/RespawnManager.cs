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
    public static RespawnManager Instance; // ����ģʽ������ȫ�ַ���

    // ��Inspector�����õ�ǰ�������߼�
    public RespawnMode currentMode = RespawnMode.Checkpoint;

    // ----- ������ -----
    // ģʽ1 (Checkpoint): �洢ÿ����ɫ�����һ���浵��λ��
    // ���޸ġ�����ʹ����� PlayerLife ��Ϊ Key
    private Dictionary<PlayerLife, Vector3> playerCheckpoints = new Dictionary<PlayerLife, Vector3>();

    // ģʽ2 (FarPoint): �Ǹ�����Զ��ˢ�µ㡱
    // ��Inspector�а�����ק����
    public Transform farRespawnPoint;

    // ----- �������� -----
    public float respawnDelay = 3.0f; // ������������

    // ----- ��ɫ���� -----
    // ���޸ġ���Inspector�а��㳡���е�������ɫ�ϵ�����
    public PlayerLife player1;
    public PlayerLife player2;


    void Awake()
    {
        // ���õ���
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        // �����ɫ��ʼλ�þ��ǵ�һ�����浵�㡱
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
            Debug.LogError("��ǰΪFarPointģʽ���� 'farRespawnPoint' δ��Inspector�����ã�");
        }
    }

    // (��������) Checkpoint �ű������ô˷��������´浵��
    public void UpdateCheckpoint(Vector3 newCheckpointPosition)
    {
        Debug.Log("�浵�����Ϊ: " + newCheckpointPosition);

        // ����������֪��ɫ�Ĵ浵��
        if (player1 != null) playerCheckpoints[player1] = newCheckpointPosition;
        if (player2 != null) playerCheckpoints[player2] = newCheckpointPosition;
    }

    // (��������) PlayerLife �ű����ô˷���
    // ���޸ġ��������͸�Ϊ PlayerLife
    public void HandlePlayerDeath(PlayerLife deadPlayer)
    {
        // ��һ����ɫ����Ӱ��
        StartCoroutine(RespawnCoroutine(deadPlayer));
    }

    // ���޸ġ��������͸�Ϊ PlayerLife
    private IEnumerator RespawnCoroutine(PlayerLife playerToRespawn)
    {
        // 1. �ȴ��ӳ�
        yield return new WaitForSeconds(respawnDelay);

        // 2. ��������λ��
        Vector3 respawnPosition;

        if (currentMode == RespawnMode.FarPoint)
        {
            // ģʽ2��ʹ��ָ����Զ��
            respawnPosition = farRespawnPoint.position;
        }
        else // currentMode == RespawnMode.Checkpoint
        {
            // ģʽ1��ʹ�øý�ɫ����¼�Ĵ浵��
            if (playerCheckpoints.ContainsKey(playerToRespawn))
            {
                respawnPosition = playerCheckpoints[playerToRespawn];
            }
            else
            {
                // �����߼�
                Debug.LogWarning("δ�ҵ� " + playerToRespawn.name + " �Ĵ浵��, ���ڳ�ʼλ������");
                // �ҵ� player1 �� player2 �ĳ�ʼλ��...
                // Ϊ�˼���������Ǿ����������ĵط�����
                respawnPosition = playerToRespawn.transform.position;
            }
        }

        // 3. ���ý�ɫ�Լ���Respawn����
        playerToRespawn.Respawn(respawnPosition);
    }
}
