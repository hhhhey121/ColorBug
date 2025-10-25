using UnityEngine;


//bug��ƽ̨�ƶ�
public class MovingPlatform : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 targetPosition;

    // ��¼ƽ̨�Ƿ�����ǰ���յ�
    private bool movingToEnd = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        if (startPoint == null || endPoint == null)
        {
            Debug.LogError("ƽ̨ " + gameObject.name + " û������ startPoint �� endPoint!");
            return;
        }

        transform.position = startPoint.position;
        targetPosition = startPoint.position;
    }

    void FixedUpdate()
    {
        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
    }

    

    // --- ���Ƿ��������л����أ�ʹ�õķ��� ---
    public void ToggleTarget()
    {
        // �л�Ŀ��
        movingToEnd = !movingToEnd;
        targetPosition = movingToEnd ? (Vector2)endPoint.position : (Vector2)startPoint.position;
    }
}