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

    
    void UpdateSprite()//��ť���Ӿ�����
    {
        if (sr != null && pressedSprite != null && unpressedSprite != null)
        {
            sr.sprite = isPressed ? pressedSprite : unpressedSprite;
        }
    }




    void Start()
    {
        sr= GetComponent<SpriteRenderer>();
        UpdateSprite(); // ��ʼ��Sprite״̬
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�����ȴʱ��
        if(Time.time < lastHitRegisterTime + pressCooldown)
        {
            return;
        }

        //�������Ƿ�����
        
        if(collision.gameObject.CompareTag("Player"))
        {
            bool hit = true;
            //foreach(ContactPoint2D contact in collision.contacts)//�������
            //{

            //}
            hit = true;

            if(hit)
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

    void TogglePlatforms()//����ť
    {
        isPressed = !isPressed;//�л�״̬

        UpdateSprite(); // �����Ӿ�

        foreach (MovingPlatform platform in platformsToControl)//�ƶ�ƽ̨
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
        lastPressTime = Time.time;//��¼��ǰ���µ�ʱ��

        UpdateSprite();


        // �����Զ����ü�ʱ��
        // (��ȡ�����еģ���ֹ��ҿ������ȵ���Invoke����)
        CancelInvoke("ResetButton");
        Invoke("ResetButton", autoResetTime);

        // �����һ����ť��״̬
        if (otherButton != null && otherButton.isPressed)
        {
            // ���ʱ���
            if (Mathf.Abs(this.lastPressTime - otherButton.lastPressTime) <= simultaneityThreshold)
            {
                // �ɹ���
                Debug.Log("ͬʱ���³ɹ�!");

                // ����ƽ̨�������Է���ƽ̨��
               TriggerPlatforms(true);
                //foreach (MovingPlatform platform in platformsToControl)//�ƶ�ƽ̨
                //{
                //    if (platform != null)
                //    {
                //        platform.ToggleTarget();
                //    }
                //}

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

        
        UpdateSprite();
    }


    
    // ˫��ť����ƽ̨�ƶ�
    
    /// <param name="triggerBoth">�Ƿ�Ҳ���� otherButton �б��е�ƽ̨</param>
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

   
}


