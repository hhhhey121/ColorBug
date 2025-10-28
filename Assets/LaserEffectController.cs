using UnityEngine;

// 确保效果上有 SpriteRenderer 和 Animator
[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class LaserEffectController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public float autoDestroyTime = 0.5f;

    // 【删除】不再需要 Animator 参数
    // private static readonly int IsAbsorbHash = Animator.StringToHash("IsAbsorb");

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spriteRenderer.enabled = false;
    }

    // 由 PlayerLaserAbility 调用
    public void Initialize(Vector3 startPoint, Vector3 endPoint, bool isAbsorbEffect)
    {
        // --- 1. 计算位置、旋转和拉伸 (保持不变) ---
        Vector3 direction = endPoint - startPoint;
        float distance = direction.magnitude;
        transform.position = startPoint;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (spriteRenderer.drawMode == SpriteDrawMode.Sliced || spriteRenderer.drawMode == SpriteDrawMode.Tiled)
        {
            spriteRenderer.size = new Vector2(distance, spriteRenderer.size.y);
        }
        else
        {
            Debug.LogWarning("LaserEffect Sprite Draw Mode is not Sliced or Tiled. Stretching may look incorrect.");
            transform.localScale = new Vector3(distance, 1, 1);
        }

        // --- 2. 【关键修改】直接播放动画 ---
        // 我们改回使用 animator.Play()，它会立即生效
        if (isAbsorbEffect)
        {
            animator.Play("AbsorbAnim", -1, 0f); // 强制从头播放 AbsorbAnim
        }
        else
        {
            animator.Play("FireAnim", -1, 0f); // 强制从头播放 FireAnim
        }
        // animator.Play(stateName, layer, normalizedTime);
        // layer = -1 表示在所有层上播放
        // normalizedTime = 0f 表示从动画开头播放

        // 【删除】不再需要设置参数
        // animator.SetBool(IsAbsorbHash, isAbsorbEffect);

        // --- 3. 让它可见 (在 Play 之后再显示，确保看到的是第一帧) ---
        spriteRenderer.enabled = true; // 让它可见

        // --- 4. 计划自我销毁 (保持不变) ---
        Destroy(gameObject, autoDestroyTime);
    }
}