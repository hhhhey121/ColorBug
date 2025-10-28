using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ItemSoundHandler : MonoBehaviour
{
    public AudioClip pickUpSound; // ʰȡ��Ʒ��Ч
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false; // ���ⳡ����ʼ�Ͳ���
    }

    // ����������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin")) // ֻ���� Coin
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