using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    ////public MovingPlatform[] platformsToControl;

    //视觉反馈
    public Sprite pressedSprite;
    public Sprite unpressedSprite;
    public SpriteRenderer sr;

    private bool isPressed = false;
    private float pressCooldown = 0.5f;
    private float lastPressTime = -1f;



    void Start()
    {
        sr= GetComponent<SpriteRenderer>();
        if(sr != null&&unpressedSprite!=null )
        {
            sr.sprite = unpressedSprite;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //检查冷却时间
        if(Time.time<lastPressTime+pressCooldown)
        {
            return;
        }

        //检查玩家是否碰到
        bool hit = false;
        if(collision.gameObject.CompareTag("player"))
        {
            
            //foreach(ContactPoint2D contact in collision.contacts)//从上面踩
            //{

            //}
            hit = true;

            if(hit)
            {
                lastPressTime = Time.time;//重置冷却计时
                TogglePlatforms();
            }
        }
    }

    void TogglePlatforms()
    {
        isPressed = !isPressed;//切换状态
        
        //切换sprite
        if(sr!=null&&pressedSprite!=null&&unpressedSprite!=null)
        {
            sr.sprite=isPressed?pressedSprite:unpressedSprite;
        }

        //foreach(MovingPlatform platform in platformsToControl)
        //{
        //    if(platform!=null)
        //    {
        //        platform.ToggleTarget();
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
