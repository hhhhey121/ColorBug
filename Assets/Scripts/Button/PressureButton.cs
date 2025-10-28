using UnityEngine;

[RequireComponent(typeof(Collider2D))]
// 【新增】同样，确保有 AudioSource 组件
[RequireComponent(typeof(AudioSource))]
public class PressureButton : MonoBehaviour
{
    // 这个状态将由 ButtonPairManager 读取
    public bool isPressed { get; private set; } = false;

    // (可选) 按钮按下/弹起时的 Sprite
    public Sprite spritePressed;
    public Sprite spriteReleased;

    // 【新增】音效字段
    [Header("音效")]
    public AudioClip soundPressed;   // 按下时的音效
    public AudioClip soundReleased;  // (可选) 弹起时的音效

    private SpriteRenderer spriteRenderer;
    private int playersOnButton = 0; // 允许多个玩家踩
    private AudioSource audioSource; // 【新增】音频播放器

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); // 【新增】获取组件

        if (spriteReleased == null)
        {
            spriteReleased = spriteRenderer.sprite; // 默认使用初始 Sprite
        }
        // 【新增】确保初始状态正确
        spriteRenderer.sprite = spriteReleased;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 确保你的两个角色都有 "Player" 标签
        if (collision.CompareTag("Player"))
        {
            // 【修改】检查这是否是第一个踩上按钮的玩家
            bool wasPressedBefore = isPressed;

            playersOnButton++;
            isPressed = true;

            if (spriteRenderer != null && spritePressed != null)
            {
                spriteRenderer.sprite = spritePressed;
            }

            // 【新增】只有当按钮从“未按下”变为“按下”时，才播放音效
            if (!wasPressedBefore && soundPressed != null)
            {
                audioSource.PlayOneShot(soundPressed);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playersOnButton--;
            if (playersOnButton <= 0)
            {
                isPressed = false;
                playersOnButton = 0; // 防止意外变为负数

                if (spriteRenderer != null && spriteReleased != null)
                {
                    spriteRenderer.sprite = spriteReleased;
                }

                // 【新增】当按钮弹起时，播放弹起音效 (如果设置了)
                if (soundReleased != null)
                {
                    audioSource.PlayOneShot(soundReleased);
                }
            }
        }
    }
}