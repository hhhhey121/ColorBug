using UnityEngine;
using System.Collections;

/// <summary>
/// 控制平台从初始位置移动到指定终点，然后停止。
/// </summary>
public class OneWayPlatform : MonoBehaviour
{
    public Transform endPoint; // 平台的目标位置
    public float moveSpeed = 2f; // 移动速度

    private Rigidbody2D rb;
    private Vector2 startPosition; // 平台的起始位置
    private Vector2 targetPosition; // 平台的目标位置
    private bool isActivated = false; // 平台是否已被激活
    private bool hasArrived = false; // 平台是否已到达终点

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // 设置为运动学刚体，以便通过代码控制

        // 记录起始位置
        startPosition = transform.position;
        targetPosition = startPosition; // 初始目标是原地

        if (endPoint == null)
        {
            Debug.LogError("平台 " + gameObject.name + " 没有设置 endPoint!");
        }
    }

    void FixedUpdate()
    {
        // 只有在被激活且尚未到达终点时才移动
        if (isActivated && !hasArrived)
        {
            // 移动平台
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime));

            // 检查是否已非常接近目标点
            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.MovePosition(targetPosition); // 精确设置到目标位置
                hasArrived = true; // 标记为已到达
                Debug.Log(gameObject.name + " 已到达终点并停止。");
            }
        }
    }

    /// <summary>
    /// 激活平台，使其开始向终点移动。
    /// </summary>
    public void ActivateMovement()
    {
        // 确保只激活一次
        if (!isActivated && endPoint != null)
        {
            isActivated = true;
            targetPosition = endPoint.position; // 设置目标为终点
            Debug.Log(gameObject.name + " 已被激活，正在前往终点。");
        }
    }
}
