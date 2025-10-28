using UnityEngine;
using System.Collections; // ��Ҫ�����ʹ��Э�̻��ӳ�����

public class BlueSquare : MonoBehaviour // �ɴݻ�����
{
    // (��ѡ) ����һ����ը/������Ч
    public GameObject destroyEffectPrefab;

    // ��������������Inspector�е��ڵ��ӳ�����ʱ�䣨��λ���룩
    [Tooltip("�����к��ò��ڻ�������ʧ")]
    public float destroyDelay = 0.5f; // Ĭ���ӳٰ���

    private bool isBeingDestroyed = false; // ��ֹ�ظ�����

    // ������������ PlayerLaserAbility �����߼�����
    public void BeDestroyed()
    {
        // ����Ѿ������ٹ����У�����ִ��
        if (isBeingDestroyed) return;
        isBeingDestroyed = true;

        Debug.Log("��ɫ���鱻����!");

        // 1. ���̲�����Ч����������ˣ�
        if (destroyEffectPrefab != null)
        {
            Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
        }

        // 2. ����ѡ���Ƽ������̽�����ײ�壬��ֹ����ʧǰ���ܵ�ס��һ��ӵ�
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // 3. ����ѡ�����̽��� Sprite Renderer�������� destroyEffectPrefab ������ʾ����Ч��
        // SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        // if (spriteRenderer != null)
        // {
        //     spriteRenderer.enabled = false;
        // }

        // 4. ���ؼ��޸ġ�ʹ�ô��ӳٵ� Destroy
        Destroy(gameObject, destroyDelay);
    }
}