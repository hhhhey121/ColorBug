using UnityEngine;

/// <summary>
/// ��ض�� OneTimeButton�������а�ť�������º󣬼��������ܿص� OneWayPlatform��
/// ����һ������׳�İ汾������������ OneTimeButton �ϵ� 'sr' �ֶ��Ƿ��� Inspector �б���ֵ��
/// </summary>
public class MultiButtonController : MonoBehaviour
{
    // Ŀ�꣺��Ҫ��صİ�ť
    public OneTimeButton[] buttonsToMonitor;

    // Ŀ�꣺��Ҫ���Ƶġ�������ƽ̨��ǽ�壩
    public OneWayPlatform[] platformsToControl;

    // ȷ��ƽֻ̨������һ��
    private bool hasActivated = false;

    /// <summary>
    /// ÿ֡��鰴ť״̬
    /// </summary>
    void Update()
    {
        // ���ƽ̨�Ѿ�������ˣ���û��Ҫ�ټ����
        if (hasActivated)
        {
            return;
        }

        // ������а�ť�Ƿ��ѱ�����
        if (AreAllButtonsPressed())
        {
            Debug.Log("--- (��׳��) ���а�ť���Ѱ��£�����ƽ̨��---");
            ActivateRealPlatforms();
            hasActivated = true; // ���Ϊ�Ѽ��ֹͣ���
        }
    }

    /// <summary>
    /// �������а�ť����������Ƿ񶼴��ڡ����¡�״̬
    /// </summary>
    /// <returns>������а�ť�������ˣ����� true</returns>
    private bool AreAllButtonsPressed()
    {
        if (buttonsToMonitor == null || buttonsToMonitor.Length == 0)
        {
            return false; // û�а�ť�ɼ��
        }

        int pressedCount = 0;

        foreach (OneTimeButton button in buttonsToMonitor)
        {
            // --- �ؼ��޸ģ�����׳�ļ�鷽ʽ ---

            // 1. ȷ����ť�ű��������
            if (button == null) continue;

            // 2. �Ӱ�ť�ű���ȡ���������� "pressedSprite"
            //    ���� OneTimeButton �ϵ� public �ֶΣ����ǿ��Զ�ȡ����
            //    ���Ǳ���ȷ������ֶ��� Inspector �б���ֵ�ˣ�
            Sprite spriteToMatch = button.pressedSprite;

            // 3. (����) ���ǲ�ʹ�� "button.sr"����Ϊ������û���� Inspector �б���ֵ��
            //    �����Լ�ȥ��ť���ڵ� GameObject ��Ѱ�� SpriteRenderer �����
            SpriteRenderer rendererOnButton = button.GetComponent<SpriteRenderer>();

            // 4. ��������Ƿ��ҵ���������Ҫ�Ķ���
            if (rendererOnButton == null || rendererOnButton.sprite == null || spriteToMatch == null)
            {
                // ��������ťû�� SpriteRenderer�����ߵ�ǰû�� sprite��
                // �������ǲ�֪�� "pressed" sprite ��ʲô��(spriteToMatch == null)����������
                if (spriteToMatch == null)
                {
                    Debug.LogWarning("��ť " + button.gameObject.name + " û���� OneTimeButton ��������� 'Pressed Sprite'!");
                }
                continue;
            }

            // 5. �Ƚϵ�ǰ sprite �����ƺ� "pressed" sprite ������
            if (rendererOnButton.sprite.name == spriteToMatch.name)
            {
                // Debug.Log(button.gameObject.name + " �Ѽ��Ϊ����״̬��");
                pressedCount++;
            }
            // --- �޸Ľ��� ---
        }

        // ֻ�е����µ������������Ǽ�ص�����ʱ���ŷ��� true
        // ����ȷ�����Ǽ�صİ�ť��������0
        return (buttonsToMonitor.Length > 0 && pressedCount == buttonsToMonitor.Length);
    }


    /// <summary>
    /// �������С����������ܿ�ƽ̨��
    /// </summary>
    private void ActivateRealPlatforms()
    {
        foreach (OneWayPlatform platform in platformsToControl)
        {
            if (platform != null)
            {
                Debug.Log("���ڼ���ƽ̨: " + platform.gameObject.name);
                // ���á�������ƽ̨�ġ�������ActivateMovement ����
                platform.ActivateMovement();
            }
            else
            {
                Debug.LogWarning("�� platformsToControl �б�����һ�� null ƽ̨���ã�");
            }
        }
    }
}

