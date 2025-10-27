using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance; // 单例

    // 在Inspector中，把你的"角色A"拖到这里
    public PlayerLaserAbility playerA_Ability;

    private bool abilityUnlocked = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // PlayerLife 脚本将在被激光击中时调用这个
    public void OnPlayerKilledByLaser()
    {
        if (abilityUnlocked) return;

        abilityUnlocked = true;

        if (playerA_Ability != null)
        {
            // -----------------------------------------------------
            // !!! 【关键修复】 !!!
            // (调用新重命名的方法)
            playerA_Ability.UnlockAbility();
            // -----------------------------------------------------
        }
    }
}