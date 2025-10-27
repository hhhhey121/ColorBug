using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class LaserProjectile : MonoBehaviour//子弹脚本
{
    public float speed = 20f;
    public float lifetime = 2f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(float direction)
    {
        rb.velocity = new Vector2(speed * direction, 0);
    }

    // 【修改】检测逻辑
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 【新增】优先检测蓝色方块
        if (other.CompareTag("BlueSquare"))
        {
            BlueSquare square = other.GetComponent<BlueSquare>();
            if (square != null)
            {
                square.BeDestroyed(); // 命令方块摧毁
            }
            Destroy(gameObject); // 销毁子弹
            return; // 结束
        }

        // 碰到“陷阱”或“地面”就销毁
        if (other.CompareTag("Trap") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}