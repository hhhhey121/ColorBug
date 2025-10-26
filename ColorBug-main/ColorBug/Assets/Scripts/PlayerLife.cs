using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
   
    public GameObject playerPS; // 粒子特效

    private Animator anim;
    private Rigidbody2D rb;
    public bool isDead = false; // 状态，防止重复死亡

    // 重生管理器的引用
    private RespawnManager respawnManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // 在游戏开始时自动查找管理器
        respawnManager = FindObjectOfType<RespawnManager>();
        if (respawnManager == null)
        {
            Debug.LogError("场景中未找到 RespawnManager！");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return; // 如果已经死了，就不要再触发了

        if (collision.gameObject.CompareTag("Trap"))
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true; // 标记为死亡
        rb.bodyType = RigidbodyType2D.Static; // 保持你原来的逻辑：身体静止
        anim.SetTrigger("death"); // 保持你原来的逻辑：播放死亡动画

       
        if (playerPS != null)
        {
            // 在角色位置生成 "playerPS" 特效
            GameObject deathParticles = Instantiate(playerPS, transform.position, Quaternion.identity);

            // 1秒后 *只销毁这个新生成的特效*
            // (你原来的代码 'Destroy(playerPS, 1f)' 会试图销毁预制体，是错误的)
            Destroy(deathParticles, 1f);
        }
        // -----------------------------------------------------

        //通知管理器，并隐藏自己 (我们不销毁角色)

        // 隐藏角色的 Sprite
        var sprite = GetComponentInChildren<SpriteRenderer>();
        if (sprite != null) sprite.enabled = false;

        // 禁用碰撞体
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;


        // 通知管理器“我死了”
        if (respawnManager != null)
        {
            respawnManager.HandlePlayerDeath(this);
        }
    }


    // 重生方法 (由 RespawnManager 调用)
    public void Respawn(Vector3 respawnPosition)
    {
        Debug.Log(gameObject.name + " 在 " + respawnPosition + " 重生。");

        // 移动到重生点
        transform.position = respawnPosition;

        // 恢复物理状态
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero; // 重置速度

        // 恢复视觉
        var sprite = GetComponentInChildren<SpriteRenderer>();
        if (sprite != null) sprite.enabled = true;

        // 恢复碰撞
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = true;

        // 重置状态
        isDead = false;

    }
}