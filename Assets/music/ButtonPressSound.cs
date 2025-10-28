using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonPressSound : MonoBehaviour
{
    [Header("按钮音效设置")]
    public AudioClip pressSound;     // 拖入音效
    [Range(0f, 1f)]
    public float volume = 1f;

    private AudioSource audioSource;
    private OneTimeButton buttonScript;
    private bool hasPlayed = false;  // 防止重复播放

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        buttonScript = GetComponent<OneTimeButton>();
        if (buttonScript == null)
        {
            Debug.LogWarning("⚠️ ButtonPressSound 未找到 OneTimeButton 组件！");
        }
    }

    void Update()
    {
        // 检测按钮是否被按下
        if (buttonScript != null && IsButtonPressedOnce() && !hasPlayed)
        {
            PlayPressSound();
            hasPlayed = true;
        }
    }

    bool IsButtonPressedOnce()
    {
        // 访问 OneTimeButton 中的 isPressedOnce 状态（通过反射或修改为 public）
        var field = typeof(OneTimeButton).GetField("isPressedOnce",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        return (bool)field.GetValue(buttonScript);
    }

    void PlayPressSound()
    {
        if (pressSound != null)
        {
            audioSource.PlayOneShot(pressSound, volume);
            Debug.Log("🔊 按钮音效播放！");
        }
    }
}