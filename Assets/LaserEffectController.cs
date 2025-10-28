using UnityEngine;

// ȷ��Ч������ SpriteRenderer �� Animator
[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class LaserEffectController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // (��ѡ) ����������Ϻ��Զ����ٵ�ʱ��
    // ��Ҳ�����ڶ����������ʱ����¼�������
    public float autoDestroyTime = 0.5f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        // ȷ��һ��ʼ�ǿ������ģ��ȴ�����ʼ��
        spriteRenderer.enabled = false;
    }

    // �� PlayerLaserAbility ����
    public void Initialize(Vector3 startPoint, Vector3 endPoint, bool isAbsorbEffect)
    {
        spriteRenderer.enabled = true; // �����ɼ�

        // --- 1. ����λ�á���ת������ ---
        Vector3 direction = endPoint - startPoint;
        float distance = direction.magnitude;

        // a) ����λ�� (�������)
        transform.position = startPoint;

        // b) ������ת (�����յ�)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // c) �������� (ֻ���� X ��)
        //    ����Ҫ����Ҫ����� Sprite Renderer �� Draw Mode ����Ϊ Sliced �� Tiled
        //    ������� Sprite ͼƬ����֧������ (�����м��Ǵ�ɫ���ظ�ͼ��)
        //    ���Ǽ��� Sprite ��ԭʼ���ȶ�Ӧ Scale X = 1
        //    ����Ҫ������� Sprite ��������߼�
        if (spriteRenderer.drawMode == SpriteDrawMode.Sliced || spriteRenderer.drawMode == SpriteDrawMode.Tiled)
        {
            // ���� Sliced/Tiled ģʽ�������޸� size ������ scale
            spriteRenderer.size = new Vector2(distance, spriteRenderer.size.y);
        }
        else // ����� Simple ģʽ (���Ƽ���������)
        {
            // ����Ҫ֪�� Sprite ԭʼ��� (��λ)
            // float originalWidth = 1.0f; // ����ԭʼ����� 1 ����λ
            // transform.localScale = new Vector3(distance / originalWidth, 1, 1);
            Debug.LogWarning("LaserEffect Sprite Draw Mode is not Sliced or Tiled. Stretching may look incorrect.");
            // �򵥴���ֱ�����ó��ȣ�������Ҫ����sprite pivot
            transform.localScale = new Vector3(distance, 1, 1);
        }


        // --- 2. ������ȷ�Ķ��� ---
        // ���Ǽ������ Animator Controller ��������״̬��
        // "FireAnim" (��������) �� "AbsorbAnim" (���򲥷Ż���һ������)
        if (isAbsorbEffect)
        {
            animator.Play("AbsorbAnim"); // �������ն���
        }
        else
        {
            animator.Play("FireAnim"); // ���ŷ��䶯��
        }

        // --- 3. �ƻ��������� ---
        Destroy(gameObject, autoDestroyTime);
    }
}