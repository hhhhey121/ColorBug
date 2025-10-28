using UnityEngine;

// 【新增】确保对象上总有一个 AudioSource 组件
[RequireComponent(typeof(AudioSource))]
public class SwitchController : MonoBehaviour
{
    // 需要在 Inspector 中指定的变量
    [Header("视觉效果")]
    public Sprite pressedSprite;    // 按下后的精灵图片
    public Sprite unpressedSprite;  // 未按下时的精灵图片

    [Header("功能")]
    public GameObject[] objectsToActivate; // 按下后需要激活的游戏对象（例如风场）

    // 【新增】音效
    [Header("音效")]
    public AudioClip pressSound; // 拖入按钮按下的音效

    // 私有变量
    private SpriteRenderer sr;
    private bool isPressed = false; // 确保开关只触发一次
    private AudioSource audioSource; // 【新增】音频播放器

    void Start()
    {
        // 自动获取 SpriteRenderer 组件
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // 初始状态设置为“未按下”
            sr.sprite = unpressedSprite;
        }

        // 【新增】获取 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
    }

    // 当有其他 Collider 2D 进入这个触发器时调用
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查是否已被按下，以及进入的是否是“Player”
        if (!isPressed && other.CompareTag("Player"))
        {
            // 1. 标记为已按下
            isPressed = true;

            // 2. 切换精灵图片
            if (sr != null && pressedSprite != null)
            {
                sr.sprite = pressedSprite;
            }

            // 【新增】 3. 播放按键音效
            if (audioSource != null && pressSound != null)
            {
                audioSource.PlayOneShot(pressSound);
            }

            // 4. 激活所有指定的游戏对象 (原第3步)
            foreach (GameObject obj in objectsToActivate)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }
    }
}