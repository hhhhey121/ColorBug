using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public GameObject playerPS; // 粒子特效
    private Animator anim;
    private Rigidbody2D rb;
    public bool isDead = false;
    private RespawnManager respawnManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        respawnManager = FindObjectOfType<RespawnManager>();
        if (respawnManager == null)
        {
            Debug.LogError("场景中未找到 RespawnManager！");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Trap"))
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true;
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death"); // 播放死亡动画

        if (playerPS != null)
        {
            GameObject deathParticles = Instantiate(playerPS, transform.position, Quaternion.identity);
            Destroy(deathParticles, 1f);
        }

        // 隐藏 Sprite (使用 GetComponentInChildren)
        var sprite = GetComponentInChildren<SpriteRenderer>();
        if (sprite != null) sprite.enabled = false;

        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        if (respawnManager != null)
        {
            respawnManager.HandlePlayerDeath(this);
        }
    }

    // 重生方法 (由 RespawnManager 调用)
    public void Respawn(Vector3 respawnPosition)
    {
        // 这里的 respawnPosition 已经是被 RespawnManager 修正过Z轴的
        Debug.Log(gameObject.name + " 在 " + respawnPosition + " (Z轴已修正) 重生。");

        transform.position = respawnPosition;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;

        // 恢复 Sprite
        var sprite = GetComponentInChildren<SpriteRenderer>();
        if (sprite != null) sprite.enabled = true;

        // 恢复碰撞
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = true;

        isDead = false;

        // -----------------------------------------------------
        // !!! 【关键修复 2】 !!!
        // 重置动画状态机，防止卡在"Die"动画的透明帧
        // 你需要在你的 Animator (动画控制器) 中：
        // 1. 创建一个新的 Trigger (触发器)，命名为 "Respawn"
        // 2. 创建一个从 "Die" 状态到 "Idle" (或你的默认) 状态的过渡 (Transition)
        // 3. 把这个过渡的条件 (Condition) 设置为 "Respawn" 触发器
        // -----------------------------------------------------
        anim.SetTrigger("Respawn");
    }
}