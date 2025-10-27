using UnityEngine;

public class BlueSquare : MonoBehaviour//可摧毁蓝块
{
    // (可选) 拖入一个爆炸/碎裂特效
    public GameObject destroyEffectPrefab;

    // 公共方法，给 LaserProjectile 脚本调用
    public void BeDestroyed()
    {
        Debug.Log("蓝色方块被摧毁!");

        if (destroyEffectPrefab != null)
        {
            Instantiate(destroyEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}