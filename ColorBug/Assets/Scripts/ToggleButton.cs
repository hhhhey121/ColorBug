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

    
    void UpdateSprite()//按钮的视觉表现
    {
        if (sr != null && pressedSprite != null && unpressedSprite != null)
        {
            sr.sprite = isPressed ? pressedSprite : unpressedSprite;
        }
    }




    void Start()
    {
        sr= GetComponent<SpriteRenderer>();
        UpdateSprite(); // 初始化Sprite状态
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //检查冷却时间
        if(Time.time < lastHitRegisterTime + pressCooldown)
        {
            return;
        }

        //检查玩家是否碰到
        
        if(collision.gameObject.CompareTag("Player"))
        {
            bool hit = true;
            //foreach(ContactPoint2D contact in collision.contacts)//从上面踩
            //{

            //}
            hit = true;

            if(hit)
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

    void TogglePlatforms()//单按钮
    {
        isPressed = !isPressed;//切换状态

        UpdateSprite(); // 更新视觉

        foreach (MovingPlatform platform in platformsToControl)//移动平台
        {
            if (platform != null)
            {
                platform.ToggleTarget();
            }
        }
    }


    void PressButton_Dual()//双按钮
    {
        isPressed = true;
        lastPressTime = Time.time;//记录当前按下的时间

        UpdateSprite();


        // 启动自动重置计时器
        // (先取消已有的，防止玩家快速连踩导致Invoke混乱)
        CancelInvoke("ResetButton");
        Invoke("ResetButton", autoResetTime);

        // 检查另一个按钮的状态
        if (otherButton != null && otherButton.isPressed)
        {
            // 检查时间差
            if (Mathf.Abs(this.lastPressTime - otherButton.lastPressTime) <= simultaneityThreshold)
            {
                // 成功！
                Debug.Log("同时按下成功!");

                // 触发平台（包括对方的平台）
               TriggerPlatforms(true);
                //foreach (MovingPlatform platform in platformsToControl)//移动平台
                //{
                //    if (platform != null)
                //    {
                //        platform.ToggleTarget();
                //    }
                //}

                // 立即重置两个按钮
                this.ResetButton();
                otherButton.ResetButton();
            }
        }
    }

    
    // 重置按钮（双按钮逻辑
    public void ResetButton()
    {
        // 停止任何待处理的 "ResetButton" 调用
        CancelInvoke("ResetButton");

        isPressed = false;
        lastPressTime = -1f;

        
        UpdateSprite();
    }


    
    // 双按钮触发平台移动
    
    /// <param name="triggerBoth">是否也触发 otherButton 列表中的平台</param>
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

   
}


