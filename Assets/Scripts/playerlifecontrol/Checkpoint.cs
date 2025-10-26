using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    public bool isOneTimeUse = true;
    private bool hasBeenTriggered = false;

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isOneTimeUse && hasBeenTriggered) return;

        // ȷ�����������ɫ���� "Player" ��ǩ (Tag)
        if (other.CompareTag("Player"))
        {
            RespawnManager.Instance.UpdateCheckpoint(transform.position);
            hasBeenTriggered = true;
            // (��ѡ) ������ײ��
            // GetComponent<Collider2D>().enabled = false; 
        }
    }
}