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
    //public int jumpCount = 2;//��Ծ����
    //public int maxJumpCount = 2;

    private bool wasGround;

    private float moveX;

    private bool moveJump;//��Ծ����

    public bool isJump;//�������ã�����Ծ״̬


    void Start()//��ʼʱ����һ��
    {
        rb = GetComponent<Rigidbody2D>();//��ʼ����ȡ���
        anim = GetComponent<Animator>();//��ʼ������

        wasGround = true;

        //��ʼʱ���״̬
        CheckSeparation();
        //���ݳ�ʼ״̬������Ծ����
        jumpCount = isSeparated ? maxJumpCountSplit : maxJumpCountCombined;                                                                                                                                                       
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
        if(otherPlayer == null) 
        {
            isSeparated = true;
            return;
        }

        //���x�������ֵ
        float distanceX=Mathf.Abs(transform.position.x-otherPlayer.position.x);

        //���붨��
        if(distanceX > sperarationThreshold )
        {
            isSeparated=true;
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
            //jumpCount = maxJumpCount;
            //��غ���ݵ�ǰ�Ƿ������������Ծ����
            jumpCount = isSeparated ? maxJumpCountSplit : maxJumpCountCombined;
        }

        Move();
        Jump();
    }
    private void Move()
    {
        if(anim != null) 
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
        //if (isGround)
        //{
        //    jumpCount = 2;
        //}

        if (isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);//��ʩ����Ծ��֮ǰ����Y���ٶ�����,ȷ��ÿ����Ծ�ĸ߶ȶ�һ�¶������������������
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);//����*�ٶ�
            jumpCount--;
            isJump = false;
        }

        //������
        //��Ծ�ָ��Ż����������أ����������ĸ�Զ�ȵȡ���

    }
    
    
}

