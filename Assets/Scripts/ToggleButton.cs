using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    //��Inspector��ѡ��˫��ťģʽ
    public bool requiresOtherButton = false;

    //˫��ťģʽ����
    public ToggleButton otherButton;
    public float simultaneityThreshold = 1f;
    public float autoResetTime = 1.0f;

    //ͨ��ģʽ
    public MovingPlatform[] platformsToControl;

    //�Ӿ�����
    public Sprite pressedSprite;
    public Sprite unpressedSprite;
    public SpriteRenderer sr;

    public bool isPressed = false;
    public float lastPressTime = -1f;// ˫��ťģʽ��ʱ����
    private float lastHitRegisterTime = -1f;// ������ȴʱ����

    private float pressCooldown = 0.5f;//����ģʽ��ʹ�ô���ȴʱ��


    // ??����������Ч����
    [Header("Sound Settings")]
    public AudioClip pressSound;           // ������Ч
    public AudioClip dualSuccessSound;     // ˫��ťͬʱ�ɹ���Ч����ѡ��
    private AudioSource audioSource;       // ��Դ


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateSprite(); // ��ʼ��Sprite״̬

        // ??��ʼ��AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    void UpdateSprite() //��ť���Ӿ�����
    {
        if (sr != null && pressedSprite != null && unpressedSprite != null)
        {
            sr.sprite = isPressed ? pressedSprite : unpressedSprite;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�����ȴʱ��
        if (Time.time < lastHitRegisterTime + pressCooldown)
        {
            return;
        }

        //�������Ƿ�����
        if (collision.gameObject.CompareTag("Player"))
        {
            bool hit = true;

            if (hit)
            {
                lastHitRegisterTime = Time.time;//������ȴ��ʱ

                // ����ģʽ���ò�ͬ�İ����߼�
                if (requiresOtherButton)
                {
                    // ����Ѿ��ǰ���״̬�����ڵȴ���һ����ť�����Ͳ�Ҫ�ظ�����
                    if (isPressed) return;
                    PressButton_Dual();
                }
                else
                {
                    TogglePlatforms();
                }
            }
        }
    }

    void TogglePlatforms() //����ťģʽ
    {
        isPressed = !isPressed; //�л�״̬
        UpdateSprite(); // �����Ӿ�

        // ?? ������Ч
        PlaySound(pressSound);

        foreach (MovingPlatform platform in platformsToControl)
        {
            if (platform != null)
            {
                platform.ToggleTarget();
            }
        }
    }

    void PressButton_Dual() //˫��ťģʽ
    {
        isPressed = true;
        lastPressTime = Time.time; //��¼��ǰ���µ�ʱ��

        UpdateSprite();

        // ?? ���Ű�����Ч
        PlaySound(pressSound);

        // �����Զ����ü�ʱ��
        CancelInvoke("ResetButton");
        Invoke("ResetButton", autoResetTime);

        // �����һ����ť��״̬
        if (otherButton != null && otherButton.isPressed)
        {
            // ���ʱ���
            if (Mathf.Abs(this.lastPressTime - otherButton.lastPressTime) <= simultaneityThreshold)
            {
                Debug.Log("ͬʱ���³ɹ�!");

                // ?? ����˫��ť�ɹ���Ч
                PlaySound(dualSuccessSound);

                // ����ƽ̨�������Է���ƽ̨��
                TriggerPlatforms(true);

                // ��������������ť
                this.ResetButton();
                otherButton.ResetButton();
            }
        }
    }

    // ���ð�ť��˫��ť�߼���
    public void ResetButton()
    {
        CancelInvoke("ResetButton");

        isPressed = false;
        lastPressTime = -1f;

        UpdateSprite();
    }

    // ˫��ť����ƽ̨�ƶ�
    void TriggerPlatforms(bool triggerBoth)
    {
        // �����Լ���ƽ̨
        foreach (MovingPlatform platform in platformsToControl)
        {
            if (platform != null)
            {
                platform.ToggleTarget();
            }
        }

        // �����˫��ťģʽ�ɹ���Ҳ������һ����ť��ƽ̨
        if (triggerBoth && otherButton != null)
        {
            foreach (MovingPlatform platform in otherButton.platformsToControl)
            {
                if (platform != null)
                {
                    platform.ToggleTarget();
                }
            }
        }
    }

    // ?? ������Ч��ͨ�ú���
    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}