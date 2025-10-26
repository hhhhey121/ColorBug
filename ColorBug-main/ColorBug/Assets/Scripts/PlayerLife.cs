using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//加载场景

public class PlayerLife : MonoBehaviour
{
    public GameObject playerPS;//粒子特效

    private Animator anim;
    private Rigidbody2D rb;
    void Start()
    {
        anim=GetComponent<Animator>();//引入动画
        rb= GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Trap")//碰到标签为Trap的陷阱时，触发死亡动画
        {
            Death();
        }
    }

    private void Death()
    {
        rb.bodyType = RigidbodyType2D.Static;//死亡后rigibodytype改为静止
        Destroy(playerPS, 1f);//1秒后删除 
        anim.SetTrigger("death");
    }
    private void Restart()//重新加载场景
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//激活当前场景
    }
}
