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

    private bool wasGround;
    private float moveX;
    private bool moveJump;//跳跃输入
    public bool isJump;//传递作用，表跳跃状态

    // --- 以下是为相机边界限制新增的代码 ---
    [Header("相机边界限制")]
    public Camera mainCamera;
    [Tooltip("玩家距离屏幕边缘的最小间距")]
    public float screenEdgePadding = 0.5f;
    private float halfScreenWidth;
    // --- 新增代码结束 ---


    void Start()//开始时调用一下
    {
        rb = GetComponent<Rigidbody2D>();//初始化获取组件
        anim = GetComponent<Animator>();//初始化动画

        wasGround = true;

        //初始时检查状态
        CheckSeparation();
        //根据初始状态设置跳跃次数
        jumpCount = isSeparated ? maxJumpCountSplit : maxJumpCountCombined;

        // --- 以下是为相机边界限制新增的代码 ---
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera != null && mainCamera.orthographic)
        {
            // 对于正交相机，计算半个屏幕宽度的世界单位
            halfScreenWidth = mainCamera.orthographicSize * mainCamera.aspect;
        }
        else
        {
            Debug.LogWarning("主相机未设置或不是正交相机！边缘限制逻辑可能无法正常工作。", this);
        }
        // --- 新增代码结束 ---
    }

    void Update()//每渲染一帧就调用一下(input类放置
    {
        CheckSeparation();//每一帧都检查是否分离

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
        if (otherPlayer == null)
        {
            isSeparated = true;
            return;
        }

        //检查x轴坐标差值
        float distanceX = Mathf.Abs(transform.position.x - otherPlayer.position.x);

        //分离定义
        if (distanceX > sperarationThreshold)
        {
            isSeparated = true;
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
            //落地后根据当前是否分离来重置跳跃次数
            jumpCount = isSeparated ? maxJumpCountSplit : maxJumpCountCombined;
        }

        Move();
        Jump();
    }

    private void Move()
    {
        if (anim != null)
        {
            anim.SetFloat("speed", Mathf.Abs(moveX));//绝对值 向左跑时x为-1也生效
        }

        rb.velocity = new Vector2(moveX * playerSpeed, rb.velocity.y);

        if (moveX != 0)//在按键
        {
            transform.localScale = new Vector3(moveX, 1, 1);
        }
    }

    private void Jump()
    {
        if (isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);//在施加跳跃力之前，把Y轴速度清零,确保每次跳跃的高度都一致二段跳变得有力、跟手
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);//方向*速度
            jumpCount--;
            isJump = false;
        }
    }

    // --- 以下是为相机边界限制新增的 *整个函数* ---

    /// <summary>
    /// 在所有物理和逻辑更新后运行，用于最终限制玩家位置
    /// </summary>
    private void LateUpdate()
    {
        if (mainCamera == null) return;

        // 1. 实时获取相机边界
        // (在LateUpdate里重新计算halfScreenWidth，以防窗口大小被玩家改变)
        halfScreenWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float cameraX = mainCamera.transform.position.x;
        float leftEdge = cameraX - halfScreenWidth;
        float rightEdge = cameraX + halfScreenWidth;

        // 2. 限制玩家的位置
        // 因为我们使用 Rigidbody，所以读取 rb.position
        Vector2 currentPos = rb.position;

        float clampedX = Mathf.Clamp(currentPos.x,
                                    leftEdge + screenEdgePadding,
                                    rightEdge - screenEdgePadding);

        // 3. 只有在位置 *真的* 被限制时才更新，以避免不必要的物理计算
        if (currentPos.x != clampedX)
        {
            // 使用 rb.MovePosition 来安全地移动 Rigidbody，这不会破坏物理模拟
            rb.MovePosition(new Vector2(clampedX, currentPos.y));

            // (可选) 如果你希望玩家撞到“空气墙”时立即停止水平速度，可以取消下面这行注释
            // rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    // --- 新增函数结束 ---
}