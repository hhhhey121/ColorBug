using UnityEngine;

// ȷ��Ч������ SpriteRenderer �� Animator
[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class LaserEffectController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public float autoDestroyTime = 0.5f;

    // ��ɾ����������Ҫ Animator ����
    // private static readonly int IsAbsorbHash = Animator.StringToHash("IsAbsorb");

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spriteRenderer.enabled = false;
    }

    // �� PlayerLaserAbility ����
    public void Initialize(Vector3 startPoint, Vector3 endPoint, bool isAbsorbEffect)
    {
        // --- 1. ����λ�á���ת������ (���ֲ���) ---
        Vector3 direction = endPoint - startPoint;
        float distance = direction.magnitude;
        transform.position = startPoint;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (spriteRenderer.drawMode == SpriteDrawMode.Sliced || spriteRenderer.drawMode == SpriteDrawMode.Tiled)
        {
            spriteRenderer.size = new Vector2(distance, spriteRenderer.size.y);
        }
        else
        {
            Debug.LogWarning("LaserEffect Sprite Draw Mode is not Sliced or Tiled. Stretching may look incorrect.");
            transform.localScale = new Vector3(distance, 1, 1);
        }

        // --- 2. ���ؼ��޸ġ�ֱ�Ӳ��Ŷ��� ---
        // ���ǸĻ�ʹ�� animator.Play()������������Ч
        if (isAbsorbEffect)
        {
            animator.Play("AbsorbAnim", -1, 0f); // ǿ�ƴ�ͷ���� AbsorbAnim
        }
        else
        {
            animator.Play("FireAnim", -1, 0f); // ǿ�ƴ�ͷ���� FireAnim
        }
        // animator.Play(stateName, layer, normalizedTime);
        // layer = -1 ��ʾ�����в��ϲ���
        // normalizedTime = 0f ��ʾ�Ӷ�����ͷ����

        // ��ɾ����������Ҫ���ò���
        // animator.SetBool(IsAbsorbHash, isAbsorbEffect);

        // --- 3. �����ɼ� (�� Play ֮������ʾ��ȷ���������ǵ�һ֡) ---
        spriteRenderer.enabled = true; // �����ɼ�

        // --- 4. �ƻ��������� (���ֲ���) ---
        Destroy(gameObject, autoDestroyTime);
    }
}