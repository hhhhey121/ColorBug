using UnityEngine;
using System.Collections;

/// <summary>
/// 一个只能按一次的按钮。
/// 按下后会激活所有关联的 OneWayPlatform，并永久保持按下状态。
/// </summary>
public class OneTimeButton : MonoBehaviour
{
    // 控制目标：改为 OneWayPlatform 数组
    public OneWayPlatform[] platformsToControl;

    // 视觉反馈
    public Sprite pressedSprite;
    public Sprite unpressedSprite;
    public SpriteRenderer sr;

    private bool isPressedOnce = false; // 标记按钮是否已被按过

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
        // 如果已经按过，则不执行任何操作
        if (isPressedOnce)
        {
            return;
        }

        // 检查是否是玩家碰到
        if (collision.gameObject.CompareTag("Player"))
        {
            // (这里可以添加更精细的碰撞方向检查，例如只在从上方踩踏时触发)

            // 标记为已按下
            isPressedOnce = true;
            Debug.Log(gameObject.name + " 按钮已被按下。");

            // 激活平台
            ActivatePlatforms();

            // 切换Sprite到“按下”状态
            if (sr != null && pressedSprite != null)
            {
                sr.sprite = pressedSprite;
            }
        }
    }

    /// <summary>
    /// 激活所有受控的平台。
    /// </summary>
    void ActivatePlatforms()
    {
        foreach (OneWayPlatform platform in platformsToControl)
        {
            if (platform != null)
            {
                platform.ActivateMovement(); // 调用新平台的激活方法
            }
        }
    }
}
