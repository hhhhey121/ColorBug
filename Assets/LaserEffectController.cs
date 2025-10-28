using UnityEngine;

// 确保效果上有 SpriteRenderer 和 Animator
[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class LaserEffectController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // (可选) 动画播放完毕后自动销毁的时间
    // 你也可以在动画本身结束时添加事件来销毁
    public float autoDestroyTime = 0.5f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        // 确保一开始是看不见的，等待被初始化
        spriteRenderer.enabled = false;
    }

    // 由 PlayerLaserAbility 调用
    public void Initialize(Vector3 startPoint, Vector3 endPoint, bool isAbsorbEffect)
    {
        spriteRenderer.enabled = true; // 让它可见

        // --- 1. 计算位置、旋转和拉伸 ---
        Vector3 direction = endPoint - startPoint;
        float distance = direction.magnitude;

        // a) 设置位置 (放在起点)
        transform.position = startPoint;

        // b) 设置旋转 (朝向终点)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // c) 设置拉伸 (只拉伸 X 轴)
        //    【重要】这要求你的 Sprite Renderer 的 Draw Mode 设置为 Sliced 或 Tiled
        //    并且你的 Sprite 图片本身支持拉伸 (比如中间是纯色或重复图案)
        //    我们假设 Sprite 的原始长度对应 Scale X = 1
        //    你需要根据你的 Sprite 调整这个逻辑
        if (spriteRenderer.drawMode == SpriteDrawMode.Sliced || spriteRenderer.drawMode == SpriteDrawMode.Tiled)
        {
            // 对于 Sliced/Tiled 模式，我们修改 size 而不是 scale
            spriteRenderer.size = new Vector2(distance, spriteRenderer.size.y);
        }
        else // 如果是 Simple 模式 (不推荐用于拉伸)
        {
            // 你需要知道 Sprite 原始宽度 (单位)
            // float originalWidth = 1.0f; // 假设原始宽度是 1 个单位
            // transform.localScale = new Vector3(distance / originalWidth, 1, 1);
            Debug.LogWarning("LaserEffect Sprite Draw Mode is not Sliced or Tiled. Stretching may look incorrect.");
            // 简单处理：直接设置长度，可能需要调整sprite pivot
            transform.localScale = new Vector3(distance, 1, 1);
        }


        // --- 2. 播放正确的动画 ---
        // 我们假设你的 Animator Controller 中有两个状态：
        // "FireAnim" (正常播放) 和 "AbsorbAnim" (反向播放或另一个动画)
        if (isAbsorbEffect)
        {
            animator.Play("AbsorbAnim"); // 播放吸收动画
        }
        else
        {
            animator.Play("FireAnim"); // 播放发射动画
        }

        // --- 3. 计划自我销毁 ---
        Destroy(gameObject, autoDestroyTime);
    }
}