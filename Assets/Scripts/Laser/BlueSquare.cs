using UnityEngine;
using System.Collections; // 需要这个来使用协程或延迟销毁

public class BlueSquare : MonoBehaviour // 可摧毁蓝块
{
    // (可选) 拖入一个爆炸/碎裂特效
    public GameObject destroyEffectPrefab;

    // 【新增】可以在Inspector中调节的延迟销毁时间（单位：秒）
    [Tooltip("被击中后多久才在画面中消失")]
    public float destroyDelay = 0.5f; // 默认延迟半秒

    private bool isBeingDestroyed = false; // 防止重复触发

    // 公共方法，由 PlayerLaserAbility 的射线检测调用
    public void BeDestroyed()
    {
        // 如果已经在销毁过程中，则不再执行
        if (isBeingDestroyed) return;
        isBeingDestroyed = true;

        Debug.Log("蓝色方块被击中!");

        // 1. 立刻播放特效（如果设置了）
        if (destroyEffectPrefab != null)
        {
            Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
        }

        // 2. 【可选但推荐】立刻禁用碰撞体，防止在消失前还能挡住玩家或子弹
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // 3. 【可选】立刻禁用 Sprite Renderer，如果你的 destroyEffectPrefab 负责显示碎裂效果
        // SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        // if (spriteRenderer != null)
        // {
        //     spriteRenderer.enabled = false;
        // }

        // 4. 【关键修改】使用带延迟的 Destroy
        Destroy(gameObject, destroyDelay);
    }
}