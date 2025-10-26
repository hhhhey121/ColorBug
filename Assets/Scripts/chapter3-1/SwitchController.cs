using UnityEngine;

public class SwitchController : MonoBehaviour
{
    // ��Ҫ�� Inspector ��ָ���ı���
    [Header("�Ӿ�Ч��")]
    public Sprite pressedSprite;    // ���º�ľ���ͼƬ
    public Sprite unpressedSprite;  // δ����ʱ�ľ���ͼƬ

    [Header("����")]
    public GameObject[] objectsToActivate; // ���º���Ҫ�������Ϸ��������糡��

    // ˽�б���
    private SpriteRenderer sr;
    private bool isPressed = false; // ȷ������ֻ����һ��

    void Start()
    {
        // �Զ���ȡ SpriteRenderer ���
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // ��ʼ״̬����Ϊ��δ���¡�
            sr.sprite = unpressedSprite;
        }
    }

    // �������� Collider 2D �������������ʱ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ����Ƿ��ѱ����£��Լ�������Ƿ��ǡ�Player��
        if (!isPressed && other.CompareTag("Player"))
        {
            // 1. ���Ϊ�Ѱ���
            isPressed = true;

            // 2. �л�����ͼƬ
            if (sr != null && pressedSprite != null)
            {
                sr.sprite = pressedSprite;
            }

            // 3. ��������ָ������Ϸ����
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