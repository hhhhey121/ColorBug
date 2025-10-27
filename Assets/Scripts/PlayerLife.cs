using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using UnityEngine.SceneManagement; 

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private RespawnManager respawnManager; // 重生管理器

    // 死亡状态，改为 public，让 PlayerMovement 脚本可以读取
    public bool isDead = false;

    // (只在角色A上有值)
    private PlayerLaserAbility laserAbility;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        laserAbility = GetComponent<PlayerLaserAbility>();

        // 自动查找管理器
        respawnManager = FindObjectOfType<RespawnManager>();
        if (respawnManager == null)
        {
            Debug.LogError("场景中未找到 RespawnManager!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果已经死了，不要重复触发
        if (isDead) return;

        if (collision.gameObject.CompareTag("Trap"))
        {
            Death();
        }
    }

    // (用于 "LethalLaser" 标签)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("LethalLaser"))
        {
           
            // 检查是角色A，并且是否已经解锁了能力
            if (laserAbility != null && laserAbility.HasAbility())
            {
                // 是A，已解锁 -> 碰到激光不会死。
                // 吸收逻辑由 PlayerLaserAbility 的 Update 按键检测处理。
                return;
            }
            else
            {
                // 是B (laserAbility == null)，
                // 或者是A第一次被击中 (laserAbility.HasAbility() == false)
                // -> 通知管理器并死亡
                AbilityManager.Instance.OnPlayerKilledByLaser();
                Death();
            }
        }
    }

    private void Death()
    {
        isDead = true; // 1. 标记为死亡
        rb.bodyType = RigidbodyType2D.Static; // 2. 保持你的逻辑：身体静止
        anim.SetTrigger("death"); // 3. 播放死亡动画


        // 通知管理器
        if (respawnManager != null)
        {
            respawnManager.HandlePlayerDeath(this);
        }
    }

    // 重生方法 (由 RespawnManager 调用)
    public void Respawn(Vector3 respawnPosition)
    {

        // 移动到新位置 (Z轴已由管理器修正)
        transform.position = respawnPosition;

        // 恢复物理
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;

        
        // 重置动画状态机，防止卡Die最后一帧
        anim.SetTrigger("Respawn");

        // 重置状态
        isDead = false;
    }
}