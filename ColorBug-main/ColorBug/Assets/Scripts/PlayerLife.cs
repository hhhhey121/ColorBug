using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public GameObject playerPS; // ������Ч
    private Animator anim;
    private Rigidbody2D rb;
    public bool isDead = false;
    private RespawnManager respawnManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        respawnManager = FindObjectOfType<RespawnManager>();
        if (respawnManager == null)
        {
            Debug.LogError("������δ�ҵ� RespawnManager��");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Trap"))
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true;
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death"); // ������������

        if (playerPS != null)
        {
            GameObject deathParticles = Instantiate(playerPS, transform.position, Quaternion.identity);
            Destroy(deathParticles, 1f);
        }

        // ���� Sprite (ʹ�� GetComponentInChildren)
        var sprite = GetComponentInChildren<SpriteRenderer>();
        if (sprite != null) sprite.enabled = false;

        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;

        if (respawnManager != null)
        {
            respawnManager.HandlePlayerDeath(this);
        }
    }

    // �������� (�� RespawnManager ����)
    public void Respawn(Vector3 respawnPosition)
    {
        // ����� respawnPosition �Ѿ��Ǳ� RespawnManager ������Z���
        Debug.Log(gameObject.name + " �� " + respawnPosition + " (Z��������) ������");

        transform.position = respawnPosition;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;

        // �ָ� Sprite
        var sprite = GetComponentInChildren<SpriteRenderer>();
        if (sprite != null) sprite.enabled = true;

        // �ָ���ײ
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = true;

        isDead = false;

        // -----------------------------------------------------
        // !!! ���ؼ��޸� 2�� !!!
        // ���ö���״̬������ֹ����"Die"������͸��֡
        // ����Ҫ����� Animator (����������) �У�
        // 1. ����һ���µ� Trigger (������)������Ϊ "Respawn"
        // 2. ����һ���� "Die" ״̬�� "Idle" (�����Ĭ��) ״̬�Ĺ��� (Transition)
        // 3. ��������ɵ����� (Condition) ����Ϊ "Respawn" ������
        // -----------------------------------------------------
        anim.SetTrigger("Respawn");
    }
}