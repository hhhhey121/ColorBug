using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance; // ����

    // ��Inspector�У������"��ɫA"�ϵ�����
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
        if (abilityUnlocked) return;

        abilityUnlocked = true;

        if (playerA_Ability != null)
        {
            // -----------------------------------------------------
            // !!! ���ؼ��޸��� !!!
            // (�������������ķ���)
            playerA_Ability.UnlockAbility();
            // -----------------------------------------------------
        }
    }
}