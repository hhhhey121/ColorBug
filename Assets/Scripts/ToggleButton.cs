using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    //在Inspector中选择双按钮模式
    public bool requiresOtherButton = false;

    //双按钮模式设置
    public ToggleButton otherButton;
    public float simultaneityThreshold = 1f;
    public float autoResetTime = 1.0f;

    //通用模式
    public MovingPlatform[] platformsToControl;

    //视觉反馈
    public Sprite pressedSprite;
    public Sprite unpressedSprite;
    public SpriteRenderer sr;

    public bool isPressed = false;
    public float lastPressTime = -1f;// 双按钮模式的时间检查
    private float lastHitRegisterTime = -1f;// 用于冷却时间检查

    private float pressCooldown = 0.5f;//两个模式都使用此冷却时间


    // ??【新增】音效设置
    [Header("Sound Settings")]
    public AudioClip pressSound;           // 按下音效
    public AudioClip dualSuccessSound;     // 双按钮同时成功音效（可选）
    private AudioSource audioSource;       // 音源


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateSprite(); // 初始化Sprite状态

        // ??初始化AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    void UpdateSprite() //按钮的视觉表现
    {
        if (sr != null && pressedSprite != null && unpressedSprite != null)
        {
            sr.sprite = isPressed ? pressedSprite : unpressedSprite;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //检查冷却时间
        if (Time.time < lastHitRegisterTime + pressCooldown)
        {
            return;
        }

        //检查玩家是否碰到
        if (collision.gameObject.CompareTag("Player"))
        {
            bool hit = true;

            if (hit)
            {
                lastHitRegisterTime = Time.time;//重置冷却计时

                // 根据模式调用不同的按下逻辑
                if (requiresOtherButton)
                {
                    // 如果已经是按下状态（正在等待另一个按钮），就不要重复触发
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

    void TogglePlatforms() //单按钮模式
    {
        isPressed = !isPressed; //切换状态
        UpdateSprite(); // 更新视觉

        // ?? 播放音效
        PlaySound(pressSound);

        foreach (MovingPlatform platform in platformsToControl)
        {
            if (platform != null)
            {
                platform.ToggleTarget();
            }
        }
    }

    void PressButton_Dual() //双按钮模式
    {
        isPressed = true;
        lastPressTime = Time.time; //记录当前按下的时间

        UpdateSprite();

        // ?? 播放按下音效
        PlaySound(pressSound);

        // 启动自动重置计时器
        CancelInvoke("ResetButton");
        Invoke("ResetButton", autoResetTime);

        // 检查另一个按钮的状态
        if (otherButton != null && otherButton.isPressed)
        {
            // 检查时间差
            if (Mathf.Abs(this.lastPressTime - otherButton.lastPressTime) <= simultaneityThreshold)
            {
                Debug.Log("同时按下成功!");

                // ?? 播放双按钮成功音效
                PlaySound(dualSuccessSound);

                // 触发平台（包括对方的平台）
                TriggerPlatforms(true);

                // 立即重置两个按钮
                this.ResetButton();
                otherButton.ResetButton();
            }
        }
    }

    // 重置按钮（双按钮逻辑）
    public void ResetButton()
    {
        CancelInvoke("ResetButton");

        isPressed = false;
        lastPressTime = -1f;

        UpdateSprite();
    }

    // 双按钮触发平台移动
    void TriggerPlatforms(bool triggerBoth)
    {
        // 触发自己的平台
        foreach (MovingPlatform platform in platformsToControl)
        {
            if (platform != null)
            {
                platform.ToggleTarget();
            }
        }

        // 如果是双按钮模式成功，也触发另一个按钮的平台
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

    // ?? 播放音效的通用函数
    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}