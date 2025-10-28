using UnityEngine;

[RequireComponent(typeof(Collider2D))]
// ��������ͬ����ȷ���� AudioSource ���
[RequireComponent(typeof(AudioSource))]
public class PressureButton : MonoBehaviour
{
    // ���״̬���� ButtonPairManager ��ȡ
    public bool isPressed { get; private set; } = false;

    // (��ѡ) ��ť����/����ʱ�� Sprite
    public Sprite spritePressed;
    public Sprite spriteReleased;

    // ����������Ч�ֶ�
    [Header("��Ч")]
    public AudioClip soundPressed;   // ����ʱ����Ч
    public AudioClip soundReleased;  // (��ѡ) ����ʱ����Ч

    private SpriteRenderer spriteRenderer;
    private int playersOnButton = 0; // ��������Ҳ�
    private AudioSource audioSource; // ����������Ƶ������

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); // ����������ȡ���

        if (spriteReleased == null)
        {
            spriteReleased = spriteRenderer.sprite; // Ĭ��ʹ�ó�ʼ Sprite
        }
        // ��������ȷ����ʼ״̬��ȷ
        spriteRenderer.sprite = spriteReleased;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ȷ�����������ɫ���� "Player" ��ǩ
        if (collision.CompareTag("Player"))
        {
            // ���޸ġ�������Ƿ��ǵ�һ�����ϰ�ť�����
            bool wasPressedBefore = isPressed;

            playersOnButton++;
            isPressed = true;

            if (spriteRenderer != null && spritePressed != null)
            {
                spriteRenderer.sprite = spritePressed;
            }

            // ��������ֻ�е���ť�ӡ�δ���¡���Ϊ�����¡�ʱ���Ų�����Ч
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
                playersOnButton = 0; // ��ֹ�����Ϊ����

                if (spriteRenderer != null && spriteReleased != null)
                {
                    spriteRenderer.sprite = spriteReleased;
                }

                // ������������ť����ʱ�����ŵ�����Ч (���������)
                if (soundReleased != null)
                {
                    audioSource.PlayOneShot(soundReleased);
                }
            }
        }
    }
}