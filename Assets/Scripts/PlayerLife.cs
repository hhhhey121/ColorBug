using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 【移除】不再需要 SceneManager
// using UnityEngine.SceneManagement; 

public class PlayerLife : MonoBehaviour
{
    // 【重要】请把你的粒子特效 "预制体" (Prefab) 拖到这里
    public GameObject playerDeathPS_Prefab;

    private Animator anim;
    private Rigidbody2D rb;
    private RespawnManager respawnManager; // 【新】重生管理器

    // 【新】死亡状态，改为 public，让 PlayerMovement 脚本可以读取
    public bool isDead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // 【新】自动查找管理器
        respawnManager = FindObjectOfType<RespawnManager>();
        if (respawnManager == null)
        {
            Debug.LogError("场景中未找到 RespawnManager!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 【新】如果已经死了，不要重复触发
        if (isDead) return;

        if (collision.gameObject.CompareTag("Trap"))
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true; // 1. 标记为死亡
        rb.bodyType = RigidbodyType2D.Static; // 2. 保持你的逻辑：身体静止
        anim.SetTrigger("death"); // 3. 播放死亡动画

        // 4. 【修复】你原来的 Destroy(playerPS) 是错误的
        //    我们应该“生成”特效，而不是“销毁”
        if (playerDeathPS_Prefab != null)
        {
            GameObject ps = Instantiate(playerDeathPS_Prefab, transform.position, Quaternion.identity);
            Destroy(ps, 2f); // 2秒后销毁这个 *特效*
        }

        // 5. 【新】通知管理器
        if (respawnManager != null)
        {
            respawnManager.HandlePlayerDeath(this);
        }
    }

    // 【移除】 private void Restart() { ... }
    // 这个方法导致了你的所有问题，必须移除。

    // 【新】重生方法 (这个方法将由 RespawnManager 调用)
    public void Respawn(Vector3 respawnPosition)
    {
        Debug.Log(gameObject.name + " 在 " + respawnPosition + " 重生");

        // 1. 移动到新位置 (Z轴已由管理器修正)
        transform.position = respawnPosition;

        // 2. 恢复物理
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;

        // 3. 【解决“看不见”问题之一】
        //    重置动画状态机，防止卡在"Die"动画的最后一帧
        //    (这需要你在 Animator 中添加 "Respawn" 触发器)
        anim.SetTrigger("Respawn");

        // 4. 重置状态
        isDead = false;
    }
}