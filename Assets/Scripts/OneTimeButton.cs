using UnityEngine;
using System.Collections;

/// <summary>
/// һ��ֻ�ܰ�һ�εİ�ť��
/// ���º�ἤ�����й����� OneWayPlatform�������ñ��ְ���״̬��
/// </summary>
public class OneTimeButton : MonoBehaviour
{
    // ����Ŀ�꣺��Ϊ OneWayPlatform ����
    public OneWayPlatform[] platformsToControl;

    // �Ӿ�����
    public Sprite pressedSprite;
    public Sprite unpressedSprite;
    public SpriteRenderer sr;

    private bool isPressedOnce = false; // ��ǰ�ť�Ƿ��ѱ�����

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null && unpressedSprite != null)
        {
            sr.sprite = unpressedSprite;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ����Ѿ���������ִ���κβ���
        if (isPressedOnce)
        {
            return;
        }

        // ����Ƿ����������
        if (collision.gameObject.CompareTag("Player"))
        {
            // (���������Ӹ���ϸ����ײ�����飬����ֻ�ڴ��Ϸ���̤ʱ����)

            // ���Ϊ�Ѱ���
            isPressedOnce = true;
            Debug.Log(gameObject.name + " ��ť�ѱ����¡�");

            // ����ƽ̨
            ActivatePlatforms();

            // �л�Sprite�������¡�״̬
            if (sr != null && pressedSprite != null)
            {
                sr.sprite = pressedSprite;
            }
        }
    }

    /// <summary>
    /// ���������ܿص�ƽ̨��
    /// </summary>
    void ActivatePlatforms()
    {
        foreach (OneWayPlatform platform in platformsToControl)
        {
            if (platform != null)
            {
                platform.ActivateMovement(); // ������ƽ̨�ļ����
            }
        }
    }
}
