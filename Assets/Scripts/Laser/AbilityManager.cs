using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance; // 单例

    // 【重要】在Inspector中，把你的"角色A"拖到这里
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
        if (abilityUnlocked) return; // 已经解锁了，不需要重复执行

        abilityUnlocked = true;

        if (playerA_Ability != null)
        {
            // 通知角色A的脚本：你解锁了！
            playerA_Ability.UnlockAndGiveFirstCharge();
        }
    }
}