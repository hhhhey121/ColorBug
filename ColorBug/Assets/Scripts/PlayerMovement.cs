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

    public bool isGround;//�Ѿ��ڵ�����
    public Transform groundCheck;//����
    public LayerMask ground;//ͼ��

    public Transform otherPlayer;//������һ����Ҷ���
    public float sperarationThreshold = 0.1f;//ͬһ��ֱ���ж�
    private bool isSeparated = false;

    public int maxJumpCountCombined = 1;
    public int maxJumpCountSplit = 2;
    private int jumpCount = 2;// ��ǰ��Ծ����

    private bool wasGround;
    private float moveX;
    private bool moveJump;//��Ծ����
    public bool isJump;//�������ã�����Ծ״̬

    // --- ������Ϊ����߽����������Ĵ��� ---
    [Header("����߽�����")]
    public Camera mainCamera;
    [Tooltip("��Ҿ�����Ļ��Ե����С���")]
    public float screenEdgePadding = 0.5f;
    private float halfScreenWidth;
    // --- ����������� ---


    void Start()//��ʼʱ����һ��
    {
        rb = GetComponent<Rigidbody2D>();//��ʼ����ȡ���
        anim = GetComponent<Animator>();//��ʼ������

        wasGround = true;

        //��ʼʱ���״̬
        CheckSeparation();
        //���ݳ�ʼ״̬������Ծ����
        jumpCount = isSeparated ? maxJumpCountSplit : maxJumpCountCombined;

        // --- ������Ϊ����߽����������Ĵ��� ---
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera != null && mainCamera.orthographic)
        {
            // ���������������������Ļ��ȵ����絥λ
            halfScreenWidth = mainCamera.orthographicSize * mainCamera.aspect;
        }
        else
        {
            Debug.LogWarning("�����δ���û��������������Ե�����߼������޷�����������", this);
        }
        // --- ����������� ---
    }

    void Update()//ÿ��Ⱦһ֡�͵���һ��(input�����
    {
        CheckSeparation();//ÿһ֡������Ƿ����

        moveX = Input.GetAxisRaw("Horizontal");//��ȡA D -1 1
        moveJump = Input.GetButtonDown("Jump");//��ȡW

        if (moveJump && jumpCount > 0)
        {
            isJump = true;
        }
    }

    //��鷽��
    private void CheckSeparation()
    {
        if (otherPlayer == null)
        {
            isSeparated = true;
            return;
        }

        //���x�������ֵ
        float distanceX = Mathf.Abs(transform.position.x - otherPlayer.position.x);

        //���붨��
        if (distanceX > sperarationThreshold)
        {
            isSeparated = true;
        }
        else isSeparated = false;
    }

    private void FixedUpdate()//��������� �̶���ֵ
    {
        // ��¼��һ֡�ĵ���״̬
        wasGround = isGround;

        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground);

        if (isGround && !wasGround)
        {
            //��غ���ݵ�ǰ�Ƿ������������Ծ����
            jumpCount = isSeparated ? maxJumpCountSplit : maxJumpCountCombined;
        }

        Move();
        Jump();
    }

    private void Move()
    {
        if (anim != null)
        {
            anim.SetFloat("speed", Mathf.Abs(moveX));//����ֵ ������ʱxΪ-1Ҳ��Ч
        }

        rb.velocity = new Vector2(moveX * playerSpeed, rb.velocity.y);

        if (moveX != 0)//�ڰ���
        {
            transform.localScale = new Vector3(moveX, 1, 1);
        }
    }

    private void Jump()
    {
        if (isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);//��ʩ����Ծ��֮ǰ����Y���ٶ�����,ȷ��ÿ����Ծ�ĸ߶ȶ�һ�¶������������������
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);//����*�ٶ�
            jumpCount--;
            isJump = false;
        }
    }

    // --- ������Ϊ����߽����������� *��������* ---

    /// <summary>
    /// ������������߼����º����У����������������λ��
    /// </summary>
    private void LateUpdate()
    {
        if (mainCamera == null) return;

        // 1. ʵʱ��ȡ����߽�
        // (��LateUpdate�����¼���halfScreenWidth���Է����ڴ�С����Ҹı�)
        halfScreenWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float cameraX = mainCamera.transform.position.x;
        float leftEdge = cameraX - halfScreenWidth;
        float rightEdge = cameraX + halfScreenWidth;

        // 2. ������ҵ�λ��
        // ��Ϊ����ʹ�� Rigidbody�����Զ�ȡ rb.position
        Vector2 currentPos = rb.position;

        float clampedX = Mathf.Clamp(currentPos.x,
                                    leftEdge + screenEdgePadding,
                                    rightEdge - screenEdgePadding);

        // 3. ֻ����λ�� *���* ������ʱ�Ÿ��£��Ա��ⲻ��Ҫ���������
        if (currentPos.x != clampedX)
        {
            // ʹ�� rb.MovePosition ����ȫ���ƶ� Rigidbody���ⲻ���ƻ�����ģ��
            rb.MovePosition(new Vector2(clampedX, currentPos.y));

            // (��ѡ) �����ϣ�����ײ��������ǽ��ʱ����ֹͣˮƽ�ٶȣ�����ȡ����������ע��
            // rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    // --- ������������ ---
}