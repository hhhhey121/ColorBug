using UnityEngine;

public class LethalLaser : MonoBehaviour//可被吸收激光
{
    // 公共方法，给 PlayerLaserAbility 脚本调用
    public void BeAbsorbed()
    {
        // 在这里播放被吸收的特效或音效
        Debug.Log("激光被吸收了!");

        // 销毁激光
        Destroy(gameObject);
    }
}