using UnityEngine;

public class LethalLaser : MonoBehaviour//�ɱ����ռ���
{
    // ������������ PlayerLaserAbility �ű�����
    public void BeAbsorbed()
    {
        // �����ﲥ�ű����յ���Ч����Ч
        Debug.Log("���ⱻ������!");

        // ���ټ���
        Destroy(gameObject);
    }
}