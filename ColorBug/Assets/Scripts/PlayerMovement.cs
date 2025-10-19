using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    Animator anim;

    public float playerSpeed = 5f;
    [Range(1, 10)]
    public float jumpSpeed = 5f;

    public bool isGround;//已经在地面上

    public Transform groundCheck;//检测点

    public LayerMask ground;//图层

    public int jumpCount = 2;//跳跃次数

    private float moveX;

    private bool moveJump;//跳跃输入

    bool isJump;//传递作用，表跳跃状态


    void Start()//开始时调用一下
    {
        rb = GetComponent<Rigidbody2D>();//初始化获取组件
        anim = GetComponent<Animator>();//初始化动画
    }

    void Update()//每渲染一帧就调用一下(input类放置
    {
        moveX = Input.GetAxisRaw("Horizontal");//获取A D -1 1
        moveJump = Input.GetButtonDown("Jump");

        if (moveJump && jumpCount > 0)
        {
            isJump = true;

        }
    }

    private void FixedUpdate()//物理类放置 固定数值
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground);
        Move();
        Jump();
    }
    private void Move()
    {
        anim.SetFloat("speed", Mathf.Abs(moveX));//绝对值 向左跑时x为-1也生效

        rb.velocity = new Vector2(moveX * playerSpeed, rb.velocity.y);

        if (moveX != 0)//在按键
        {
            transform.localScale = new Vector3(moveX, 1, 1);
        }
    }

    private void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
        }

        if (isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);//在施加跳跃力之前，先把Y轴速度清零,确保每次跳跃的高度都一致二段跳变得有力、跟手
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);//方向*速度
            jumpCount--;
            isJump = false;
        }

        //待做：
        //跳跃手感优化（重力因素）（长按跳的更远等等。。

    }
}

