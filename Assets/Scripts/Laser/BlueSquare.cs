using UnityEngine;

public class BlueSquare : MonoBehaviour//�ɴݻ�����
{
    // (��ѡ) ����һ����ը/������Ч
    public GameObject destroyEffectPrefab;

    // ������������ LaserProjectile �ű�����
    public void BeDestroyed()
    {
        Debug.Log("��ɫ���鱻�ݻ�!");

        if (destroyEffectPrefab != null)
        {
            Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}