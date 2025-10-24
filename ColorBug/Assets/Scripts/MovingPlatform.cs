using UnityEngine;


//bug版平台移动
public class MovingPlatform : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Vector2 targetPosition;

    // 记录平台是否正在前往终点
    private bool movingToEnd = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        if (startPoint == null || endPoint == null)
        {
            Debug.LogError("平台 " + gameObject.name + " 没有设置 startPoint 或 endPoint!");
            return;
        }

        transform.position = startPoint.position;
        targetPosition = startPoint.position;
    }

    void FixedUpdate()
    {
        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
    }

    

    // --- 这是方案二（切换开关）使用的方法 ---
    public void ToggleTarget()
    {
        // 切换目标
        movingToEnd = !movingToEnd;
        targetPosition = movingToEnd ? (Vector2)endPoint.position : (Vector2)startPoint.position;
    }
}