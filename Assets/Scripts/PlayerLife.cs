using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private RespawnManager respawnManager;
    public bool isDead = false;

    // (��ɫA����)
    private PlayerLaserAbility laserAbility;
    // (��ҽ�ҽű�)
    private Coin playerCoinCollector;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        respawnManager = FindObjectOfType<RespawnManager>();
        laserAbility = GetComponent<PlayerLaserAbility>();

        // ����������ȡ��ҵĽ�ҽű�
        playerCoinCollector = GetComponent<Coin>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // 1. ����Ƿ�ײ���� "Trap"
        if (collision.gameObject.CompareTag("Trap"))
        {
            // �����߼���
            // 2. ������ "Trap" �ǲ��� "���յش�"
            FinalWinSpike finalSpike = collision.gameObject.GetComponentInParent<FinalWinSpike>();

            if (finalSpike != null)
            {
                // 3a. ����ǣ�֪ͨ LevelWinManager
                if (LevelWinManager.Instance != null)
                {
                    // �ѽ����Ϣ���ݹ�ȥ
                    LevelWinManager.Instance.NotifyDeathOnFinalSpike(playerCoinCollector);
                }
            }

            // 4. ������Σ���ִ������ (RespawnManager �ᴦ������)
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("LethalLaser"))
        {
            // (�����еļ�������߼������ֲ���)
            if (laserAbility != null && laserAbility.HasAbility())
            {
                return;
            }
            else
            {
                AbilityManager.Instance.OnPlayerKilledByLaser();
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
        if (respawnManager != null) respawnManager.HandlePlayerDeath(this);
    }
    public void Respawn(Vector3 respawnPosition)
    {
        transform.position = respawnPosition;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Respawn");
        isDead = false;
    }
}