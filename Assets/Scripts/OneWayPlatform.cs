using UnityEngine;
using System.Collections;

/// <summary>
/// ����ƽ̨�ӳ�ʼλ���ƶ���ָ���յ㣬Ȼ��ֹͣ��
/// </summary>
public class OneWayPlatform : MonoBehaviour
{
    public Transform endPoint; // ƽ̨��Ŀ��λ��
    public float moveSpeed = 2f; // �ƶ��ٶ�

    private Rigidbody2D rb;
    private Vector2 startPosition; // ƽ̨����ʼλ��
    private Vector2 targetPosition; // ƽ̨��Ŀ��λ��
    private bool isActivated = false; // ƽ̨�Ƿ��ѱ�����
    private bool hasArrived = false; // ƽ̨�Ƿ��ѵ����յ�

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // ����Ϊ�˶�ѧ���壬�Ա�ͨ���������

        // ��¼��ʼλ��
        startPosition = transform.position;
        targetPosition = startPosition; // ��ʼĿ����ԭ��

        if (endPoint == null)
        {
            Debug.LogError("ƽ̨ " + gameObject.name + " û������ endPoint!");
        }
    }

    void FixedUpdate()
    {
        // ֻ���ڱ���������δ�����յ�ʱ���ƶ�
        if (isActivated && !hasArrived)
        {
            // �ƶ�ƽ̨
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime));

            // ����Ƿ��ѷǳ��ӽ�Ŀ���
            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.MovePosition(targetPosition); // ��ȷ���õ�Ŀ��λ��
                hasArrived = true; // ���Ϊ�ѵ���
                Debug.Log(gameObject.name + " �ѵ����յ㲢ֹͣ��");
            }
        }
    }

    /// <summary>
    /// ����ƽ̨��ʹ�俪ʼ���յ��ƶ���
    /// </summary>
    public void ActivateMovement()
    {
        // ȷ��ֻ����һ��
        if (!isActivated && endPoint != null)
        {
            isActivated = true;
            targetPosition = endPoint.position; // ����Ŀ��Ϊ�յ�
            Debug.Log(gameObject.name + " �ѱ��������ǰ���յ㡣");
        }
    }
}
