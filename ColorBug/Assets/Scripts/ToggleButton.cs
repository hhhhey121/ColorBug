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
    public float lastPressTime = -1f;
    private float lastHitTime = -1f;

    private float pressCooldown = 0.5f;//����ģʽ��ʹ�ô���ȴʱ��




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
        //�����ȴʱ��
        if (Time.time < lastPressTime + pressCooldown)
        {
            return;
        }

        //�������Ƿ�����
        bool hit = false;
        if (collision.gameObject.CompareTag("Player"))
        {

            //foreach(ContactPoint2D contact in collision.contacts)//�������
            //{

            //}
            hit = true;

            if (hit)
            {
                lastPressTime = Time.time;//������ȴ��ʱ


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

    void TogglePlatforms()//����ť
    {
        isPressed = !isPressed;//�л�״̬

        //�л�sprite
        if (sr != null && pressedSprite != null && unpressedSprite != null)
        {
            sr.sprite = isPressed ? pressedSprite : unpressedSprite;
        }

        foreach (MovingPlatform platform in platformsToControl)
        {
            if (platform != null)
            {
                platform.ToggleTarget();
            }
        }
    }


    void PressButton_Dual()//˫��ť
    {
        isPressed = true;
        lastPressTime = Time.time;
        isPressed = !isPressed;//�л�״̬

        //�л�sprite//UpdateSprite();
        if (sr != null && pressedSprite != null && unpressedSprite != null)
        {
            sr.sprite = isPressed ? pressedSprite : unpressedSprite;
        }


        // �����Զ����ü�ʱ��
        Invoke("ResetButton", autoResetTime);

        // �����һ����ť��״̬
        if (otherButton != null && otherButton.isPressed)
        {
            // ���ʱ���
            if (Mathf.Abs(this.lastPressTime - otherButton.lastPressTime) <= simultaneityThreshold)
            {
                // �ɹ���
                Debug.Log("ͬʱ���³ɹ�!");

                // ����ƽ̨��������ť�ģ�
                TriggerPlatforms(true);

                // ��������������ť
                this.ResetButton();
                otherButton.ResetButton();
            }
        }
    }


    // ���ð�ť��˫��ť�߼�
    public void ResetButton()
    {
        // ֹͣ�κδ������ "ResetButton" ����
        CancelInvoke("ResetButton");

        isPressed = false;
        lastPressTime = -1f;
        isPressed = !isPressed;//�л�״̬

        //�л�sprite
        if (sr != null && pressedSprite != null && unpressedSprite != null)
        {
            sr.sprite = isPressed ? pressedSprite : unpressedSprite;
        }
    }


    /// <summary>
    /// ��������ƽ̨�ƶ��ķ���
    /// </summary>
    /// <param name="triggerBoth">�Ƿ�Ҳ���� otherButton �б��е�ƽ̨</param>
    void TriggerPlatforms(bool triggerBoth)
    {
        // ���Ǵ����Լ���ƽ̨
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


}