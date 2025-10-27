using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PressureButton : MonoBehaviour
{
    // ���״̬���� ButtonPairManager ��ȡ
    public bool isPressed { get; private set; } = false;

    // (��ѡ) ��ť����/����ʱ�� Sprite
    public Sprite spritePressed;
    public Sprite spriteReleased;

    private SpriteRenderer spriteRenderer;
    private int playersOnButton = 0; // ��������Ҳ�

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteReleased == null)
        {
            spriteReleased = spriteRenderer.sprite; // Ĭ��ʹ�ó�ʼ Sprite
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ȷ�����������ɫ���� "Player" ��ǩ
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
                playersOnButton = 0; // ��ֹ�����Ϊ����
                if (spriteRenderer != null && spriteReleased != null)
                {
                    spriteRenderer.sprite = spriteReleased;
                }
            }
        }
    }
}
