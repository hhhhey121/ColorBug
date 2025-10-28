using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ItemSoundHandler : MonoBehaviour
{
    public AudioClip pickUpSound; // 拾取物品音效
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false; // 避免场景开始就播放
    }

    // 监听触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin")) // 只监听 Coin
        {
            PlayPickUpSound();
        }
    }

    public void PlayPickUpSound()
    {
        if (pickUpSound != null)
        {
            audioSource.PlayOneShot(pickUpSound);
        }
    }
}