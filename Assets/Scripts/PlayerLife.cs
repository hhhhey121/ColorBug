using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using UnityEngine.SceneManagement; 

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private RespawnManager respawnManager; // ����������

    // ����״̬����Ϊ public���� PlayerMovement �ű����Զ�ȡ
    public bool isDead = false;

    // (ֻ�ڽ�ɫA����ֵ)
    private PlayerLaserAbility laserAbility;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        laserAbility = GetComponent<PlayerLaserAbility>();

        // �Զ����ҹ�����
        respawnManager = FindObjectOfType<RespawnManager>();
        if (respawnManager == null)
        {
            Debug.LogError("������δ�ҵ� RespawnManager!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ����Ѿ����ˣ���Ҫ�ظ�����
        if (isDead) return;

        if (collision.gameObject.CompareTag("Trap"))
        {
            Death();
        }
    }

    // (���� "LethalLaser" ��ǩ)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("LethalLaser"))
        {
           
            // ����ǽ�ɫA�������Ƿ��Ѿ�����������
            if (laserAbility != null && laserAbility.HasAbility())
            {
                // ��A���ѽ��� -> �������ⲻ������
                // �����߼��� PlayerLaserAbility �� Update ������⴦��
                return;
            }
            else
            {
                // ��B (laserAbility == null)��
                // ������A��һ�α����� (laserAbility.HasAbility() == false)
                // -> ֪ͨ������������
                AbilityManager.Instance.OnPlayerKilledByLaser();
                Death();
            }
        }
    }

    private void Death()
    {
        isDead = true; // 1. ���Ϊ����
        rb.bodyType = RigidbodyType2D.Static; // 2. ��������߼������徲ֹ
        anim.SetTrigger("death"); // 3. ������������


        // ֪ͨ������
        if (respawnManager != null)
        {
            respawnManager.HandlePlayerDeath(this);
        }
    }

    // �������� (�� RespawnManager ����)
    public void Respawn(Vector3 respawnPosition)
    {

        // �ƶ�����λ�� (Z�����ɹ���������)
        transform.position = respawnPosition;

        // �ָ�����
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;

        
        // ���ö���״̬������ֹ��Die���һ֡
        anim.SetTrigger("Respawn");

        // ����״̬
        isDead = false;
    }
}