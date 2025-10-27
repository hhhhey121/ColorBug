using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PressureButton : MonoBehaviour
{
    // 这个状态将由 ButtonPairManager 读取
    public bool isPressed { get; private set; } = false;

    // (可选) 按钮按下/弹起时的 Sprite
    public Sprite spritePressed;
    public Sprite spriteReleased;

    private SpriteRenderer spriteRenderer;
    private int playersOnButton = 0; // 允许多个玩家踩

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteReleased == null)
        {
            spriteReleased = spriteRenderer.sprite; // 默认使用初始 Sprite
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 确保你的两个角色都有 "Player" 标签
        if (collision.CompareTag("Player"))
        {
            playersOnButton++;
            isPressed = true;
            if (spriteRenderer != null && spritePressed != null)
            {
                spriteRenderer.sprite = spritePressed;
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
            }
        }
    }
}
