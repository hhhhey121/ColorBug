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

        // 确保你的两个角色都有 "Player" 标签 (Tag)
        if (other.CompareTag("Player"))
        {
            RespawnManager.Instance.UpdateCheckpoint(transform.position);
            hasBeenTriggered = true;
            // (可选) 禁用碰撞体
            // GetComponent<Collider2D>().enabled = false; 
        }
    }
}