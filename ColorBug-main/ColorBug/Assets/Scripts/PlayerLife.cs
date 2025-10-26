using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//���س���

public class PlayerLife : MonoBehaviour
{
    public GameObject playerPS;//������Ч

    private Animator anim;
    private Rigidbody2D rb;
    void Start()
    {
        anim=GetComponent<Animator>();//���붯��
        rb= GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Trap")//������ǩΪTrap������ʱ��������������
        {
            Death();
        }
    }

    private void Death()
    {
        rb.bodyType = RigidbodyType2D.Static;//������rigibodytype��Ϊ��ֹ
        Destroy(playerPS, 1f);//1���ɾ�� 
        anim.SetTrigger("death");
    }
    private void Restart()//���¼��س���
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//���ǰ����
    }
}
