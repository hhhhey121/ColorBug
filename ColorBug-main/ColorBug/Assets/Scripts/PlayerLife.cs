using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
   
    public GameObject playerPS; // ������Ч

    private Animator anim;
    private Rigidbody2D rb;
    public bool isDead = false; // ״̬����ֹ�ظ�����

    // ����������������
    private RespawnManager respawnManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // ����Ϸ��ʼʱ�Զ����ҹ�����
        respawnManager = FindObjectOfType<RespawnManager>();
        if (respawnManager == null)
        {
            Debug.LogError("������δ�ҵ� RespawnManager��");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return; // ����Ѿ����ˣ��Ͳ�Ҫ�ٴ�����

        if (collision.gameObject.CompareTag("Trap"))
        {
            Death();
        }
    }

    private void Death()
    {
        isDead = true; // ���Ϊ����
        rb.bodyType = RigidbodyType2D.Static; // ������ԭ�����߼������徲ֹ
        anim.SetTrigger("death"); // ������ԭ�����߼���������������

       
        if (playerPS != null)
        {
            // �ڽ�ɫλ������ "playerPS" ��Ч
            GameObject deathParticles = Instantiate(playerPS, transform.position, Quaternion.identity);

            // 1��� *ֻ������������ɵ���Ч*
            // (��ԭ���Ĵ��� 'Destroy(playerPS, 1f)' ����ͼ����Ԥ���壬�Ǵ����)
            Destroy(deathParticles, 1f);
        }
        // -----------------------------------------------------

        //֪ͨ���������������Լ� (���ǲ����ٽ�ɫ)

        // ���ؽ�ɫ�� Sprite
        var sprite = GetComponentInChildren<SpriteRenderer>();
        if (sprite != null) sprite.enabled = false;

        // ������ײ��
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;


        // ֪ͨ�������������ˡ�
        if (respawnManager != null)
        {
            respawnManager.HandlePlayerDeath(this);
        }
    }


    // �������� (�� RespawnManager ����)
    public void Respawn(Vector3 respawnPosition)
    {
        Debug.Log(gameObject.name + " �� " + respawnPosition + " ������");

        // �ƶ���������
        transform.position = respawnPosition;

        // �ָ�����״̬
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero; // �����ٶ�

        // �ָ��Ӿ�
        var sprite = GetComponentInChildren<SpriteRenderer>();
        if (sprite != null) sprite.enabled = true;

        // �ָ���ײ
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = true;

        // ����״̬
        isDead = false;

    }
}