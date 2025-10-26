using UnityEngine;

/// <summary>
/// 监控多个 OneTimeButton，当所有按钮都被按下后，激活所有受控的 OneWayPlatform。
/// 这是一个更健壮的版本，它不依赖于 OneTimeButton 上的 'sr' 字段是否在 Inspector 中被赋值。
/// </summary>
public class MultiButtonController : MonoBehaviour
{
    // 目标：需要监控的按钮
    public OneTimeButton[] buttonsToMonitor;

    // 目标：需要控制的【真正】平台（墙体）
    public OneWayPlatform[] platformsToControl;

    // 确保平台只被激活一次
    private bool hasActivated = false;

    /// <summary>
    /// 每帧检查按钮状态
    /// </summary>
    void Update()
    {
        // 如果平台已经激活过了，就没必要再检查了
        if (hasActivated)
        {
            return;
        }

        // 检查所有按钮是否都已被按下
        if (AreAllButtonsPressed())
        {
            Debug.Log("--- (健壮版) 所有按钮均已按下！激活平台！---");
            ActivateRealPlatforms();
            hasActivated = true; // 标记为已激活，停止检查
        }
    }

    /// <summary>
    /// 遍历所有按钮，检查它们是否都处于“按下”状态
    /// </summary>
    /// <returns>如果所有按钮都按下了，返回 true</returns>
    private bool AreAllButtonsPressed()
    {
        if (buttonsToMonitor == null || buttonsToMonitor.Length == 0)
        {
            return false; // 没有按钮可监控
        }

        int pressedCount = 0;

        foreach (OneTimeButton button in buttonsToMonitor)
        {
            // --- 关键修改：更健壮的检查方式 ---

            // 1. 确保按钮脚本本身存在
            if (button == null) continue;

            // 2. 从按钮脚本获取我们期望的 "pressedSprite"
            //    这是 OneTimeButton 上的 public 字段，我们可以读取它。
            //    我们必须确保这个字段在 Inspector 中被赋值了！
            Sprite spriteToMatch = button.pressedSprite;

            // 3. (核心) 我们不使用 "button.sr"，因为它可能没有在 Inspector 中被赋值。
            //    我们自己去按钮所在的 GameObject 上寻找 SpriteRenderer 组件。
            SpriteRenderer rendererOnButton = button.GetComponent<SpriteRenderer>();

            // 4. 检查我们是否找到了所有需要的东西
            if (rendererOnButton == null || rendererOnButton.sprite == null || spriteToMatch == null)
            {
                // 如果这个按钮没有 SpriteRenderer，或者当前没有 sprite，
                // 或者我们不知道 "pressed" sprite 长什么样(spriteToMatch == null)，就跳过。
                if (spriteToMatch == null)
                {
                    Debug.LogWarning("按钮 " + button.gameObject.name + " 没有在 OneTimeButton 组件上设置 'Pressed Sprite'!");
                }
                continue;
            }

            // 5. 比较当前 sprite 的名称和 "pressed" sprite 的名称
            if (rendererOnButton.sprite.name == spriteToMatch.name)
            {
                // Debug.Log(button.gameObject.name + " 已检测为按下状态。");
                pressedCount++;
            }
            // --- 修改结束 ---
        }

        // 只有当按下的数量等于我们监控的总数时，才返回 true
        // 并且确保我们监控的按钮数量大于0
        return (buttonsToMonitor.Length > 0 && pressedCount == buttonsToMonitor.Length);
    }


    /// <summary>
    /// 激活所有【真正】的受控平台。
    /// </summary>
    private void ActivateRealPlatforms()
    {
        foreach (OneWayPlatform platform in platformsToControl)
        {
            if (platform != null)
            {
                Debug.Log("正在激活平台: " + platform.gameObject.name);
                // 调用【真正】平台的【真正】ActivateMovement 方法
                platform.ActivateMovement();
            }
            else
            {
                Debug.LogWarning("在 platformsToControl 列表中有一个 null 平台引用！");
            }
        }
    }
}

