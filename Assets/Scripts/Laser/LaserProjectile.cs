using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class LaserProjectile : MonoBehaviour//�ӵ��ű�
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

    // ���޸ġ�����߼�
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �����������ȼ����ɫ����
        if (other.CompareTag("BlueSquare"))
        {
            BlueSquare square = other.GetComponent<BlueSquare>();
            if (square != null)
            {
                square.BeDestroyed(); // �����ݻ�
            }
            Destroy(gameObject); // �����ӵ�
            return; // ����
        }

        // ���������塱�򡰵��桱������
        if (other.CompareTag("Trap") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}