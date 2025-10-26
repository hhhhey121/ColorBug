using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ���Ƴ���������Ҫ SceneManager
// using UnityEngine.SceneManagement; 

public class PlayerLife : MonoBehaviour
{
    // ����Ҫ��������������Ч "Ԥ����" (Prefab) �ϵ�����
    public GameObject playerDeathPS_Prefab;

    private Animator anim;
    private Rigidbody2D rb;
    private RespawnManager respawnManager; // ���¡�����������

    // ���¡�����״̬����Ϊ public���� PlayerMovement �ű����Զ�ȡ
    public bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // ���¡��Զ����ҹ�����
        respawnManager = FindObjectOfType<RespawnManager>();
        if (respawnManager == null)
        {
            Debug.LogError("������δ�ҵ� RespawnManager!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���¡�����Ѿ����ˣ���Ҫ�ظ�����
        if (isDead) return;

        if (collision.gameObject.CompareTag("Trap"))
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true; // 1. ���Ϊ����
        rb.bodyType = RigidbodyType2D.Static; // 2. ��������߼������徲ֹ
        anim.SetTrigger("death"); // 3. ������������

        // 4. ���޸�����ԭ���� Destroy(playerPS) �Ǵ����
        //    ����Ӧ�á����ɡ���Ч�������ǡ����١�
        if (playerDeathPS_Prefab != null)
        {
            GameObject ps = Instantiate(playerDeathPS_Prefab, transform.position, Quaternion.identity);
            Destroy(ps, 2f); // 2���������� *��Ч*
        }

        // 5. ���¡�֪ͨ������
        if (respawnManager != null)
        {
            respawnManager.HandlePlayerDeath(this);
        }
    }

    // ���Ƴ��� private void Restart() { ... }
    // �����������������������⣬�����Ƴ���

    // ���¡��������� (����������� RespawnManager ����)
    public void Respawn(Vector3 respawnPosition)
    {
        Debug.Log(gameObject.name + " �� " + respawnPosition + " ����");

        // 1. �ƶ�����λ�� (Z�����ɹ���������)
        transform.position = respawnPosition;

        // 2. �ָ�����
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;

        // 3. �������������������֮һ��
        //    ���ö���״̬������ֹ����"Die"���������һ֡
        //    (����Ҫ���� Animator ����� "Respawn" ������)
        anim.SetTrigger("Respawn");

        // 4. ����״̬
        isDead = false;
    }
}