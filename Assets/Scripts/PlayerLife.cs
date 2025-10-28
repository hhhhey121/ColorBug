using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private RespawnManager respawnManager;
    public bool isDead = false;

    // (角色A独有)
    private PlayerLaserAbility laserAbility;
    // (玩家金币脚本)
    private Coin playerCoinCollector;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        respawnManager = FindObjectOfType<RespawnManager>();
        laserAbility = GetComponent<PlayerLaserAbility>();

        // 【新增】获取玩家的金币脚本
        playerCoinCollector = GetComponent<Coin>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // 1. 检查是否撞到了 "Trap"
        if (collision.gameObject.CompareTag("Trap"))
        {
            // 【新逻辑】
            // 2. 检查这个 "Trap" 是不是 "最终地刺"
            FinalWinSpike finalSpike = collision.gameObject.GetComponentInParent<FinalWinSpike>();

            if (finalSpike != null)
            {
                // 3a. 如果是，通知 LevelWinManager
                if (LevelWinManager.Instance != null)
                {
                    // 把金币信息传递过去
                    LevelWinManager.Instance.NotifyDeathOnFinalSpike(playerCoinCollector);
                }
            }

            // 4. 无论如何，都执行死亡 (RespawnManager 会处理重生)
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("LethalLaser"))
        {
            // (你现有的激光解锁逻辑，保持不变)
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