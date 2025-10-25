using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public enum CharacterState { Combined, Split }
    public CharacterState currentState = CharacterState.Combined;//��ʼ״̬��ֹ

    public GameObject partA; // ����A����player2
    public GameObject partB; // ����B��С��player
    public KeyCode splitCombineKey = KeyCode.Space;

    public float combineDistance = 1.2f; // �Զ���ϵĴ�������
    public float combineCooldown = 0.5f;
    private float lastCombineTime = 0f;

    public float partAMass = 2f; // ����A������
    public float partBMass = 1f; // ����B������
    public float partASpeed = 4f; // ����A���ƶ��ٶ�
    public float partBSpeed = 4f; // ����B���ƶ��ٶ�
    public float partAJumpSpeed = 10f; // ����A����Ծ�ٶ�
    public float partBJumpSpeed = 5f; // ����B����Ծ�ٶ�

    //���ʱ��ƫ��λ��
    public Vector3 combinedOffset = new Vector3(0, 1, 0);

    // �������
    private Rigidbody2D rbA;
    private Rigidbody2D rbB;
    private PlayerMovement playerMovementA;
    private PlayerMovement playerMovementB;

    


    void Start()
    {
        // ��ȡ����������������
        if (partA != null)
        {
            rbA = partA.GetComponent<Rigidbody2D>();
            playerMovementA = partA.GetComponent<PlayerMovement>();
            // ��ʼ��ʱ���ò���A����������
            if (rbA != null) rbA.mass = partAMass;
            if (playerMovementA != null)
            {
                playerMovementA.playerSpeed = partASpeed;
                playerMovementA.jumpSpeed = partAJumpSpeed;
                playerMovementA.enabled = (currentState == CharacterState.Combined); // ��ʼ���ʱֻ����A�Ŀ���
            }
        }

        if (partB != null)
        {
            rbB = partB.GetComponent<Rigidbody2D>();
            playerMovementB = partB.GetComponent<PlayerMovement>();
            // ��ʼ��ʱ���ò���B����������
            if (rbB != null) rbB.mass = partBMass;
            if (playerMovementB != null)
            {
                playerMovementB.playerSpeed = partBSpeed;
                playerMovementB.jumpSpeed = partBJumpSpeed;
                playerMovementB.enabled = false; // ��ʼ���ʱ����B�Ŀ���
            }
        }

        // ��ʼΪ���״̬��������B��λ�����õ�����A
        if (currentState == CharacterState.Combined && partA != null && partB != null)
        {
            //��Ӹ��ӹ�ϵ��
            partB.transform.SetParent(partA.transform);
            partB.transform.localPosition = combinedOffset;
            partB.transform.localRotation = Quaternion.identity;
            if (rbB != null) rbB.isKinematic = true; // ���ʱ����B��������Ӱ��
        }
    }

    void Update()
    {
        // ������/��ϰ�������
        if (Input.GetKeyDown(splitCombineKey) && Time.time > lastCombineTime + combineCooldown)
        {
            ToggleSplitCombine();
        }





        // ����ڷ���״̬������Ƿ������Զ��������
        if (currentState == CharacterState.Split)
        {
            CheckCombineCondition();
        }
    }



    

  




    /// <summary>
    /// �л�����/���״̬
    /// </summary>
    void ToggleSplitCombine()
    {
        if (currentState == CharacterState.Combined)
        {
            SplitCharacter();
        }
        else
        {
            TryCombine();
        }
    }

    /// <summary>
    /// �����ɫ
    /// </summary>
    void SplitCharacter()
    {
        if (partA == null || partB == null) return;

        currentState = CharacterState.Split;
        lastCombineTime = Time.time;

        partB.transform.SetParent(null);

        // �����������ֵĶ�������
        if (playerMovementA != null) playerMovementA.enabled = true;
        if (playerMovementB != null)
        {
            playerMovementB.enabled = true;
            // ȷ������B�ָ�����ģ��
            if (rbB != null) rbB.isKinematic = false;




        }


        // ���Ը���������һ����΢�ķ�������ʹ����Ȼ�ֿ�
        if (rbA != null) rbA.AddForce(new Vector2(-1, 0.5f) * 2f, ForceMode2D.Impulse);
        if (rbB != null) rbB.AddForce(new Vector2(1, 0.5f) * 2f, ForceMode2D.Impulse);

        Debug.Log("��ɫ�ѷ��룡");
    }




    



    /// <summary>
    /// ������Ͻ�ɫ
    /// </summary>
    void TryCombine()
    {
        if (partA == null || partB == null) return;

        // �򵥵ľ����⣬������������Ӹ������������������ٶȡ��ض�����ȣ�
        float distance = Vector2.Distance(partA.transform.position, partB.transform.position);
        // ��ӵ�����䣬���combineDistance��ֵ
        Debug.Log("��ǰ��Ͼ����趨: " + combineDistance + ", ʵ�ʾ���: " + distance);

        if (distance <= combineDistance)
        {
            CombineCharacter();
        }
        else
        {
            Debug.Log("�����־����Զ���޷���ϡ���ǰ����: " + distance.ToString("F2"));
        }
        
    }

    /// <summary>
    /// ��Ͻ�ɫ
    /// </summary>
    void CombineCharacter()
    {
        currentState = CharacterState.Combined;
        lastCombineTime = Time.time;


       


        //// ���ò���B�Ķ������ƣ���Ϻ��Բ���AΪ����
        if (playerMovementB != null) playerMovementB.enabled = false;
        // ���ʱ����B��������Ӱ�죬���沿��A
        if (rbB != null) rbB.isKinematic = true;

        // ������B��λ�����õ�����Aͷ��
        if (partA != null && partB != null)
        {
            partB.transform.SetParent(partA.transform);
            partB.transform.localPosition = new Vector3(0, 1, 0);// ����A��ͷ����y��������1����λ
            partB.transform.localRotation = Quaternion.identity;
        }




        Debug.Log("��ɫ����ϣ�");
    }










    /// <summary>
    /// ����Զ��������
    /// </summary>
    void CheckCombineCondition()
    {
        if (partA == null || partB == null) return;

        float distance = Vector2.Distance(partA.transform.position, partB.transform.position);
        if (distance <= combineDistance && Time.time > lastCombineTime + combineCooldown)
        {
            // �Զ������ʾ
            Debug.Log("�����Զ�������������ո�����");
        }
    }

    // ��ѡ�ģ���Scene��ͼ�л���Gizmos�Ա��ڵ���
    void OnDrawGizmosSelected()
    {
        if (partA != null && partB != null)
        {
            // ������Ͼ��뷶Χ
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(partA.transform.position, combineDistance);
            // ����������֮�������
            Gizmos.color = currentState == CharacterState.Combined ? Color.green : Color.white;
            Gizmos.DrawLine(partA.transform.position, partB.transform.position);
        }
    }
}

