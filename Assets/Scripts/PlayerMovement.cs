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

    public Transform otherPlayer;//拖入另一个玩家对象
    public float sperarationThreshold = 0.1f;//同一垂直线判定
    private bool isSeparated = false;

    public int maxJumpCountCombined = 1;
    public int maxJumpCountSplit = 2;
    private int jumpCount = 2;// 当前跳跃次数
    //public int jumpCount = 2;//做
    //public int maxJumpCount = 2;

    private bool wasGround;

    private float moveX;

    private bool moveJump;//跳跃输入

    public bool isJump;//传递作用，表跳跃状态


    private float facingDirection = 1f;


    void Start()//开始时调用一下
    {
        rb = GetComponent<Rigidbody2D>();//初始化获取组件
        anim = GetComponent<Animator>();//初始化动画

        wasGround = true;

        //初始时检查状态
        CheckSeparation();
        //根据初始状态设置跳跃次数
        jumpCount = isSeparated ? maxJumpCountSplit : maxJumpCountCombined;                                                                                                                                                       
    }

    void Update()//每渲染一帧就调用一下(input类放置
    {
        CheckSeparation();//每一帧都检查是否分离

        // 在检测跳跃输入前，先检查是否在地面上。
        // 如果在地面上，就重置跳跃次数
        // 解决被压住时跳跃次数无法刷新的BUG
        if (isGround)
        {
            jumpCount = isSeparated ? maxJumpCountSplit : maxJumpCountCombined;
        }


        moveX = Input.GetAxisRaw("Horizontal");//获取A D -1 1
        moveJump = Input.GetButtonDown("Jump");//获取W

        if (moveJump && jumpCount > 0)
        {
            isJump = true;
            
        }
    }

    //检查方法
    private void CheckSeparation()
    {
        if(otherPlayer == null) 
        {
            isSeparated = true;
            return;
        }

        //检查x轴坐标差值
        float distanceX=Mathf.Abs(transform.position.x-otherPlayer.position.x);

        //分离定义
        if(distanceX > sperarationThreshold )
        {
            isSeparated=true;
        }
        else isSeparated = false;
    }

    private void FixedUpdate()//物理类放置 固定数值
    {
        // 记录上一帧的地面状态
        wasGround = isGround;
        
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground);

        if (isGround && !wasGround)
        {
            //jumpCount = maxJumpCount;
            //落地后根据当前是否分离来重置跳跃次数
            jumpCount = isSeparated ? maxJumpCountSplit : maxJumpCountCombined;
        }

        Move();
        Jump();
    }
    private void Move()
    {
        if(anim != null) 
        {
        anim.SetFloat("speed", Mathf.Abs(moveX));//绝对值 向左跑时x为-1也生效
        }

            rb.velocity = new Vector2(moveX * playerSpeed, rb.velocity.y);

        if (moveX != 0)//在按键
        {
            // 【修改】更新朝向
            //facingDirection = Mathf.Sign(moveX);
           
            //transform.localScale = new Vector3(moveX, 1, 1);

            // 使用 Mathf.Sign 来确保 scale.x 总是 1 或 -1, 永远不会是 0
            transform.localScale = new Vector3(Mathf.Sign(moveX), 1, 1);
        }
    }

    // 添加一个公共方法，让 PlayerLaserAbility 脚本可以读取朝向
    public float GetFacingDirection()
    {
        return transform.localScale.x;
    }

    private void Jump()
    {
        //if (isGround)
        //{
        //    jumpCount = 2;
        //}

        if (isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);//在施加跳跃力之前，把Y轴速度清零,确保每次跳跃的高度都一致二段跳变得有力、跟手
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);//方向*速度
            jumpCount--;
            isJump = false;
        }

        //待做：
        //跳跃手感优化（重力因素）（长按跳的更远等等。。

    }
    
    
}

