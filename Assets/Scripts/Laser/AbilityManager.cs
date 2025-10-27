using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance; // ����

    // ����Ҫ����Inspector�У������"��ɫA"�ϵ�����
    public PlayerLaserAbility playerA_Ability;

    private bool abilityUnlocked = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // PlayerLife �ű����ڱ��������ʱ�������
    public void OnPlayerKilledByLaser()
    {
        if (abilityUnlocked) return; // �Ѿ������ˣ�����Ҫ�ظ�ִ��

        abilityUnlocked = true;

        if (playerA_Ability != null)
        {
            // ֪ͨ��ɫA�Ľű���������ˣ�
            playerA_Ability.UnlockAndGiveFirstCharge();
        }
    }
}